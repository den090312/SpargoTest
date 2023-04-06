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
    /// Консольный функционал для партий товара
    /// </summary>
    public class ConsignmentConsole : IOutput<Consignment>
    {
        /// <summary>
        /// Создание партии через консоль
        /// </summary>
        /// <returns>Партия товара</returns>
        public Consignment Output()
        {
            var consignment = new Consignment();

            consignment.ProductId = Tools.CheckId<Product>("Введите идентификатор товара:");
            consignment.WarehouseId = Tools.CheckId<Warehouse>("Введите идентификатор склада:");

            return consignment;
        }
    }
}
