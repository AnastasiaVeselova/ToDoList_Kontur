using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ToDoTasks
{
    public class ToDoTaskPatchInfo
    {
        public ToDoTaskPatchInfo(Guid id, string title = null, string text = null, DateTime? endAt = null, ToDoTaskPriority? priority = null)
        {
            this.Id = id;
            this.Title = title;
            this.Text = text;
            this.EndAt = endAt;
            this.Priority = priority;
        }
        public Guid Id { get; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime? EndAt { get; set; }

        public ToDoTaskPriority? Priority { get; set; }
    }
}
