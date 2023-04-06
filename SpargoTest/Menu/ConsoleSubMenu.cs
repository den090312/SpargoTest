using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpargoTest.Interfaces;

namespace SpargoTest.Menu
{
    /// <summary>
    /// Консольное подменю
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    public class ConsoleSubMenu : ISubMenu
    {
        /// <summary>
        /// Заголовок меню
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Список пунктов подменю
        /// </summary>
        public IEnumerable<string?> Items { get; set; } = new List<string?>();
    }
}
