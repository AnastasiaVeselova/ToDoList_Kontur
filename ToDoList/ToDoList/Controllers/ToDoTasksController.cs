﻿using System;
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
                var error = ServiceErrorResponses.BodyIsMissing("ToDoTasksBuildInfo");
                return this.BadRequest(error);
            }

            var userLoginRequest = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            User user = null;

            try
            {
                user = await users.GetAsync(userLoginRequest.Value, cancellationToken);
            }
            catch
            {
                var error = ServiceErrorResponses.UserNotFound(userLoginRequest.Value);
                return BadRequest(error);
            }

            //добавить try-catch в случае если аргументы null
            var modelCreationInfo = ToDoTaskBuildInfoConverter.Convert(user.Id.ToString(), buildInfo);

            var modelTaskInfo = await this.tasks.CreateAsync(modelCreationInfo, cancellationToken);

            var clientTaskInfo = ToDoTaskInfoConverter.Convert(modelTaskInfo);

            var routeParams = new Dictionary<string, object>
            {
                { "taskId", clientTaskInfo.Id }
            };

            return this.CreatedAtRoute("GetTaskRoute", routeParams, clientTaskInfo);
        }

        [HttpGet]
        [Route("{taskId}", Name = "GetTaskRoute")]
        public async Task<IActionResult> GetTaskAsync([FromRoute] string taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!Guid.TryParse(taskId, out var modelToDoTaskId))
            {
                var error = ServiceErrorResponses.ToDoTaskNotFound(taskId);
                return this.NotFound(error);
            }

            var userLoginRequest = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            User user = null;

            try
            {
                user = await users.GetAsync(userLoginRequest.Value, cancellationToken);
            }
            catch
            {
                var error = ServiceErrorResponses.UserNotFound(userLoginRequest.Value);
                return BadRequest(error);
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


            if (user.Id != modelTask.UserId)
            {
                var error = ServiceErrorResponses.AccessDenied();
                return StatusCode(StatusCodes.Status403Forbidden, error);
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
                var error = ServiceErrorResponses.ToDoTaskNotFound(taskId);
                return this.NotFound(error);
            }

            var userLoginRequest = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            User user = null;

            try
            {
                user = await users.GetAsync(userLoginRequest.Value, cancellationToken);
            }
            catch
            {
                var error = ServiceErrorResponses.UserNotFound(userLoginRequest.Value);
                return BadRequest(error);
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


            if (user.Id != modelTask.UserId)
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

            var userLoginRequest = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            User user = null;

            try
            {
                user = await users.GetAsync(userLoginRequest.Value, cancellationToken);
            }
            catch
            {
                var error = ServiceErrorResponses.UserNotFound(userLoginRequest.Value);
                return BadRequest(error);
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


            if (user.Id != modelTask.UserId)
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

            var clientTask = ToDoTaskConverter.Convert(patchTask);

            return Ok(clientTask);
        }
    }
}