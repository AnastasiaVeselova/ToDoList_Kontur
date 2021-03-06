﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.ToDoTasks
{
    using Client = Client.Models.ToDoTasks;
    using Model = Models.ToDoTasks;

    public static class ToDoTaskPatchConverter
    {
        public static Model.ToDoTaskPatchInfo Convert(Guid taskId, Client.ToDoTaskPatchInfo clientPatch)
        {
            if (clientPatch == null)
            {
                throw new ArgumentNullException(nameof(clientPatch));
            }

            return new Model.ToDoTaskPatchInfo(
                taskId,
                clientPatch.Title,
                clientPatch.Text,
                clientPatch.EndAt,
                ToDoTaskPriorityConverter.Convert(clientPatch.Priority));
        }
    }
}
