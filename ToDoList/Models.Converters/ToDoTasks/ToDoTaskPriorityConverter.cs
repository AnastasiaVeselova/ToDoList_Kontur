using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.Tasks
{
    using Model = Models.ToDoTasks;
    using Client = Client.Models.ToDoTasks;

    public static class ToDoTaskPriorityConverter
    {
        public static Client.ToDoTaskPriority Convert(Model.ToDoTaskPriority modelPriority)
        {
            //!!!сделать человескую конвертацию
            return (Client.ToDoTaskPriority)Enum.Parse(typeof(Client.ToDoTaskPriority), modelPriority.ToString());
        }

        public static Model.ToDoTaskPriority Convert(Client.ToDoTaskPriority clientPriority)
        {
            return (Model.ToDoTaskPriority)Enum.Parse(typeof(Model.ToDoTaskPriority), clientPriority.ToString());
        }
    }
}
