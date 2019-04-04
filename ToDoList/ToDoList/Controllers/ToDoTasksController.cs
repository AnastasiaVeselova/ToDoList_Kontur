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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Models.Users.Services;
using Models.Users;

namespace ToDoList.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [Route("api/[controller]")]
    public class ToDoTasksController : Controller
    {
        private readonly IToDoTaskService tasks;
        private readonly IUserService users;

        public ToDoTasksController(IToDoTaskService tasks, IUserService users)
        {
            this.tasks = tasks;
            this.users = users;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync([FromBody] Client.Models.ToDoTasks.ToDoTaskBuildInfo buildInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (buildInfo == null)
            {
                return this.BadRequest();
            }

            var userIdClame = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var modelCreationInfo = ToDoTaskBuildInfoConverter.Convert(userIdClame.Value, buildInfo);
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

            if (!Guid.TryParse(taskId, out var modelToDoTaskId))
            {
                return this.NotFound();
            }

            var loginIdentifier = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            User user = null;

            try
            {
                user = await users.GetAsync(loginIdentifier.Value, cancellationToken);
            }
            catch
            {
                return BadRequest();
            }

            if (user.Id != modelToDoTaskId)
            {
                return NotFound();
            }

            ToDoTask modelTask = null;

            try
            {
                modelTask = await this.tasks.GetAsync(modelToDoTaskId, cancellationToken);
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

            if (!Guid.TryParse(taskId, out var modelToDoTaskId))
            {
                return NotFound();
            }

            var loginIdentifier = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            User user = null;

            try
            {
                user = await users.GetAsync(loginIdentifier.Value, cancellationToken);
            }
            catch
            {
                return BadRequest();
            }

            if (user.Id != modelToDoTaskId)
            {
                return NotFound();
            }

            try
            {
                await tasks.RemoveAsync(modelToDoTaskId, cancellationToken);
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