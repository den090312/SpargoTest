using SpargoTest.Interfaces;
using SpargoTest.Models;

namespace SpargoTest.CustomConsole
{
    /// <summary>
    /// Консольный функционал для продуктов
    /// </summary>
    public class ProductConsole : IConsole<Product>
    {
        /// <summary>
        /// Создание продукта через консоль
        /// </summary>
        /// <returns>Продукт</returns>
        public Product Get()
        {
            var prodcut = new Product();

            Console.WriteLine("Введите наименование:");
            prodcut.Name = Console.ReadLine();

            return prodcut;
        }
    }
}
