using SpargoTest.Interfaces;
using SpargoTest.Menu;
using SpargoTest.Repository;

namespace SpargoTest.Services
{
    /// <summary>
    /// Различные инструменты
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Хранилище
        /// </summary>
        public static ICrud Storage => new Storage(new SqlServerProvider(createTables: true));

        /// <summary>
        /// Консольное меню
        /// </summary>
        public static IMainMenu Menu => new ConsoleMenu();

        /// <summary>
        /// Имя папка моделей
        /// </summary>
        public static string ModelsFolderName => "Models";

        /// <summary>
        /// Получение корневого каталога для текущего запуска приложения
        /// </summary>
        /// <returns>Корневой каталог приложения</returns>
        public static string BaseDirectory()
        {
            var appRoot = AppDomain.CurrentDomain.BaseDirectory;

#if DEBUG
            appRoot = Path.GetFullPath(Path.Combine(appRoot, @"..\..\..\"));
#endif

            return appRoot;
        }

        /// <summary>
        /// Консольный вывод свойств класса
        /// </summary>
        /// <typeparam name="T">Тип класса</typeparam>
        /// <param name="items">Перечень объектов класса</param>
        public static void Output<T>(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                var properties = typeof(T).GetProperties();
                var values = properties.Select(p => $"{p.Name}: '{p.GetValue(item)}'");

                Console.WriteLine(string.Join(", ", values));
            }
        }
    }
}
