using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.ToDoTasks
{
     public class ToDoTaskInfo
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public string Title { get; set; }

        public DateTime EndAt { get; set; }

        public ToDoTaskPriority Priority { get; set; }

    }
}
