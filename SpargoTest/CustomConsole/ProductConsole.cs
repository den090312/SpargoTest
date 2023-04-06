using SpargoTest.Interfaces;
using SpargoTest.Models;

namespace SpargoTest.CustomConsole
{
    /// <summary>
    /// Консольный функционал для товаров
    /// </summary>
    public class ProductConsole : IOutput<Product>
    {
        /// <summary>
        /// Создание товар через консоль
        /// </summary>
        /// <returns>Товар</returns>
        public Product Output()
        {
            var prodcut = new Product();

            Console.WriteLine("Введите наименование:");
            prodcut.Name = Console.ReadLine();

            return prodcut;
        }
    }
}
