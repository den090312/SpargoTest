using SpargoTest.Services;

namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Терминал ввода-вывода
    /// </summary>
    public interface ITerminal
    {
        /// <summary>
        /// Ввод данных определенного формата
        /// </summary>
        /// <typeparam name="T">Формат данных</typeparam>
        /// <returns>Значение определенного формата</returns>
        T? Input<T>();

        /// <summary>
        /// Ввод данных
        /// </summary>
        /// <returns>Введенное значение</returns>
        string? Input();

        /// <summary>
        /// Вывод сообщения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        void Output(string? message);

        /// <summary>
        /// Вывод сообщения об успехе
        /// </summary>
        void WriteSuccessMessage();

        /// <summary>
        /// Вывод сообщения об итогах операции
        /// </summary>
        /// <param name="result">Результат операции</param>
        void WriteResultMessage(Result result);

        /// <summary>
        /// Вывод свойств класса
        /// </summary>
        /// <typeparam name="T">Тип класса</typeparam>
        /// <param name="items">Перечень свойств класса</param>
        void Output<T>(IEnumerable<T> items);

        /// <summary>
        /// Проверка объекта по идентификатору
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="message">Служебное сообщение</param>
        /// <returns>Значение идентификатора</returns>
        int CheckId<T>(string message);
    }
}