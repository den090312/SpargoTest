using System.Threading.Channels;

using SpargoTest.Interfaces;

namespace SpargoTest.Services
{
    /// <summary>
    /// Консольный терминал
    /// </summary>
    public class ConsoleTerminal : ITerminal
    {
        /// <summary>
        /// Получение значения из консоли
        /// </summary>
        /// <returns>Значение</returns>
        public string? Input() => Console.ReadLine();

        /// <summary>
        /// Вывод сообщения в консоль
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public void Output(string? message) => Console.WriteLine(message);

        /// <summary>
        /// Ввод данных определенного формата
        /// </summary>
        /// <typeparam name="T">Формат данных</typeparam>
        /// <returns>Значение определенного формата</returns>
        public T? Input<T>()
        {
            if (Tools.StringTryParse<T>(Console.ReadLine(), out T? result))
                return result;

            return default;
        }
    }
}
