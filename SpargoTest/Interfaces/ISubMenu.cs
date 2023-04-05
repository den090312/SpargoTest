namespace SpargoTest.Interfaces
{
    public interface ISubMenu
    {
        /// <summary>
        /// Заголовок пункта меню
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Список подпунктов меню
        /// </summary>
        public IEnumerable<string?> Items { get; set; }
    }
}
