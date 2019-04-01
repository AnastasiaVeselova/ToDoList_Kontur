using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ToDoTasks
{
    public class ToDoTaskPatchInfo
    {
        /// <summary>
        /// Создает новый экземпляр объекта, описывающего изменение заметки
        /// </summary>
        /// <param name="recordId">Идентификатор задачи, которую нужно изменить</param>
        /// <param name="title">Новый заголовок задачи</param>
        /// <param name="text">Новый текст задачи</param>
        public ToDoTaskPatchInfo(Guid id, string title = null, string text = null,DateTime? endAt = null, bool? isDone = null, ToDoTaskPriority? priority = null)
        {
            this.Id = id;
            this.Title = title;
            this.Text = text;
        }

        /// <summary>
        /// Идентификатор задачи, которую нужно изменить
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Новый заголовок задачи
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Новый текст задачи
        /// </summary>
        public string Text { get; set; }

        public DateTime? EndAt { get; set; }

        public bool? IsDone { get; set; }

        public ToDoTaskPriority? Priority { get; set; }             
    }
}
