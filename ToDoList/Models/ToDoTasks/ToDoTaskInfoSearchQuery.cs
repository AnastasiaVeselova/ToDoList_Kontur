using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ToDoTasks
{
    /// <summary>
    /// Параметры поиска заметок
    /// </summary>
    public class ToDoTaskInfoSearchQuery
    {
        /// <summary>
        /// Позиция, начиная с которой нужно производить поиск
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Максимальеное количество заметок, которое нужно вернуть
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Пользователь, которому принадлежит заметка
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Минимальная дата создания заметки
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Максимальная дата создания заметки
        /// </summary>
        public DateTime? CreatedTo { get; set; }

        /// <summary>
        /// Тип сортировки
        /// </summary>
        public SortType? Sort { get; set; }

        /// <summary>
        /// Аспект заметки, по которому нужно искать
        /// </summary>
        public ToDoTaskSortBy? SortBy { get; set; }

        public ToDoTaskPriority? Priority { get; set; }

        public DateTime? EndAt { get; set; }

        public bool? IsDone { get; set; }
    }
}
