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
    public class WarehouseConsole : IInputOutput<Warehouse>
    {
        /// <summary>
        /// Создание склада через консоль
        /// </summary>
        /// <returns></returns>
        public Warehouse Input()
        {
            var warehouse = new Warehouse();

            var pharmacyId = 0;

            Console.WriteLine("Введите идентификатор аптеки:");

            while (!int.TryParse(Console.ReadLine(), out pharmacyId))
            {
                Console.WriteLine("Неверный формат идентификатора аптеки. Пожалуйста, введите целое число.");
            }

            //ToDo: проверить существование аптеки по этому идентификатору
            
            warehouse.PharmacyId = pharmacyId;

            Console.WriteLine("Введите наименование склада:");
            warehouse.Name = Console.ReadLine();

            return warehouse;
        }

        /// <summary>
        /// Вывод перечня складов на консоль
        /// </summary>
        /// <param name="crud">Интерфейс операций со складами</param>
        public void Output(IEnumerable<Warehouse> warehouses)
        {
            foreach (var warehouse in warehouses)
                Console.WriteLine($"'Id': {warehouse.Id}, 'PharmacyId': {warehouse.PharmacyId}, 'Name': {warehouse.Name}");
        }
    }
}
