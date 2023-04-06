using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpargoTest.Interfaces;
using SpargoTest.Models;
using SpargoTest.Services;

namespace SpargoTest.Panel
{
    /// <summary>
    /// Панель создания склада
    /// </summary>
    public class WarehousePanel : IPanel<Warehouse>
    {
        /// <summary>
        /// Получение склада
        /// </summary>
        /// <returns>Склад</returns>
        public Warehouse Get()
        {
            var warehouse = new Warehouse();

            warehouse.PharmacyId = Tools.CheckId<Pharmacy>("Введите идентификатор аптеки:");

            Tools.Terminal.Output("Введите наименование склада:");
            warehouse.Name = Tools.Terminal.Input();

            return warehouse;
        }
    }
}
