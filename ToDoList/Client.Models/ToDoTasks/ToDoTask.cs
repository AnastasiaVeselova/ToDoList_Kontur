﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.ToDoTasks
{
    public class ToDoTask : ToDoTaskInfo
    {
        /// <summary>
        /// Описание задачи
        /// </summary>
        public string Text { get; set; }
    }
}
