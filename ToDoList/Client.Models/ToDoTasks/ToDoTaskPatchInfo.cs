using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.ToDoTasks
{
    [DataContract]
    public class TodoTaskPatchInfo
    {
        /// <summary>
        /// Новый заголовок задачи
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Title { get; set; }

        /// <summary>
        /// Новый текст задачи
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Text { get; set; }
    }
