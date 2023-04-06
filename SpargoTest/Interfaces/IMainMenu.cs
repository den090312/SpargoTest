namespace SpargoTest.Interfaces
{
    /// <summary>
    /// Интерфейс главного меню
    /// </summary>
    public interface IMainMenu
    {
        /// <summary>
        /// Запуск пункта меню
        /// </summary>
        /// <typeparam name="T">Тип объекта в меню</typeparam>
        /// <param name="subMenu">Пункт подменю</param>
        /// <param name="items">Перечень пунктов меню</param>
        /// <param name="choice">Выбор опции для действия</param>
        /// <param name="proceed">Вход в подпункт меню</param>
        void Go<T>(ISubMenu subMenu, IEnumerable<T> items, out int choice, out bool proceed);

        /// <summary>
        /// Действие в меню
        /// </summary>
        /// <typeparam name="T">Тип объекта действия</typeparam>
        /// <param name="choice">Выбор опции для действия с объектом</param>
        /// <param name="crud">Набор операций с объектом</param>
        /// <param name="panel">Панель данных</param>
        public void Action<T>(int choice, ICrud crud, IPanel<T> panel);

        /// <summary>
        /// Получение перечня подпунктов меню
        /// </summary>
        /// <param name="menuItem">Подпункт меню</param>
        /// <returns>Перечень подпунктов меню</returns>
        public IEnumerable<string> GetSubMenu(string menuItem);
    }
}
