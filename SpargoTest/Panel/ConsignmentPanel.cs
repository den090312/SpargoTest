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
    /// Панель создания партии товара
    /// </summary>
    public class ConsignmentPanel : IPanel<Consignment>
    {
        /// <summary>
        /// Терминал ввода-вывода
        /// </summary>
        private readonly ITerminal _terminal;

        /// <summary>
        /// Конструктор панели создания партии товара
        /// </summary>
        /// <param name="terminal">Терминал ввода-вывода</param>
        /// <exception cref="ArgumentNullException">Исключение при значении null</exception>
        public ConsignmentPanel(ITerminal terminal) => _terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));

        /// <summary>
        /// Получение партии
        /// </summary>
        /// <returns>Партия товара</returns>
        public Consignment Get()
        {
            var consignment = new Consignment();

            consignment.ProductId = _terminal.CheckId<Product>("Введите идентификатор товара:");
            consignment.WarehouseId = _terminal.CheckId<Warehouse>("Введите идентификатор склада:");

            return consignment;
        }

        /// <summary>
        /// Вывод сообщения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public void Output(string? message) => _terminal.Output(message);
    }
}
