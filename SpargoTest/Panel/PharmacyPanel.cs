using SpargoTest.Interfaces;
using SpargoTest.Models;
using SpargoTest.Services;

namespace SpargoTest.Panel
{
    /// <summary>
    /// Панель создания аптеки
    /// </summary>
    public class PharmacyPanel : IPanel<Pharmacy>
    {
        /// <summary>
        /// Получение аптеки
        /// </summary>
        /// <returns>Аптека</returns>
        public Pharmacy Get()
        {
            var pharmacy = new Pharmacy();

            Tools.Terminal.Output("Введите наименование:");
            pharmacy.Name = Tools.Terminal.Input();

            Tools.Terminal.Output("Введите адрес:");
            pharmacy.Address = Tools.Terminal.Input();

            Tools.Terminal.Output("Введите телефон:");
            pharmacy.PhoneNumber = Tools.Terminal.Input();

            return pharmacy;
        }
    }
}
