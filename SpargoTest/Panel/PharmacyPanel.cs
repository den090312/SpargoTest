using SpargoTest.Interfaces;
using SpargoTest.Models;
using SpargoTest.Services;

namespace SpargoTest.Panel
{
    /// <summary>
    /// Панель создания аптеки
    /// </summary>
    public class PharmacyPanel : IPanel<Pharmacy>
    {
        /// <summary>
        /// Терминал ввода-вывода
        /// </summary>
        private ITerminal _terminal;

        /// <summary>
        /// Конструктор панели создания аптеки
        /// </summary>
        /// <param name="terminal">Терминал ввода-вывода</param>
        /// <exception cref="ArgumentNullException">Исключение при значении null</exception>
        public PharmacyPanel(ITerminal terminal) => _terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));

        /// <summary>
        /// Получение аптеки
        /// </summary>
        /// <returns>Аптека</returns>
        public Pharmacy Get()
        {
            var pharmacy = new Pharmacy();

            _terminal.Output("Введите наименование:");
            pharmacy.Name = _terminal.Input();

            _terminal.Output("Введите адрес:");
            pharmacy.Address = _terminal.Input();

            _terminal.Output("Введите телефон:");

            var phoneNumber = _terminal.Input<int>();

            while (phoneNumber == default)
            {
                _terminal.Output("Введите число без пробелов между цифрами:");
                phoneNumber = _terminal.Input<int>();
            }

            pharmacy.PhoneNumber = phoneNumber.ToString();

            return pharmacy;
        }

        /// <summary>
        /// Вывод сообщения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public void Output(string? message) => _terminal.Output(message);
    }
}
