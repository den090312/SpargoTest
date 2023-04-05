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
        public void Output(ICrud crud)
        {
            var products = crud.GetMany<Product>(out Result readResult);

            if (!readResult.Success)
                Console.WriteLine($"Произошла ошибка при получении товаров: {readResult.ErrorMessage}");

            foreach (var product in products)
                Console.WriteLine($"Id - {product.Id}, Name - {product.Name}");
        }
    }
}
