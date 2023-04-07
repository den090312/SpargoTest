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
    }
}