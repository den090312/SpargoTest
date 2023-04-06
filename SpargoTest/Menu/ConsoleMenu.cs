﻿using System.ComponentModel.Design;

using SpargoTest.CustomConsole;
using SpargoTest.Interfaces;
using SpargoTest.Models;
using SpargoTest.Services;

namespace SpargoTest.Menu
{
    /// <summary>
    /// Главное консольное меню
    /// </summary>
    public class ConsoleMenu : IMainMenu
    {
        /// <summary>
        /// Заголовок списка 
        /// </summary>
        private static readonly string _listTitle = "Список";

        /// <summary>
        /// Заголовок создания
        /// </summary>
        public static string CreateTitle { get; } = "Создать";

        /// <summary>
        /// Заголовок удаления
        /// </summary>
        public static string DeleteTitle { get; } = "Удалить";

        /// <summary>
        /// Заголовок списка продуктов
        /// </summary>
        public static string Products { get; } = $"{_listTitle} товарных наименований";

        /// <summary>
        /// Заголовок списка аптек
        /// </summary>
        public static string Pharmacies { get; } = $"{_listTitle} аптек";

        /// <summary>
        /// Заголовок списка складов
        /// </summary>
        public static string Warehouses { get; } = $"{_listTitle} складов";

        /// <summary>
        /// Заголовок списка партий
        /// </summary>
        public static string Consignments { get; } = $"{_listTitle} партий";

        /// <summary>
        /// Запуск пункта меню
        /// </summary>
        /// <param name="subMenu">Пункт подменю</param>
        /// <param name="objects">Перечень объектов для подменю</param>
        /// <param name="io">Интерфейс ввода-вывода</param>
        /// <param name="choice">Выбор опции для действия</param>
        public void Go<T>(ISubMenu subMenu, IEnumerable<T> objects, out int choice, out bool proceed)
        {
            Console.Clear();

            choice = 0;
            proceed = false;

            var exit = false;

            while (!exit)
            {
                WriteSubMenu(subMenu, objects, out int menuItemsCount);

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
        /// <param name="input">Интерфейс ввода</param>
        public void Action<T>(int choice, ICrud crud, IInput<T> input)
        {
            if (choice == 2)
                Delete<T>(crud);

            if (choice != 1)
                return;

            crud.Create<T>(input.Input(), out Result crudResult);

            SuccessMessage(crudResult);
        }

        /// <summary>
        /// Получение перечня подпунктов меню
        /// </summary>
        /// <param name="menuItem">Пункт меню</param>
        /// <returns>Перечень пунктов меню</returns>
        public IEnumerable<string> GetSubMenu(string menuItem)
            => new List<string> { $"{CreateTitle} {menuItem}", $"{DeleteTitle} {menuItem}" };

        /// <summary>
        /// Вывод подменю
        /// </summary>
        /// <typeparam name="T">Тип объекта подменю</typeparam>
        /// <param name="subMenu">Интерфейс работы подменю</param>
        /// <param name="items">Пункты подменю</param>
        /// <param name="menuItemsCount">Количество пунктов подменю</param>
        private void WriteSubMenu<T>(ISubMenu subMenu, IEnumerable<T> items, out int menuItemsCount)
        {
            Console.WriteLine(subMenu.Title);
            Tools.Output(items);

            var i = 1;

            foreach (var item in subMenu.Items)
            {
                Console.WriteLine($"{i}. {item}");
                i++;
            }

            menuItemsCount = subMenu.Items.Count();

            Console.WriteLine($"{menuItemsCount + 1}. Назад");
        }

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
        {
            Console.Clear();
            Console.WriteLine($"Неверный ввод. Пожалуйста, введите число от 1 до {count + 1}.");
        }
    }
}