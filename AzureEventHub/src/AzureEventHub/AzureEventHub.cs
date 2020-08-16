using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using AzureEventHub.Event;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AzureEventHub
{
    public class AzureEventHub : IEventHub, IDisposable
    {
        private readonly EventHubProducerClient _eventHubProducerClient;

        private bool _isDisposed;
        bool IEventHub.IsClosed => _eventHubProducerClient.IsClosed;

        internal AzureEventHub(EventHubProducerClient eventHubProducerClient)
        {
            _eventHubProducerClient = eventHubProducerClient;
        }

        public async Task PublishAsync<T>(T data)
        {
            var eventBatch = await _eventHubProducerClient.CreateBatchAsync();
            var message = new EventMessage<T>
            {
                Action = string.Empty,
                CacheKey = string.Empty,
                Data = data,
            };

            var payloadMessage = JsonConvert.SerializeObject(message, GlobalSetting.JsonSetting);
            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(payloadMessage)));

            await _eventHubProducerClient.SendAsync(eventBatch);
        }

        public async Task PublishAsync<T>(EventMessage<T> message)
        {
            var eventBatch = await _eventHubProducerClient.CreateBatchAsync();

            var payloadMessage = JsonConvert.SerializeObject(message, GlobalSetting.JsonSetting);
            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(payloadMessage)));

            await _eventHubProducerClient.SendAsync(eventBatch);
        }

        public async Task PublishAsync<T>(T data, string actionKey, string cacheKey)
        {
            var eventBatch = await _eventHubProducerClient.CreateBatchAsync();
            var message = new EventMessage<T>
            {
                Action = actionKey,
                CacheKey = cacheKey,
                Data = data,
            };

            var payloadMessage = JsonConvert.SerializeObject(message, GlobalSetting.JsonSetting);
            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(payloadMessage)));

            await _eventHubProducerClient.SendAsync(eventBatch);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                _isDisposed = true;
                _eventHubProducerClient.CloseAsync();
            }
        }
    }
}