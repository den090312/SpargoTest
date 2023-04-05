using System.ComponentModel.Design;

using SpargoTest.CustomConsole;
using SpargoTest.Interfaces;
using SpargoTest.Models;

namespace SpargoTest
{
    /// <summary>
    /// Главное меню
    /// </summary>
    public class ConsoleMenu : IMainMenu
    {
        private static readonly string _listTitle = "Список";
        
        public static string CreateTitle { get; } = "Создать";
        
        public static string DeleteTitle { get; } = "Удалить";
        
        public static string Products { get; } = $"{_listTitle} товарных наименований";
        
        public static string Pharmacies { get; } = $"{_listTitle} аптек";
        
        public static string Warehouses { get; } = $"{_listTitle} складов";
        
        public static string Consignments { get; } = $"{_listTitle} партий";

        /// <summary>
        /// Запуск пункта меню
        /// </summary>
        /// <param name="subMenu">Пункт подменю</param>
        /// <param name="crud">Набор операций с объектами</param>
        /// <param name="io">Интерфейс ввода-вывода</param>
        /// <param name="choice">Выбор опции для действия</param>
        public void Go<T>(ISubMenu subMenu, ICrud crud, IInputOutput<T> io, out int choice, out bool proceed)
        {
            choice = 0;
            proceed = false;

            var exit = false;

            while (!exit)
            {
                Console.WriteLine(subMenu.Title);

                io.Output(crud);

                var i = 1;

                foreach (var item in subMenu.Items)
                {
                    Console.WriteLine($"{i}. {item}");
                    i++;
                }

                var menuItemsCount = subMenu.Items.Count();

                Console.WriteLine($"{menuItemsCount + 1}. Назад");

                if (!int.TryParse(Console.ReadLine(), out choice))
                    WriteError(menuItemsCount);
                else
                {
                    if (choice < 1 || choice > menuItemsCount)
                    {
                        if (choice != menuItemsCount + 1)
                            WriteError(menuItemsCount);
                        else
                            exit = true;
                    }
                    else
                    {
                        proceed = true;

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Действие в меню
        /// </summary>
        /// <typeparam name="T">Тип объекта действия</typeparam>
        /// <param name="choice">Выбор опции для действия с объектом</param>
        /// <param name="crud">Набор операций с объектом</param>
        /// <param name="io">Интерфейс ввода-вывода</param>
        public void Action<T>(int choice, ICrud crud, IInputOutput<T> io)
        {
            if (choice == 2)
                Delete<T>(crud);

            if (choice != 1)
                return;

            crud.Create<T>(io.Input(), out Result crudResult);

            SuccessMessage(crudResult);
        }

        /// <summary>
        /// Получение перечня подпунктов меню
        /// </summary>
        /// <param name="menuItem">Пункт меню</param>
        /// <returns>Перечень пунктов меню</returns>
        public IEnumerable<string> GetSubMenu(string menuItem)
            => new List<string> { $"{ConsoleMenu.CreateTitle} {menuItem}", $"{ConsoleMenu.DeleteTitle} {menuItem}" };

        /// <summary>
        /// Вывод сообщения об успешном выполнении операции
        /// </summary>
        /// <param name="crudResult">Результат операции</param>
        private static void SuccessMessage(Result crudResult)
        {
            if (crudResult.Success)
                Console.WriteLine("Операция выполнена успешно");
        }

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <typeparam name="T">Тип удаляемого объекта</typeparam>
        /// <param name="crud">Набор операций с объектами</param>
        private static void Delete<T>(ICrud crud)
        {
            Console.WriteLine("Введите идентификатор для удаления:");

            var exit = false;

            while (!exit)
            {
                if (!int.TryParse(Console.ReadLine(), out int Id))
                    Console.WriteLine("Введите корректное число!");
                else
                {
                    exit = true;

                    crud.Remove<T>(Id, out Result crudResult);

                    SuccessMessage(crudResult);
                }
            }
        }

        /// <summary>
        /// Вывод ошибки меню на консоль
        /// </summary>
        /// <param name="count">Количество пунктов меню</param>
        private static void WriteError(int count) 
            => Console.WriteLine($"Неверный ввод. Пожалуйста, введите число от 1 до {count + 1}.");
    }
}
