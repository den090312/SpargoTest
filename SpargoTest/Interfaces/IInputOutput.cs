using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Интерфейс ввода-вывода
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    public interface IInputOutput<T>
    {
        /// <summary>
        /// Получение объекта по входным данным
        /// </summary>
        /// <returns>Получаемый объект</returns>
        T Input();

        /// <summary>
        /// Вывод объектов
        /// </summary>
        /// <param name="crud">Набор операций с объектами</param>
        void Output(ICrud crud);
    }
}
