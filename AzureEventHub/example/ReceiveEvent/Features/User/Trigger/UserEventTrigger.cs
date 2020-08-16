using AzureEventHub.Event;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReceiveEvent.Features.User.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ReceiveEvent.Features.User.Trigger
{
    public class UserEventTrigger
    {
        private readonly IUserReceiveEvent _userReceiveEvent;

        public UserEventTrigger(IUserReceiveEvent userReceiveEvent)
        {
            _userReceiveEvent = userReceiveEvent;
        }

        [FunctionName("UserEvent")]
        public async Task Run([EventHubTrigger("%UserConfig:HubName%", Connection = "UserConfig:ConnectionString",
            ConsumerGroup = "%UserConfig:ConsumeGroup%")] EventData[] events, ILogger log)
        {
            foreach (EventData @event in events)
            {
                try
                {
                    log.LogInformation($"Event Hub trigger function processed a message");

                    string messageBody = Encoding.UTF8.GetString(@event.Body.Array,
                        @event.Body.Offset, @event.Body.Count);
                    if (!string.IsNullOrEmpty(messageBody))
                    {
                        var message = JsonConvert.DeserializeObject<EventMessage<UserModel>>(messageBody);
                        if (message != null)
                        {
                            var response = await _userReceiveEvent.HandleEvent(message);
                            if (!response.Succeeded)
                            {
                                log.LogError(response.ErrorMessage);
                                log.LogInformation($"Error occurred when Event Hub trigger function processed the message:" +
                                    $" {messageBody}");
                            }
                            else
                                log.LogInformation($"Event Hub trigger function succeeded the message: {messageBody}");
                        }
                    }
                    log.LogInformation($"Event Hub trigger function has finished");
                }
                catch (Exception ex)
                {
                    log.LogError(ex, "Exception occurred when Event Hub trigger function processed a message");
                }
            }
        }
    }
}