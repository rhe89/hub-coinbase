using Coinbase.BackgroundTasks;
using Hub.HostedServices.Tasks;
using Hub.Storage.Providers;
using Hub.Web.ApiControllers;
using Microsoft.AspNetCore.Mvc;

namespace Coinbase.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerController : WorkerControllerBase
    {
        private readonly IBackgroundTaskQueueHandler _backgroundTaskQueueHandler;

        public WorkerController(IWorkerLogProvider workerLogProvider, 
            IBackgroundTaskQueueHandler backgroundTaskQueueHandler) : base(workerLogProvider)
        {
            _backgroundTaskQueueHandler = backgroundTaskQueueHandler;
        }
        
        [HttpPost("QueueUpdateAccountsTask")]
        public IActionResult QueueUpdateAccountsTask()
        {
            _backgroundTaskQueueHandler.QueueBackgroundTask<UpdateAccountsTask>();

            return Ok();
        }
    }
}