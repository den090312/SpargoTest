using System.Threading.Channels;

using SpargoTest.Interfaces;
using SpargoTest.Repository;

namespace SpargoTest.Services
{
    /// <summary>
    /// Консольный терминал
    /// </summary>
    public class ConsoleTerminal : ITerminal
    {
        /// <summary>
        /// Интерфейс операций с объектами
        /// </summary>
        private readonly ICrud _crud;

        /// <summary>
        /// Конструктор консольного терминала
        /// </summary>
        /// <param name="crud">нтерфейс операций с объектами</param>
        /// <exception cref="ArgumentNullException">Исключение при значении null</exception>
        public ConsoleTerminal(ICrud crud) => _crud = crud ?? throw new ArgumentNullException(nameof(crud));

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

        /// <summary>
        /// Вывод сообщения об успехе
        /// </summary>
        public void WriteSuccessMessage()
        {
            Output("Операция выполнена успешно. Нажмите Enter.");
            Input();
        }

        /// <summary>
        /// Вывод сообщения об итогах операции
        /// </summary>
        /// <param name="result">Результат операции</param>
        public void WriteResultMessage(Result result)
        {
            if (!result.Success)
                WriteError(result.ErrorMessage);

            WriteSuccessMessage();
        }

        /// <summary>
        /// Вывод свойств класса
        /// </summary>
        /// <typeparam name="T">Тип класса</typeparam>
        /// <param name="items">Перечень свойств класса</param>
        public void Output<T>(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                var properties = typeof(T).GetProperties();
                var values = properties.Select(p => $"{p.Name}: '{p.GetValue(item)}'");

                Output(string.Join(", ", values));
            }
        }

        /// <summary>
        /// Проверка объекта по идентификатору
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="message">Служебное сообщение</param>
        /// <returns>Значение идентификатора</returns>
        public int CheckId<T>(string message)
        {
            var id = Input<int>(message);
            _crud.Get<T>(id, out Result result);

            while (!result.Success)
            {
                Output($"Объект с идентификатором {id} не найден.");
                id = Input<int>(message);
                _crud.Get<T>(id, out result);
            }

            return id;
        }

        /// <summary>
        /// Получение значения
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="message">Сообщение для ввода данных</param>
        /// <returns>Значение</returns>
        private T? Input<T>(string message)
        {
            T? value;

            Output(message);

            while (!Tools.StringTryParse(Input(), out value))
            {
                Output("Введите корректное значение");
            }

            return value;
        }

        /// <summary>
        /// Вывод сообщения об ошибке
        /// <param name="errorText">Текст ошибки</param>
        /// </summary>
        private void WriteError(string? errorText)
        {
            if (errorText == null)
                Output($"Операция не выполнена. Нажмите Enter");
            else
                Output($"{errorText}. Нажмите Enter");

            Input();
        }
    }
}
