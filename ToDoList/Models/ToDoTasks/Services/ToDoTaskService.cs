using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Models.ToDoTasks.Services
{
    public class ToDoTaskService : IToDoTaskService
    {
        private readonly IMongoCollection<ToDoTask> _toDoTasks;

        public ToDoTaskService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("ToDoTasksDB"));
            var database = client.GetDatabase("ToDoTasksDB");
            _toDoTasks = database.GetCollection<ToDoTask>("ToDoTasks");
        }

        public Task<ToDoTaskInfo> CreateAsync(TodoTaskCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var now = DateTime.UtcNow;

            var todoTask = new ToDoTask
            {
                Id = Guid.NewGuid(),
                UserId = creationInfo.UserId,
                CreatedAt = now,
                LastUpdatedAt = now,
                IsDone = false,
                Title = creationInfo.Title,
                Text = creationInfo.Text,
                Priority = creationInfo.Priority,
                EndAt = creationInfo.EndAt
            };

            _toDoTasks.InsertOne(todoTask);

            return Task.FromResult<ToDoTaskInfo>(todoTask);
        }

        public Task<ToDoTask> GetAsync(Guid toDoTaskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var task = _toDoTasks.Find<ToDoTask>(t => t.Id == toDoTaskId).FirstOrDefault();

            if (task == null)
            {
                throw new ToDoTaskNotFoundException(toDoTaskId);
            }

            return Task.FromResult(task);
        }

        public Task<ToDoTask> PatchAsync(ToDoTaskPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var task = _toDoTasks.Find<ToDoTask>(t => t.Id == patchInfo.Id).FirstOrDefault();

            if (task == null)
            {
                throw new ToDoTaskNotFoundException(patchInfo.Id);
            }

            var updated = false;

            if (patchInfo.Title != null)
            {
                task.Title = patchInfo.Title;
                updated = true;
            }
            if (patchInfo.Text != null)
            {
                task.Text = patchInfo.Text;
                updated = true;
            }
            if (patchInfo.Priority != null)
            {
                task.Priority = patchInfo.Priority.Value;
                updated = true;
            }
            if (patchInfo.EndAt != null)
            {
                task.EndAt = patchInfo.EndAt.Value;
                updated = true;
            }
            if (patchInfo.IsDone != null)
            {
                task.IsDone = patchInfo.IsDone.Value;
                updated = true;
            }

            if (updated)
            {
                task.LastUpdatedAt = DateTime.UtcNow;
                _toDoTasks.ReplaceOne(t => t.Id == task.Id, task);
            }

            return Task.FromResult(task);
        }

        public Task RemoveAsync(Guid taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var task = _toDoTasks.Find<ToDoTask>(t => t.Id == taskId).FirstOrDefault();
            if (task == null)
            {
                throw new ToDoTaskNotFoundException(taskId);
            }

            _toDoTasks.DeleteOne<ToDoTask>(t => t.Id == taskId);

            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<ToDoTaskInfo>> SearchAsync(ToDoTaskInfoSearchQuery query, CancellationToken cancelltionToken)
        {
            return Task.FromResult<IReadOnlyList<ToDoTaskInfo>>(new List<ToDoTaskInfo>());
        }
    }
}