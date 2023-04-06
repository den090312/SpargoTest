namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Подменю
    /// </summary>
    public interface ISubMenu
    {
        /// <summary>
        /// Заголовок подменю
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Список пунктов подменю
        /// </summary>
        public IEnumerable<string?> Items { get; set; }
    }
}
