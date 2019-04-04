using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ToDoTasks
{
    public class TodoTaskCreationInfo
    {
        public TodoTaskCreationInfo(Guid userId, string title, string text, DateTime endAt,  ToDoTaskPriority priority = ToDoTaskPriority.Normal)
        {
            this.UserId = userId;
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
            this.EndAt = endAt;
            this.Priority = priority;
        }

        public Guid UserId { get; }

        public string Title { get; }

        public string Text { get; }

        public DateTime EndAt { get; set; }

        public ToDoTaskPriority Priority { get; set; }
    }
}