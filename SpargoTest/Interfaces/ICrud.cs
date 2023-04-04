namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Набор операций с объектами
    /// </summary>
    public interface ICrud
    {
        /// <summary>
        /// Получение перечня объектов
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="crudResult">Возможные ошибки при получении объектов</param>
        /// <returns>Перечень объектов</returns>
        IEnumerable<T> Read<T>(out CrudResult crudResult);

        /// <summary>
        /// Создать объект
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="crudResult">Возможные ошибки при создании объекта</param>
        void Create<T>(T obj, out CrudResult crudResult);

        /// <summary>
        /// Удалить объект
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="Id">Идентификатор объекта</param>
        /// <param name="crudResult">Возможные ошибки при удалении объекта</param>
        void Delete<T>(int Id, out CrudResult crudResult);
    }
}
