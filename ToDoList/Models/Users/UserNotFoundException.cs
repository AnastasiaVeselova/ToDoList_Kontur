using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Users
{
    /// <summary>
    /// Исключение, которое возникает при попытке получить несуществующую заметку
    /// </summary>
    public class NoteNotFoundExcepction : Exception
    {
        /// <summary>
        /// Создает новый экземпляр исключения о том, что заметка не найдена
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        public NoteNotFoundExcepction(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Создает новый экземпляр исключения о том, что заметка не найдена
        /// </summary>
        /// <param name="noteId">Идентификатор заметки, которая не найдена</param>
        public NoteNotFoundExcepction(Guid noteId)
            : base($"Note \"{noteId}\" is not found.")
        {
        }
    }
}