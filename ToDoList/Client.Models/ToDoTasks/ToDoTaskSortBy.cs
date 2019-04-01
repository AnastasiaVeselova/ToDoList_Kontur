using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.ToDoTasks
{ 
    public enum ToDoTaskSortBy
    {
        /// <summary>
        /// Сортировкаи по дате создания
        /// </summary>
        Creation = 0,

        /// <summary>
        /// Сортировка по дате последнего изменения
        /// </summary>
        LastUpdate
    }
}
