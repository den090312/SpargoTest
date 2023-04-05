using SpargoTest.Services;

namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Интерфейс операций с объектами
    /// </summary>
    public interface ICrud
    {
        /// <summary>
        /// Получение объекта
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <typeparam name="Id">Идентификатор получаемого объекта</typeparam>
        /// <param name="crudResult">Возможные ошибки при получении объекта</param>
        /// <returns>Получаемый объект</returns>
        T? Get<T>(int Id, out Result crudResult);

        /// <summary>
        /// Получение перечня объектов
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="crudResult">Возможные ошибки при получении объектов</param>
        /// <returns>Перечень объектов</returns>
        IEnumerable<T> GetAll<T>(out Result crudResult);

        /// <summary>
        /// Создать объект
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="crudResult">Возможные ошибки при создании объекта</param>
        void Create<T>(T obj, out Result crudResult);

        /// <summary>
        /// Удалить объект
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="Id">Идентификатор объекта</param>
        /// <param name="crudResult">Возможные ошибки при удалении объекта</param>
        void Remove<T>(int Id, out Result crudResult);
    }
}
