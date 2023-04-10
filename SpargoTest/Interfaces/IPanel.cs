using SpargoTest.Models;

namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Панель данных
    /// </summary>
    /// <typeparam name="T">Тип объекта панели</typeparam>
    public interface IPanel<T>
    {
        /// <summary>
        /// Получение объекта
        /// </summary>
        /// <returns>Получаемый объект</returns>
        T Get();

        /// <summary>
        /// Вывод сообщения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        void Output(string? message);
    }
}
