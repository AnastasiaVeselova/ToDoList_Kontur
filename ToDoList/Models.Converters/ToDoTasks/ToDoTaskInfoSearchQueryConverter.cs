using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.Tasks
{
    using Model = Models.ToDoTasks;
    using Client = Client.Models.ToDoTasks;

    public static class ToDoTaskInfoSearchQueryConverter
    {
        public static Model.ToDoTaskInfoSearchQuery Convert(Client.ToDoTaskInfoSearchQuery clientQuery)
        {
            if (clientQuery == null)
            {
                throw new ArgumentNullException(nameof(clientQuery));
            }

            var modelSort = clientQuery.Sort.HasValue ?
                SortTypeConverter.Convert(clientQuery.Sort.Value) :
                (Models.SortType?)null;

            var modelSortBy = clientQuery.SortBy.HasValue ?
                ToDoTaskSortByConverter.Convert(clientQuery.SortBy.Value) :
                (Model.ToDoTaskSortBy?)null;

            var priority = clientQuery.Priority.HasValue ? 
                ToDoTaskPriorityConverter.Convert(clientQuery.Priority.Value)
                : (Model.ToDoTaskPriority?)null;

            return new Model.ToDoTaskInfoSearchQuery
            {
                CreatedFrom = clientQuery.CreatedFrom,
                CreatedTo = clientQuery.CreatedTo,

                Limit = clientQuery.Limit,
                Offset = clientQuery.Offset,

                Priority = priority,
                EndAt = clientQuery.EndAt,
                IsDone = clientQuery.IsDone,

                Sort = modelSort,
                SortBy = modelSortBy
            };
        }
    }
}
