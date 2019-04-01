using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Converters.Tasks
{
    using Client = Client.Models.ToDoTasks;
    using Model = Models.ToDoTasks;

    /// <summary>
    /// Предоставляет методы конвертирования аспекта сортировки заметок между клиентской и серверной моделями
    /// </summary>
    public static class ToDoTaskSortByConverter
    {
        /// <summary>
        /// Переводит аспект сортировки заметок из клиентской модели в серверную
        /// </summary>
        /// <param name="clientSortBy">Аспект сортировки заметок в клиентской модели</param>
        /// <returns>Аспект сортировки заметок в серверной модели</returns>
        public static Model.ToDoTaskSortBy Convert(Client.ToDoTaskSortBy clientSortBy)
        {
            switch (clientSortBy)
            {
                case Client.ToDoTaskSortBy.Creation:
                    return Model.ToDoTaskSortBy.Creation;

                case Client.ToDoTaskSortBy.LastUpdate:
                    return Model.ToDoTaskSortBy.LastUpdate;

                default:
                    throw new ArgumentException($"Unknown sort by value \"{clientSortBy}\".", nameof(clientSortBy));
            }
        }
    }
}