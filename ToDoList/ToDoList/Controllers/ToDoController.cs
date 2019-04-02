using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ToDoTasks;
using Models.Converters.ToDoTasks;

namespace ToDoListAPI.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private readonly IToDoTaskService tasks;

        public TasksController(IToDoTaskService taskRepository)
        {
            this.tasks = taskRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync([FromBody] Client.Models.ToDoTasks.ToDoTasksBuildInfo buildInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (buildInfo == null)
            {
                return this.BadRequest();
            }

            var userId = Guid.Empty.ToString();
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var userId = HttpContext.Items["userId"].ToString();
            var modelCreationInfo = ToDoTaskBuildInfoConverter.Convert(userId, buildInfo);
            var modelTaskInfo = await this.tasks.CreateAsync(modelCreationInfo, cancellationToken);
            var clientTaskInfo = ToDoTaskInfoConverter.Convert(modelTaskInfo);

            var routeParams = new Dictionary<string, object>
            {
                { "taskId", clientTaskInfo.Id }
            };

            var a = this.CreatedAtRoute("GetTaskRoute", routeParams, clientTaskInfo);
            return a;
        }

        [HttpGet]
        [Route("{taskId}", Name = "GetTaskRoute")]
        public async Task<IActionResult> GetTaskAsync([FromRoute] string taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(taskId, out var modeltaskId))
            {
                return this.NotFound();
            }

            Models.ToDoTasks.ToDoTask modelTask = null;
            try
            {
                modelTask = await this.tasks.GetAsync(modeltaskId, cancellationToken);
            }
            catch (Models.ToDoTasks.ToDoTaskNotFoundException)
            {
                return this.NotFound();
            }

            var clientTask = ToDoTaskConverter.Convert(modelTask);

            return this.Ok(clientTask);
        }

        [HttpDelete]
        [Route("{taskId}")]
        public async Task<IActionResult> RemoveTaskAsync([FromRoute] string taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(taskId, out var taskGuid))
            {
                return NotFound();
            }

            try
            {
                await tasks.RemoveAsync(taskGuid, cancellationToken);
            }
            catch (Models.ToDoTasks.ToDoTaskNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch]
        [Route("{taskId}")]
        public async Task<IActionResult> PatchTaskAsync([FromRoute] string taskId, [FromBody]Client.Models.ToDoTasks.ToDoTaskPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (patchInfo == null)
            {
                return BadRequest();
            }
            if (!Guid.TryParse(taskId, out var taskGuid))
            {
                return NotFound();
            }

            var modelPatchInfo = ToDoTaskPatchConverter.Convert(taskGuid, patchInfo);

            Models.ToDoTasks.ToDoTask patchTask = null;

            try
            {
                patchTask = await tasks.PatchAsync(modelPatchInfo, cancellationToken);
            }
            catch (Models.ToDoTasks.ToDoTaskNotFoundException)
            {
                return NotFound();
            }

            var clientTask = ToDoTaskConverter.Convert(patchTask);
            return Ok(clientTask);
        }
    }
}