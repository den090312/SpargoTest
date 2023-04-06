namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Терминал ввода-вывода
    /// </summary>
    public interface ITerminal
    {
        /// <summary>
        /// Ввод данных
        /// </summary>
        /// <returns>Введенное значение</returns>
        string? Input();

        /// <summary>
        /// Вывод сообщения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        void Output(string message);
    }
}