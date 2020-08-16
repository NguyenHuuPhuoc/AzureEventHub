using AzureEventHub.Event;
using ReceiveEvent.Features.User.Models;
using System.Threading.Tasks;

namespace ReceiveEvent.Features.User
{
    public interface IUserReceiveEvent
    {
        Task<Result> HandleEvent(EventMessage<UserModel> message);
    }
}