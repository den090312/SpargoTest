using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpargoTest.Interfaces;
using SpargoTest.Models;
using SpargoTest.Services;

namespace SpargoTest.Panel
{
    /// <summary>
    /// Панель создания склада
    /// </summary>
    public class WarehousePanel : IPanel<Warehouse>
    {
        /// <summary>
        /// Терминал ввода-вывода
        /// </summary>
        private readonly ITerminal _terminal;

        /// <summary>
        /// Конструктор панели создания склада
        /// </summary>
        /// <param name="terminal">Терминал ввода-вывода</param>
        /// <exception cref="ArgumentNullException">Исключение при значении null</exception>
        public WarehousePanel(ITerminal terminal) => _terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));

        /// <summary>
        /// Получение склада
        /// </summary>
        /// <returns>Склад</returns>
        public Warehouse Get()
        {
            var warehouse = new Warehouse();

            warehouse.PharmacyId = _terminal.CheckId<Pharmacy>("Введите идентификатор аптеки:");

            _terminal.Output("Введите наименование склада:");
            warehouse.Name = _terminal.Input();

            return warehouse;
        }

        /// <summary>
        /// Вывод сообщения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public void Output(string? message) => _terminal.Output(message);
    }
}
