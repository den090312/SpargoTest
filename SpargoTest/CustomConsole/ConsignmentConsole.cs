using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpargoTest.Interfaces;
using SpargoTest.Models;

namespace SpargoTest.CustomConsole
{
    /// <summary>
    /// Консольный функционал для партий товара
    /// </summary>
    public class ConsignmentConsole : IConsole<Consignment>
    {
        /// <summary>
        /// Создание партии через консоль
        /// </summary>
        /// <returns>Партия товара</returns>
        public Consignment Get()
        {
            var consignment = new Consignment();

            Console.WriteLine("Введите идентификатор партии:");

            var id = 0;

            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Неверный формат идентификатора партии. Пожалуйста, введите целое число.");
            }

            consignment.Id = id;

            var productId = 0;

            Console.WriteLine("Введите идентификатор продукта:");

            while (!int.TryParse(Console.ReadLine(), out productId))
            {
                Console.WriteLine("Неверный формат идентификатора продукта. Пожалуйста, введите целое число.");
            }

            consignment.ProductId = productId;

            var warehouseId = 0;

            Console.WriteLine("Введите идентификатор склада:");

            while (!int.TryParse(Console.ReadLine(), out warehouseId))
            {
                Console.WriteLine("Неверный формат идентификатора склада. Пожалуйста, введите целое число.");
            }

            consignment.WarehouseId = warehouseId;

            return consignment;
        }
    }
}
