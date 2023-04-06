using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpargoTest.Interfaces;
using SpargoTest.Models;
using SpargoTest.Services;

namespace SpargoTest.CustomConsole
{
    /// <summary>
    /// Консольный функционал для складов
    /// </summary>
    public class WarehouseConsole : IOutput<Warehouse>
    {
        /// <summary>
        /// Создание склада через консоль
        /// </summary>
        /// <returns>Склад</returns>
        public Warehouse Output()
        {
            var warehouse = new Warehouse();

            warehouse.PharmacyId = Tools.CheckId<Pharmacy>("Введите идентификатор аптеки:");

            Console.WriteLine("Введите наименование склада:");
            warehouse.Name = Console.ReadLine();

            return warehouse;
        }
    }
}
