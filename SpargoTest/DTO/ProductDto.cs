namespace SpargoTest.DTO
{
    /// <summary>
    /// Товар
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование товара
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Количество товара на складах
        /// </summary>
        public int ProductCount { get; set; }
    }
}
