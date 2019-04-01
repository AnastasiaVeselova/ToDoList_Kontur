using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.Tasks
{
    using Client = Client.Models.ToDoTasks;
    using Model = Models.ToDoTasks;

    public static class ToDoTaskCreateInfoConverter
    {
        public static Model.TodoTaskCreationInfo Convert(string clientUserId, Client.TodoBuildInfo clientBuildInfo)
        {
            if (clientUserId == null)
            {
                throw new ArgumentNullException(nameof(clientUserId));
            }

            if (clientBuildInfo == null)
            {
                throw new ArgumentNullException(nameof(clientBuildInfo));
            }

            if (!Guid.TryParse(clientUserId, out var modelUserId))
            {
                throw new ArgumentException($"The client user id \"{clientUserId}\" is invalid.", nameof(clientUserId));
            }

            return new Model.TodoTaskCreationInfo(modelUserId,clientBuildInfo.Title, clientBuildInfo.Text, clientBuildInfo.EndAt, ToDoTaskPriorityConverter.Convert(clientBuildInfo.Priority));
        }
    }
}
