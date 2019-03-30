﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ToDoTasks
{
    /// <summary>
    /// Информация о задаче
    /// </summary>
    public class ToDoTaskInfo
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которому принадлежит задача
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Дата создания задачи
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// Название задачи
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Теги задачи
        /// </summary>
        public IReadOnlyList<string> Tags { get; set; }
    }
}