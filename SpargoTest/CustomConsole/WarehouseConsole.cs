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
    /// Консольный функционал для складов
    /// </summary>
    public class WarehouseConsole : IConsole<Warehouse>
    {
        /// <summary>
        /// Создание склада через консоль
        /// </summary>
        /// <returns></returns>
        public Warehouse Get()
        {
            var warehouse = new Warehouse();

            Console.WriteLine("Введите идентификатор склада:");

            var id = 0;

            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Неверный формат идентификатора склада. Пожалуйста, введите целое число.");
            }
            
            warehouse.Id = id;

            var pharmacyId = 0;

            Console.WriteLine("Введите идентификатор аптеки:");

            while (!int.TryParse(Console.ReadLine(), out pharmacyId))
            {
                Console.WriteLine("Неверный формат идентификатора аптеки. Пожалуйста, введите целое число.");
            }
            
            warehouse.PharmacyId = pharmacyId;

            Console.WriteLine("Введите наименование склада:");
            warehouse.Name = Console.ReadLine();

            return warehouse;
        }
    }
}
