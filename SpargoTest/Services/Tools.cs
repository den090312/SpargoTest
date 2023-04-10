using System.ComponentModel;
using System.Data.SqlClient;

using SpargoTest.Panel;
using SpargoTest.Interfaces;
using SpargoTest.Menu;
using SpargoTest.Repository;
using SpargoTest.DTO;

namespace SpargoTest.Services
{
    /// <summary>
    /// Различные инструменты
    /// </summary>
    public static class Tools
    {
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
        /// Конвертер строки
        /// </summary>
        /// <typeparam name="T">Тип результата конвертации</typeparam>
        /// <param name="input">Конвертируемая строка</param>
        /// <param name="value">Сконвертированное значение</param>
        /// <returns>Результат конвертера</returns>
        public static bool StringTryParse<T>(string? input, out T? value)
        {
            value = default;

            if (input == null)
                return false;

            var converter = TypeDescriptor.GetConverter(typeof(T));

            if (converter == null || !converter.IsValid(input))
                return false;

            value = (T?)converter.ConvertFromString(input);

            return true;
        }
    }
}
