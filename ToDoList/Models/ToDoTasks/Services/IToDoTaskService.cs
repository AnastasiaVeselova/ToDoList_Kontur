using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models.ToDoTasks
{
    public interface IToDoTaskService
    {
        Task<ToDoTaskInfo> CreateAsync(TodoTaskCreationInfo creationInfo, CancellationToken cancellationToken);

        Task<IReadOnlyList<ToDoTaskInfo>> SearchAsync(ToDoTaskInfoSearchQuery query, CancellationToken cancellationToken);

        Task<ToDoTask> GetAsync(Guid recordId, CancellationToken cancellationToken);

        Task<ToDoTask> PatchAsync(ToDoTaskPatchInfo patchInfo, CancellationToken cancellationToken);

        Task RemoveAsync(Guid recordId, CancellationToken cancellationToken);
    }
}