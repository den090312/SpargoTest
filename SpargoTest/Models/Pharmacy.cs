namespace SpargoTest.Models
{
    /// <summary>
    /// Аптека
    /// </summary>
    public class Pharmacy
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Наименование
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// Адрес
        /// </summary>
        public string? Address { get; set; }
        
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string? PhoneNumber { get; set; }
    }
}
