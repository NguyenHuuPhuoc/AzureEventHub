using Azure.Messaging.EventHubs.Producer;
using System;
using System.Collections.Concurrent;

namespace AzureEventHub
{
    public class AzureHubFactory : IEventHubFactory, IDisposable
    {
        private static readonly ConcurrentDictionary<string, IEventHub> _hubs = new ConcurrentDictionary<string, IEventHub>();
        private readonly string _connectionString;

        private readonly object _syncLock = new object();
        private bool _isDisposed = false;

        internal AzureHubFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEventHub CreateAzureEventHub(string key)
        {
            lock (_syncLock)
            {
                if (_hubs.ContainsKey(key) && _hubs[key].IsClosed)
                {
                    _hubs.TryRemove(key, out _);
                }

                var eventBus = _hubs.GetOrAdd(key, (_) =>
                {
                    var client = new EventHubProducerClient(_connectionString, key);

                    return new AzureEventHub(client);
                });

                return eventBus;
            }
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
                foreach (var item in _hubs.Values)
                {
                    (item as IDisposable)?.Dispose();
                }
            }
        }
    }
}