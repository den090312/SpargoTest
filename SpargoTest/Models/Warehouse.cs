namespace SpargoTest.Models
{
    /// <summary>
    /// Склад
    /// </summary>
    public class Warehouse
    {
        /// <summary>
        /// Идентификатор склада
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Идентификатор аптеки
        /// </summary>
        public int? PharmacyId { get; set; }
        
        /// <summary>
        /// Наименование склада
        /// </summary>
        public string? Name { get; set; }
    }
}
