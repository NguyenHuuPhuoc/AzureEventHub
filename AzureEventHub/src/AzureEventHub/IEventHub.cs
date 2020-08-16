using AzureEventHub.Event;
using System.Threading.Tasks;

namespace AzureEventHub
{
    public interface IEventHub
    {
        internal bool IsClosed { get; }

        Task PublishAsync<T>(T data);

        Task PublishAsync<T>(EventMessage<T> message);

        Task PublishAsync<T>(T data, string actionKey, string cacheKey);
    }
}