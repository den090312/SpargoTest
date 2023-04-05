namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Подменю
    /// </summary>
    public interface ISubMenu
    {
        /// <summary>
        /// Заголовок пункта подменю
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Список подпунктов подменю
        /// </summary>
        public IEnumerable<string?> Items { get; set; }
    }
}
