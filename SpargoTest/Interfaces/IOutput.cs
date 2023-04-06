using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpargoTest.Models;

namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Интерфейс для вывода объекта
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    public interface IOutput<T>
    {
        /// <summary>
        /// Вывод объекта
        /// </summary>
        /// <returns>Получаемый объект</returns>
        T Output();
    }
}
