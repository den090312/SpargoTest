namespace SpargoTest.Models
{
    /// <summary>
    /// Партия товара
    /// </summary>
    public class Consignment
    {
        /// <summary>
        /// Идентификатор партии
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public int? ProductId { get; set; }

        /// <summary>
        /// Идентификатор склада
        /// </summary>
        public int? WarehouseId { get; set; }
    }
}
