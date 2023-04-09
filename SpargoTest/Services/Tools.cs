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
        /// Провайдер базы данных
        /// </summary>
        private static IDatabaseProvider _databaseProvider = new SqlExpressProvider();

        /// <summary>
        /// Терминал
        /// </summary>
        public static ITerminal Terminal => new ConsoleTerminal();

        /// <summary>
        /// Инициализация базы данных
        /// </summary>
        public static void InitializeDatabase()
        {
            _databaseProvider.Initialize();
            WriteSuccessMessage();
        }

        /// <summary>
        /// Хранилище
        /// </summary>
        public static ICrud Storage => new Storage(_databaseProvider);

        /// <summary>
        /// Меню
        /// </summary>
        public static IMainMenu Menu => new ConsoleMenu();

        /// <summary>
        /// Имя папка моделей
        /// </summary>
        public static string ModelsFolderName => "Models";

        /// <summary>
        /// Получить список продуктов по идентификатору аптеки
        /// </summary>
        /// <param name="pharmacyId">Идентификатор аптеки</param>
        /// <param name="result">Результат получения</param>
        /// <returns>Список продуктов</returns>
        public static IEnumerable<ProductDto> GetProductsByPharmacy(int pharmacyId) 
            => new SqlExpressProvider().GetProductsByPharmacy(pharmacyId);

        /// <summary>
        /// Вывод сообщения об успехе
        /// </summary>
        public static void WriteSuccessMessage()
        {
            Terminal.Output("Операция выполнена успешно. Нажмите любую клавишу.");
            Terminal.Input();
        }

        /// <summary>
        /// Вывод сообщения об итогах операции
        /// </summary>
        /// <param name="result">Результат операции</param>
        public static void WriteResultMessage(Result result)
        {
            if (!result.Success)
                WriteErrorMessage(result.ErrorMessage);

            WriteSuccessMessage();
        }

        /// <summary>
        /// Обработка результата операции
        /// </summary>
        /// <param name="result">Результат операции</param>
        /// <returns>Результат обработки</returns>
        public static bool HandleResult(Result result)
        {
            if (!result.Success)
            {
                WriteErrorMessage(result.ErrorMessage);
                
                return false;
            }

            return true;
        }

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
        /// Вывод свойств класса
        /// </summary>
        /// <typeparam name="T">Тип класса</typeparam>
        /// <param name="items">Перечень объектов класса</param>
        public static void Output<T>(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                var properties = typeof(T).GetProperties();
                var values = properties.Select(p => $"{p.Name}: '{p.GetValue(item)}'");

                Terminal.Output(string.Join(", ", values));
            }
        }

        /// <summary>
        /// Проверка объекта по идентификатору
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="message">Служебное сообщение</param>
        /// <returns>Значение идентификатора</returns>
        public static int CheckId<T>(string message) where T : class
        {
            var id = Tools.Input<int>(message);
            Storage.Get<T>(id, out Result result);

            while (!result.Success)
            {
                Terminal.Output($"Объект с идентификатором {id} не найден.");
                id = Tools.Input<int>(message);
                Storage.Get<T>(id, out result);
            }

            return id;
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

        /// <summary>
        /// Вывод сообщения об ошибке
        /// <param name="errorMessage">Текст ошибки</param>
        /// </summary>
        private static void WriteErrorMessage(string? errorMessage)
        {
            if (errorMessage == null)
                Terminal.Output($"Операция не выполнена. Нажмите любую клавишу");
            else
                Terminal.Output($"{errorMessage}. Нажмите любую клавишу");

            Terminal.Input();
        }

        /// <summary>
        /// Получение значения
        /// </summary>
        /// <typeparam name="T">Тип значения</typeparam>
        /// <param name="message">Сообщение для ввода данных</param>
        /// <returns>Значение</returns>
        private static T? Input<T>(string message)
        {
            T? value;

            Terminal.Output(message);

            while (!StringTryParse(Terminal.Input(), out value))
            {
                Terminal.Output("Введите корректное значение");
            }

            return value;
        }
    }
}
