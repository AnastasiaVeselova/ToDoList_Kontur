using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ToDoTasks
{
    public class ToDoTaskNotFoundException : Exception
    {
        /// <summary>
        /// Создает новый экземпляр исключения о том, что заметка не найдена
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public ToDoTaskNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Создает новый экземпляр исключения о том, что заметка не найдена
        /// </summary>
        /// <param name="noteId">Идентификатор заметки, которая не найдена</param>
        public ToDoTaskNotFoundException(Guid taskId)
            : base($"Note \"{taskId}\" is not found.")
        {
        }
    }
}
