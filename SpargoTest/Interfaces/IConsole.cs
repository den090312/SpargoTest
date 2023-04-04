using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Консольный интерфейс
    /// </summary>
    /// <typeparam name="T">Тип объекта в консоли</typeparam>
    public interface IConsole<T>
    {
        /// <summary>
        /// Получение объекта через консоль
        /// </summary>
        /// <returns>Получаемый через консоль объект</returns>
        T Get();
    }
}
