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

        public void Output(ICrud crud)
        {
            var warehouses = crud.GetMany<Warehouse>(out CrudResult readResult);

            if (!readResult.Success)
                Console.WriteLine($"Произошла ошибка при получении товаров: {readResult.ErrorMessage}");

            foreach (var warehouse in warehouses)
                Console.WriteLine($"Id - {warehouse.Id}, PharmacyId - {warehouse.PharmacyId}, Name - {warehouse.Name}");
        }
    }
}
