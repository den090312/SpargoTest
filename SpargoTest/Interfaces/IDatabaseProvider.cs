using SpargoTest.Services;

namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Провайдер для работы с базой данных
    /// </summary>
    public interface IDatabaseProvider
    {
        /// <summary>
        /// Инициализация базы данных
        /// </summary>
        void Initialize();

        /// <summary>
        /// Тест соединения с сервером
        /// </summary>
        /// <returns>Индикатор успешного соединения</returns>
        bool ServerOnline();

        /// <summary>
        /// Добавить объект в базу данных
        /// </summary>
        /// <typeparam name="T">Тип добавляемого объекта</typeparam>
        /// <param name="obj">Добавляемый объект</param>
        /// <param name="result">Результат при добавлении объекта</param>
        void Add<T>(T obj, out Result result);

        /// <summary>
        /// Получение объекта из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="Id">Идентификатор объекта</param>
        /// <param name="result">Результат при получении объекта</param>
        /// <returns>Получаемый объект</returns>
        T? Get<T>(int Id, out Result result);

        /// <summary>
        /// Получение всех объектов типа Т из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="result">Результат при получении объектлв</param>
        /// <returns>Перечень объектов</returns>
        IEnumerable<T> GetAll<T>(out Result result);

        /// <summary>
        /// Удаление объекта типа T из базы данных
        /// </summary>
        /// <typeparam name="T">Тип удаляемого объекта</typeparam>
        /// <param name="Id">Идентификатор удаляемого объекта</param>
        /// <param name="result">Результат при удалении объекта</param>
        void Remove<T>(int Id, out Result result);
    }
}
