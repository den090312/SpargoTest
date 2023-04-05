using SpargoTest.Interfaces;
using SpargoTest.Models;

namespace SpargoTest.CustomConsole
{
    /// <summary>
    /// Консольный функционал для продуктов
    /// </summary>
    public class ProductConsole : IInput<Product>
    {
        /// <summary>
        /// Создание продукта через консоль
        /// </summary>
        /// <returns>Продукт</returns>
        public Product Input()
        {
            var prodcut = new Product();

            Console.WriteLine("Введите наименование:");
            prodcut.Name = Console.ReadLine();

            return prodcut;
        }
    }
}
