using SpargoTest.Interfaces;

namespace SpargoTest.Services
{
    /// <summary>
    /// Менеджер приложения
    /// </summary>
    public class ApplicationManager
    {
        /// <summary>
        /// Провайдер для работы с базой данных
        /// </summary>
        public IDatabaseProvider Provider { get; }

        /// <summary>
        /// Интерфейс операций с объектами
        /// </summary>
        public ICrud Crud { get; }
        
        /// <summary>
        /// Интерфейс главного меню
        /// </summary>
        public IMainMenu Menu { get; }

        /// <summary>
        /// Конструктор Менеджера приложения
        /// </summary>
        /// <param name="provider">Провайдер для работы с базой данных</param>
        /// <param name="crud">Интерфейс операций с объектами</param>
        /// <param name="menu">Интерфейс главного меню</param>
        public ApplicationManager(IDatabaseProvider provider, ICrud crud, IMainMenu menu)
        {
            Crud = crud;
            Provider = provider;
            Menu = menu;
        }
    }
}
