namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Провайдер для работы с базой данных
    /// </summary>
    public interface IDatabaseProvider
    {
        /// <summary>
        /// Тест соединения с базой данных
        /// </summary>
        /// <returns>Индикатор успешного соединения</returns>
        bool ConnectionIsOk();

        void Add<T>(T obj, out CrudResult crudResult);

        /// <summary>
        /// Получение всех объектов типа Т из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="crudResult">Возможные ошибки при получении объектлв</param>
        /// <returns>Перечень объектов</returns>
        IEnumerable<T> GetAll<T>(out CrudResult crudResult);
        
        /// <summary>
        /// Удаление объекта типа T из базы данных
        /// </summary>
        /// <typeparam name="T">Тип удаляемого объекта</typeparam>
        /// <param name="Id">Идентификатор удаляемого объекта</param>
        /// <param name="crudResult">Возможные ошибки при удалении объекта</param>
        void Remove<T>(int Id, out CrudResult crudResult);
    }
}
