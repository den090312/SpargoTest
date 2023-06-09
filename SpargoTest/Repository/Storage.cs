﻿using System;
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
        /// <param name="result">Результат при создании объекта</param>
        public void Create<T>(T obj, out Result result)
            => _databaseProvider.Add(obj, out result);

        /// <summary>
        /// Удаление объекта из хранилища
        /// </summary>
        /// <typeparam name="T">Тип удаляемого объекта</typeparam>
        /// <param name="Id">Идентификатор удаляемого объекта</param>
        /// <param name="result">Результат при удалении объектов</param>
        public void Remove<T>(int Id, out Result result)
            => _databaseProvider.Remove<T>(Id, out result);

        /// <summary>
        /// Чтение объекта из хранилища
        /// </summary>
        /// <typeparam name="T">Тип читаемого объекта</typeparam>
        /// <param name="Id">Идентификатор читаемого объекта</param>
        /// <param name="result">Результат при чтении объекта</param>
        /// <returns>Читаемый объект</returns>
        public T? Get<T>(int Id, out Result result)
            => _databaseProvider.Get<T>(Id, out result);

        /// <summary>
        /// Чтение объектов из хранилища
        /// </summary>
        /// <typeparam name="T">Тип читаемыех объектов</typeparam>
        /// <param name="obj">Читаемый объект</param>
        /// <param name="result">Результат при чтении объектов</param>
        /// <returns>Перечень читаемых объектов</returns>
        public IEnumerable<T> GetAll<T>(out Result result)
            => _databaseProvider.GetAll<T>(out result);
    }
}