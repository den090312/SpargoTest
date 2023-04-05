using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpargoTest.Interfaces;
using SpargoTest.Services;

namespace SpargoTest.Repository
{
    /// <summary>
    /// Хранилище объектов
    /// </summary>
    public class Storage : ICrud
    {
        /// <summary>
        /// Провайдер базы данных для работы с хранилищем
        /// </summary>
        private readonly IDatabaseProvider _databaseProvider;

        /// <summary>
        /// Конструктор хранилища объектов
        /// </summary>
        /// <param name="sqlProvider">Провайдер базы данных</param>
        /// <exception cref="ArgumentNullException">Исключение при передаче в конструктор значения null</exception>
        public Storage(IDatabaseProvider sqlProvider)
            => _databaseProvider = sqlProvider ?? throw new ArgumentNullException(nameof(sqlProvider));

        /// <summary>
        /// Создание объекта в хранилище
        /// </summary>
        /// <typeparam name="T">Тип создаваемого объекта</typeparam>
        /// <param name="obj">Создаваемый объект</param>
        /// <param name="crudResult">Возможные ошибки при создании объекта</param>
        public void Create<T>(T obj, out Result crudResult)
            => _databaseProvider.Add(obj, out crudResult);

        /// <summary>
        /// Удаление объекта из хранилища
        /// </summary>
        /// <typeparam name="T">Тип удаляемого объекта</typeparam>
        /// <param name="Id">Идентификатор удаляемого объекта</param>
        /// <param name="crudResult">Возможные ошибки при удалении объектов</param>
        public void Remove<T>(int Id, out Result crudResult)
            => _databaseProvider.Remove<T>(Id, out crudResult);

        /// <summary>
        /// Чтение объекта из хранилища
        /// </summary>
        /// <typeparam name="T">Тип читаемого объекта</typeparam>
        /// <param name="Id">Идентификатор читаемого объекта</param>
        /// <param name="crudResult">Возможные ошибки при чтении объекта</param>
        /// <returns>Читаемый объект</returns>
        public T? Get<T>(int Id, out Result crudResult)
            => _databaseProvider.Get<T>(Id, out crudResult);

        /// <summary>
        /// Чтение объектов из хранилища
        /// </summary>
        /// <typeparam name="T">Тип читаемыех объектов</typeparam>
        /// <param name="obj">Читаемый объект</param>
        /// <param name="crudResult">Возможные ошибки при чтении объектов</param>
        /// <returns>Перечень читаемых объектов</returns>
        public IEnumerable<T> GetAll<T>(out Result crudResult)
            => _databaseProvider.GetAll<T>(out crudResult);
    }
}