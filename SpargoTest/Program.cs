using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;

using SpargoTest.CustomConsole;
using SpargoTest.Interfaces;
using SpargoTest.Menu;
using SpargoTest.Services;

namespace SpargoTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            var exit = false;

            while (!exit)
            {
                WriteMainMenu();

                if (!int.TryParse(Console.ReadLine(), out int choice))
                    Console.WriteLine("Неверный ввод. Пожалуйста, введите число от 1 до 6.");
                else
                    ChoiceProcessing(ref exit, choice);
            }
        }

        /// <summary>
        /// Обработка выбора пункта меню
        /// </summary>
        /// <param name="exit">Индикатор выхода из меню</param>
        /// <param name="choice">Маркер выбора</param>
        private static void ChoiceProcessing(ref bool exit, int choice)
        {
            switch (choice)
            {
                case 1:
                    Choice(choice, ConsoleMenu.Products, "товар", new ProductConsole());
                    break;
                case 2:
                    Choice(choice, ConsoleMenu.Pharmacies, "аптеку", new PharmacyConsole());
                    break;
                case 3:
                    Choice(choice, ConsoleMenu.Warehouses, "склад", new WarehouseConsole());
                    break;
                case 4:
                    Choice(choice, ConsoleMenu.Consignments, "партию", new ConsignmentConsole());
                    break;
                case 5:
                    //ToDo: Вывод списка товаров и их количества в выбранной аптеке
                    break;
                case 6:
                    Tools.InitializeDatabase();
                    Console.WriteLine("База данных успешно создана. Нажмите любую клавишу для продолжения");
                    Console.ReadLine();
                    break;
                case 7:
                    exit = true;
                    break;
            }
        }

        /// <summary>
        /// Вывод главное меню
        /// </summary>
        private static void WriteMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Главное меню:");
            Console.WriteLine($"1. {ConsoleMenu.Products}");
            Console.WriteLine($"2. {ConsoleMenu.Pharmacies}");
            Console.WriteLine($"3. {ConsoleMenu.Warehouses}");
            Console.WriteLine($"4. {ConsoleMenu.Consignments}");
            Console.WriteLine("5. Вывести список товаров и их количество в выбранной аптеке");
            Console.WriteLine("6. Пересоздать базу данных");
            Console.WriteLine("7. Выход");
        }

        /// <summary>
        /// Обработка консольного выбора
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="choice">Опция выбора</param>
        /// <param name="subMenuTitle">Заголовок подменю</param>
        /// <param name="subMenuName">Имя меню</param>
        /// <param name="input">Интерфейс для ввода данных</param>
        private static void Choice<T>(int choice, string subMenuTitle, string subMenuName, IInput<T> input) where T : class
        {
            var subMenu = new ConsoleSubMenu { Title = subMenuTitle + ":", Items = Tools.Menu.GetSubMenu(subMenuName) };
            var items = Tools.Storage.GetAll<T>(out Result result);

            if (!result.Success)
                Console.WriteLine($"Ошибка при получении {subMenuName}");

            Tools.Menu.Go(subMenu, items, out choice, out bool proceed);

            if (proceed)
                Tools.Menu.Action(choice, Tools.Storage, input);
        }
    }
}