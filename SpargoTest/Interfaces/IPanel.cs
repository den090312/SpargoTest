using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
