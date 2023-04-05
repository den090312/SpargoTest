using SpargoTest.Interfaces;
using SpargoTest.Models;

namespace SpargoTest.CustomConsole
{
    /// <summary>
    /// Консольный функционал для аптек
    /// </summary>
    public class PharmacyConsole : IInputOutput<Pharmacy>
    {
        /// <summary>
        /// Создание аптеки через консоль
        /// </summary>
        /// <returns>Аптека</returns>
        public Pharmacy Input()
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

        /// <summary>
        /// Вывод перечня аптек через консоль
        /// </summary>
        /// <param name="crud">Интерфейс операций с аптеками</param>
        public void Output(ICrud crud)
        {
            var pharmacies = crud.GetMany<Pharmacy>(out Result readResult);

            if (!readResult.Success)
                Console.WriteLine($"Произошла ошибка при получении товаров: {readResult.ErrorMessage}");

            foreach (var pharmacy in pharmacies)
                Console.WriteLine($"Id - {pharmacy.Id}" +
                    $", PharmacyId - {pharmacy.Name}" +
                    $", Name - {pharmacy.Address}" +
                    $", PhoneNumber - {pharmacy.PhoneNumber}");
        }
    }
}
