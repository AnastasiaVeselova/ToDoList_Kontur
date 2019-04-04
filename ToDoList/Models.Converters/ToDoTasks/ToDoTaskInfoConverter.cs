using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.ToDoTasks
{
    using Model = Models.ToDoTasks;
    using Client = Client.Models.ToDoTasks;

    public static class ToDoTaskInfoConverter
    {
        public static Client.ToDoTaskInfo Convert(Model.ToDoTaskInfo modelTaskInfo)
        {
            if (modelTaskInfo == null)
            {
                throw new ArgumentNullException(nameof(modelTaskInfo));
            }

            return new Client.ToDoTaskInfo
            {
                Id = modelTaskInfo.Id.ToString(),
                UserId = modelTaskInfo.UserId.ToString(),
                Title = modelTaskInfo.Title,
                CreatedAt = modelTaskInfo.CreatedAt,
                LastUpdatedAt = modelTaskInfo.LastUpdatedAt,
                Priority = ToDoTaskPriorityConverter.Convert(modelTaskInfo.Priority),
                EndAt = modelTaskInfo.EndAt
            };
        }
    }
}
