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
    public class ConsignmentConsole : IInputOutput<Consignment>
    {
        /// <summary>
        /// Создание партии через консоль
        /// </summary>
        /// <returns>Партия товара</returns>
        public Consignment Input()
        {
            var consignment = new Consignment();

            var productId = 0;

            Console.WriteLine("Введите идентификатор продукта:");

            while (!int.TryParse(Console.ReadLine(), out productId))
            {
                Console.WriteLine("Неверный формат идентификатора продукта. Пожалуйста, введите целое число.");
            }

            //ToDo: проверить существование продукта по этому идентификатору

            consignment.ProductId = productId;

            var warehouseId = 0;

            Console.WriteLine("Введите идентификатор склада:");

            while (!int.TryParse(Console.ReadLine(), out warehouseId))
            {
                Console.WriteLine("Неверный формат идентификатора склада. Пожалуйста, введите целое число.");
            }

            //ToDo: проверить существование склада по этому идентификатору

            consignment.WarehouseId = warehouseId;

            return consignment;
        }

        /// <summary>
        /// Вывод объектов на консоль
        /// </summary>
        public void Output(IEnumerable<Consignment> consignments)
        {
            foreach (var consignment in consignments)
                Console.WriteLine($"'Id' - {consignment.Id}" +
                    $", 'Name': {consignment.ProductId}" +
                    $", 'Warehouse': {consignment.WarehouseId}");
        }
    }
}
