using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpargoTest.Models;

namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Интерфейс ввода-вывода
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    public interface IInputOutput<T>
    {
        /// <summary>
        /// Ввод объекта
        /// </summary>
        /// <returns>Получаемый объект</returns>
        T Input();

        /// <summary>
        /// Вывод объектов
        /// </summary>
        void Output(IEnumerable<T> objects);
    }
}
