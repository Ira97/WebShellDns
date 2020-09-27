using System;

namespace WebShell.Models
{
    /// <summary>
    /// Команда WebShell
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Текст комманды
        /// </summary>
        public string TextCommand { get; set; }
        /// <summary>
        /// Время
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// Ответ
        /// </summary>
        public string ResponseCommand { get; set; }
    }
}
