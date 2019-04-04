using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.ToDoTasks
{
    [DataContract]
    public class ToDoTaskPatchInfo
    {
        [DataMember(IsRequired = false)]
        public string Title { get; set; }

        [DataMember(IsRequired = false)]
        public string Text { get; set; }

        [DataMember(IsRequired = false)]
        public DateTime EndAt { get; set; }

        [DataMember(IsRequired = false)]
        public ToDoTaskPriority Priority { get; set; }
    }
}
