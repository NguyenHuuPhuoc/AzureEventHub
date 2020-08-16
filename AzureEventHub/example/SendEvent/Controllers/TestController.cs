using AzureEventHub;
using Microsoft.AspNetCore.Mvc;
using SendEvent.Models;
using System.Threading.Tasks;

namespace SendEvent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly IEventHubFactory _eventHubFactory;

        public TestController(IEventHubFactory eventHubFactory)
        {
            _eventHubFactory = eventHubFactory;
        }

        [HttpPost]
        public async Task<IActionResult> TestSendEvent([FromBody] UserModel request)
        {
            if (ModelState.IsValid)
                return BadRequest();

            var key = "test";
            var eventHub = _eventHubFactory.CreateAzureEventHub(key);
            await eventHub.PublishAsync<UserModel>(request, Action.AddNew.ToString(), GenerateCacheKey(request.Name));

            return Ok();
        }

        private static string GenerateCacheKey(string name)
        {
            return $"User_{name}";
        }
    }
}