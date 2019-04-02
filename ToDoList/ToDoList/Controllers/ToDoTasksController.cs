using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Converters.ToDoTasks;
using Models.ToDoTasks;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTasksController : ControllerBase
    {
        private readonly IToDoTaskService tasks;

        public ToDoTasksController(IToDoTaskService taskRepository)
        {
            this.tasks = taskRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync([FromBody] Client.Models.ToDoTasks.ToDoBuildInfo creationInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (creationInfo == null)
            {
                return this.BadRequest();
            }
            var userId = HttpContext.Items["userId"].ToString();
            var modelCreationInfo = ToDoTaskCreateInfoConverter.Convert(userId, creationInfo);
            var modelTaskInfo = await this.tasks.CreateAsync(modelCreationInfo, cancellationToken);
            var clientTaskInfo = ToDoTaskInfoConverter.Convert(modelTaskInfo);

            var routeParams = new Dictionary<string, object>
            {
                { "todoTaskId", clientTaskInfo.Id }
            };

            return this.CreatedAtRoute("GetToDoTaskRoute", routeParams, clientTaskInfo);
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

            Models.ToDoTasks.ToDoTask task = null;
            try
            {
                task = await this.tasks.GetAsync(taskGuid, cancellationToken);
            }
            catch (Models.ToDoTasks.ToDoTaskNotFoundException)
            {
                return this.NotFound();
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
    }
}