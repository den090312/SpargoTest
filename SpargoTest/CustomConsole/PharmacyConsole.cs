using SpargoTest.Interfaces;
using SpargoTest.Models;

namespace SpargoTest.CustomConsole
{
    /// <summary>
    /// Консольный функционал для аптек
    /// </summary>
    public class PharmacyConsole : IConsole<Pharmacy>
    {
        /// <summary>
        /// Создание аптеки через консоль
        /// </summary>
        /// <returns>Аптека</returns>
        public Pharmacy Get()
        {
            var pharmacy = new Pharmacy();

            Console.WriteLine("Введите наименование:");
            pharmacy.Name = Console.ReadLine();

            Console.WriteLine("Введите адрес:");
            pharmacy.Address = Console.ReadLine();

            Console.WriteLine("Введите телефон:");
            pharmacy.PhoneNumber = Console.ReadLine();

            return pharmacy;
        }
    }
}
