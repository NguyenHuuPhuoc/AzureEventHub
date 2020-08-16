using Microsoft.Extensions.DependencyInjection;

namespace AzureEventHub
{
    public static class EventHubConfiguration
    {
        public static IServiceCollection AddEventHub(this IServiceCollection services, string eventHubConnectionString)
        {
            services.AddSingleton<IEventHubFactory, AzureHubFactory>((provider) => new AzureHubFactory(eventHubConnectionString));
            return services;
        }
    }
}