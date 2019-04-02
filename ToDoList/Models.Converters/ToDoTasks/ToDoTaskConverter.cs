using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.ToDoTasks
{
    using Client = Client.Models.ToDoTasks;
    using Model = Models.ToDoTasks;


    public static class ToDoTaskConverter
    {
        public static Client.ToDoTask Convert(Model.ToDoTask modelTask)
        {
            if (modelTask == null)
            {
                throw new ArgumentNullException(nameof(modelTask));
            }

            return new Client.ToDoTask
            {
                Id = modelTask.Id.ToString(),
                UserId = modelTask.UserId.ToString(),
                Title = modelTask.Title,
                Text = modelTask.Text,
                CreatedAt = modelTask.CreatedAt,
                LastUpdatedAt = modelTask.LastUpdatedAt,
                Priority = ToDoTaskPriorityConverter.Convert(modelTask.Priority),
                IsDone = modelTask.IsDone,
                EndAt = modelTask.EndAt,
            };
        }
    }
}
