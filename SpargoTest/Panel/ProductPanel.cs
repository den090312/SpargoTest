using SpargoTest.Interfaces;
using SpargoTest.Models;
using SpargoTest.Services;

namespace SpargoTest.Panel
{
    /// <summary>
    /// Панель создания товара
    /// </summary>
    public class ProductPanel : IPanel<Product>
    {
        /// <summary>
        /// Создание товара
        /// </summary>
        /// <returns>Товар</returns>
        public Product Get()
        {
            var prodcut = new Product();

            Tools.Terminal.Output("Введите наименование:");
            prodcut.Name = Tools.Terminal.Input();

            return prodcut;
        }
    }
}
