using System;

namespace AzureEventHub.Event
{
    public class EventMessage<T>
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public string CacheKey { get; set; }

        public string Action { get; set; }

        public T Data { get; set; }

        public EventMessage()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
    }
}