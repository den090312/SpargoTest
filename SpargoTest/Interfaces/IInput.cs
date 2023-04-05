using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpargoTest.Models;

namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Интерфейс для ввода данных
    /// </summary>
    /// <typeparam name="T">Тип объекта данных</typeparam>
    public interface IInput<T>
    {
        /// <summary>
        /// Ввод объекта
        /// </summary>
        /// <returns>Получаемый объект</returns>
        T Input();
    }
}
