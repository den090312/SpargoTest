using SpargoTest.Interfaces;
using SpargoTest.Models;

namespace SpargoTest.CustomConsole
{
    /// <summary>
    /// Консольный функционал для продуктов
    /// </summary>
    public class ProductConsole : IInputOutput<Product>
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

        /// <summary>
        /// Вывод продуктов на консоль
        /// </summary>
        /// <param name="crud">Интерфейс операций с продуктами</param>
        public void Output(IEnumerable<Product> products)
        {
            foreach (var product in products)
                Console.WriteLine($"'Id': {product.Id}, 'Name': {product.Name}");
        }
    }
}
