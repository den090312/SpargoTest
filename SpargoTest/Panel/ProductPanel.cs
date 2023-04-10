using SpargoTest.Interfaces;
using SpargoTest.Models;
using SpargoTest.Services;

namespace SpargoTest.Panel
{
    /// <summary>
    /// Панель создания товара
    /// </summary>
    public class ProductPanel : IPanel<Product>
    {
        /// <summary>
        /// Терминал ввода-вывода
        /// </summary>
        private readonly ITerminal _terminal;

        /// <summary>
        /// Конструктор панели создания товара
        /// </summary>
        /// <param name="terminal">Терминал ввода-вывода</param>
        /// <exception cref="ArgumentNullException">Исключение при значении null</exception>
        public ProductPanel(ITerminal terminal) => _terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));

        /// <summary>
        /// Создание товара
        /// </summary>
        /// <returns>Товар</returns>
        public Product Get()
        {
            var prodcut = new Product();

            _terminal.Output("Введите наименование:");
            prodcut.Name = _terminal.Input();

            return prodcut;
        }

        /// <summary>
        /// Вывод сообщения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public void Output(string? message) => _terminal.Output(message);
    }
}
