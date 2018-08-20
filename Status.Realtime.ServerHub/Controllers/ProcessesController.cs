using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Status.Realtime.ServerHub.Hubs;
using System.Threading.Tasks;

namespace Status.Realtime.ServerHub.Controllers
{
    [Route("api/[controller]")]
    public class ProcessesController : Controller
    {
        private readonly IHubContext<ProcessHub> hub;

        public ProcessesController(IHubContext<ProcessHub> hub)
        {
            this.hub = hub;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            await hub.Clients.All.SendAsync("GetStatus");
            return Ok();
        }
    }
}