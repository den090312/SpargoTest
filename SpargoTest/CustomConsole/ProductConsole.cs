using SpargoTest.Interfaces;
using SpargoTest.Models;

namespace SpargoTest.CustomConsole
{
    public class ProductConsole : IConsole<Product>
    {
        public Product Get()
        {
            var prodcut = new Product();

            Console.WriteLine("Введите наименование:");
            prodcut.Name = Console.ReadLine();

            return prodcut;
        }
    }
}
