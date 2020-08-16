namespace ReceiveEvent.Features.User.Configuration
{
    public class UserConfig
    {
        public string ConnectionString { get; set; }
        public string HubName { get; set; }
        public string ConsumeGroup { get; set; }
    }
}