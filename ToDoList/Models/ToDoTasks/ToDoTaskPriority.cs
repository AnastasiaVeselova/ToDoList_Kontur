using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ToDoTasks
{
    public enum ToDoTaskPriority
    {
        Urgent = 3,
        Highest = 2,
        High = 1,
        Normal = 0,
        Low = -1,
        Lowest = -2
    }
}
