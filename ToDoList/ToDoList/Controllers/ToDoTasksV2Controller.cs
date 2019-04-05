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
using ToDoList.Errors;
using Microsoft.AspNetCore.Http;

namespace ToDoList.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [Route("api/[controller]")]
    public class ToDoTasksV2Controller : Controller
    {
        private readonly IToDoTaskService tasks;

        public ToDoTasksV2Controller(IToDoTaskService tasks)
        {
            this.tasks = tasks;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync([FromBody] Client.Models.ToDoTasks.ToDoTaskBuildInfo buildInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (buildInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("ToDoTasksBuildInfo");
                return this.BadRequest(error);
            }

            var userIdClame = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var modelCreationInfo = ToDoTaskBuildInfoConverter.Convert(userIdClame.Value, buildInfo);

            var modelTaskInfo = await this.tasks.CreateAsync(modelCreationInfo, cancellationToken);

            var clientTaskInfo = ToDoTaskInfoConverter.Convert(modelTaskInfo);

            var routeParams = new Dictionary<string, object>
            {
                { "taskId", clientTaskInfo.Id }
            };

            return this.CreatedAtRoute("GetTaskRouteV2", routeParams, clientTaskInfo);
        }

        [HttpGet]
        [Route("{taskId}", Name = "GetTaskRouteV2")]
        public async Task<IActionResult> GetTaskAsync([FromRoute] string taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(taskId, out var modelToDoTaskId))
            {
                var error = ServiceErrorResponses.ToDoTaskNotFound(taskId);
                return this.NotFound(error);
            }
          
            ToDoTask modelTask = null;

            try
            {
                modelTask = await this.tasks.GetAsync(modelToDoTaskId, cancellationToken);
            }
            catch (ToDoTaskNotFoundException)
            {
                var error = ServiceErrorResponses.ToDoTaskNotFound(taskId);
                return NotFound(error);
            }

            var clientTask = ToDoTaskConverter.Convert(modelTask);

            var userIdRequest = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdRequest.Value != clientTask.UserId)
            {
                var error = ServiceErrorResponses.AccessDenied();
                return StatusCode(StatusCodes.Status403Forbidden, error);
            }
           
            return this.Ok(clientTask);
        }

        [HttpDelete]
        [Route("{taskId}")]
        public async Task<IActionResult> RemoveTaskAsync([FromRoute] string taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(taskId, out var modelToDoTaskId))
            {
                var error = ServiceErrorResponses.ToDoTaskNotFound(taskId);
                return this.NotFound(error);
            }

            ToDoTask modelTask = null;

            try
            {
                modelTask = await this.tasks.GetAsync(modelToDoTaskId, cancellationToken);
            }
            catch (ToDoTaskNotFoundException)
            {
                var error = ServiceErrorResponses.ToDoTaskNotFound(taskId);
                return NotFound(error);
            }

            var clientTask = ToDoTaskConverter.Convert(modelTask);

            var userIdRequest = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdRequest.Value != clientTask.UserId)
            {
                var error = ServiceErrorResponses.AccessDenied();
                return StatusCode(StatusCodes.Status403Forbidden, error);
            }

            try
            {
                await tasks.RemoveAsync(modelToDoTaskId, cancellationToken);
            }
            catch (ToDoTaskNotFoundException)
            {
                var error = ServiceErrorResponses.ToDoTaskNotFound(taskId);
                return NotFound(error);
            }

            return NoContent();
        }

        [HttpPatch]
        [Route("{taskId}")]
        public async Task<IActionResult> PatchTaskAsync([FromRoute] string taskId, [FromBody]Client.Models.ToDoTasks.ToDoTaskPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(taskId, out var modelToDoTaskId))
            {
                var error = ServiceErrorResponses.ToDoTaskNotFound(taskId);
                return this.NotFound(error);
            }

            ToDoTask modelTask = null;

            try
            {
                modelTask = await this.tasks.GetAsync(modelToDoTaskId, cancellationToken);
            }
            catch (ToDoTaskNotFoundException)
            {
                var error = ServiceErrorResponses.ToDoTaskNotFound(taskId);
                return NotFound(error);
            }

            var clientTask = ToDoTaskConverter.Convert(modelTask);

            var userIdRequest = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (userIdRequest.Value != clientTask.UserId)
            {
                var error = ServiceErrorResponses.AccessDenied();
                return StatusCode(StatusCodes.Status403Forbidden, error);
            }

            var modelPatchInfo = ToDoTaskPatchConverter.Convert(modelToDoTaskId, patchInfo);

            ToDoTask patchTask = null;

            try
            {
                patchTask = await tasks.PatchAsync(modelPatchInfo, cancellationToken);
            }
            catch (ToDoTaskNotFoundException)
            {
                var error = ServiceErrorResponses.ToDoTaskNotFound(taskId);
                return NotFound(error);
            }

            var clientPatchInfo = ToDoTaskConverter.Convert(patchTask);

            return Ok(clientPatchInfo);
        }
    }
}