namespace AzureEventHub
{
    public interface IEventHubFactory
    {
        IEventHub CreateAzureEventHub(string key);
    }
}