using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpargoTest.Interfaces;

namespace SpargoTest
{
    /// <summary>
    /// Хранилище объектов
    /// </summary>
    public class Storage : ICrud
    {
        /// <summary>
        /// Провайдер базы данных для работы с хранилищем
        /// </summary>
        private readonly IDatabaseProvider _sqlProvider;

        /// <summary>
        /// Конструктор хранилища объектов
        /// </summary>
        /// <param name="sqlProvider">Провайдер базы данных</param>
        /// <exception cref="ArgumentNullException">Исключение при передаче в конструктор значения null</exception>
        public Storage(IDatabaseProvider sqlProvider) 
            => _sqlProvider = sqlProvider ?? throw new ArgumentNullException(nameof(sqlProvider));

        /// <summary>
        /// Создание объекта в хранилище
        /// </summary>
        /// <typeparam name="T">Тип создаваемого объекта</typeparam>
        /// <param name="obj">Создаваемый объект</param>
        /// <param name="crudResult">Возможные ошибки при создании объекта</param>
        public void Create<T>(T obj, out CrudResult crudResult) 
            => _sqlProvider.Add(obj, out crudResult);

        /// <summary>
        /// Удаление объекта из хранилища
        /// </summary>
        /// <typeparam name="T">Тип удаляемого объекта</typeparam>
        /// <param name="Id">Идентификатор удаляемого объекта</param>
        /// <param name="crudResult">Возможные ошибки при удалении объектов</param>
        public void Delete<T>(int Id, out CrudResult crudResult) 
            => _sqlProvider.Remove<T>(Id, out crudResult);

        /// <summary>
        /// Чтение объектов из хранилища
        /// </summary>
        /// <typeparam name="T">Тип читаемыех объектов</typeparam>
        /// <param name="obj">Читаемый объект</param>
        /// <param name="crudResult">Возможные ошибки при чтении объектов</param>
        /// <returns></returns>
        public IEnumerable<T> Read<T>(out CrudResult crudResult) 
            => _sqlProvider.GetAll<T>(out crudResult);
    }
}