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
    /// Панель создания партии товара
    /// </summary>
    public class ConsignmentPanel : IPanel<Consignment>
    {
        /// <summary>
        /// Получение партии
        /// </summary>
        /// <returns>Партия товара</returns>
        public Consignment Get()
        {
            var consignment = new Consignment();

            consignment.ProductId = Tools.CheckId<Product>("Введите идентификатор товара:");
            consignment.WarehouseId = Tools.CheckId<Warehouse>("Введите идентификатор склада:");

            return consignment;
        }
    }
}
