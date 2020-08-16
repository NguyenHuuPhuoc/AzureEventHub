using AzureEventHub.Event;
using ReceiveEvent.Features.User.Models;
using System.Threading.Tasks;

namespace ReceiveEvent.Features.User
{
    public class UserReceiveEvent : IUserReceiveEvent
    {
        public async Task<Result> HandleEvent(EventMessage<UserModel> message)
        {
            Action.TryParse(message.Action, out Action action);
            return await ReceiveMessage(action, message);
        }

        private async Task<Result> ReceiveMessage(Action action, EventMessage<UserModel> message)
        {
            await Task.CompletedTask;
            switch (action)
            {
                case Action.AddNew:
                    return new Result { Succeeded = true, ErrorMessage = "" };

                case Action.Update:
                    return new Result { Succeeded = true, ErrorMessage = "" };

                case Action.Delete:
                    return new Result { Succeeded = true, ErrorMessage = "" };

                default:
                    return new Result { Succeeded = false, ErrorMessage = "Error occurred when Sync: The Action is not found!" };
            }
        }
    }
}