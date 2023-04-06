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

                if (!int.TryParse(Tools.Terminal.Input(), out int choice))
                    Tools.Terminal.Output("Неверный ввод. Пожалуйста, введите число от 1 до 6.");
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
                    Choice(choice, ConsoleMenu.Products, "товар", new ProductPanel());
                    SuccessOutput();                    
                    break;
                case 2:
                    Choice(choice, ConsoleMenu.Pharmacies, "аптеку", new PharmacyPanel());
                    SuccessOutput();
                    break;
                case 3:
                    Choice(choice, ConsoleMenu.Warehouses, "склад", new WarehousePanel());
                    SuccessOutput();
                    break;
                case 4:
                    Choice(choice, ConsoleMenu.Consignments, "партию", new ConsignmentPanel());
                    SuccessOutput();
                    break;
                case 5:
                    //ToDo: Вывод списка товаров и их количества в выбранной аптеке
                    break;
                case 6:
                    Tools.InitializeDatabase();
                    SuccessOutput();
                    break;
                case 7:
                    exit = true;
                    break;
            }
        }

        /// <summary>
        /// Главное меню
        /// </summary>
        private static void WriteMainMenu()
        {
            Console.Clear();
            Tools.Terminal.Output("Главное меню:");
            Tools.Terminal.Output($"1. {ConsoleMenu.Products}");
            Tools.Terminal.Output($"2. {ConsoleMenu.Pharmacies}");
            Tools.Terminal.Output($"3. {ConsoleMenu.Warehouses}");
            Tools.Terminal.Output($"4. {ConsoleMenu.Consignments}");
            Tools.Terminal.Output("5. Вывести список товаров и их количество в выбранной аптеке");
            Tools.Terminal.Output("6. Пересоздать базу данных");
            Tools.Terminal.Output("7. Выход");
        }

        /// <summary>
        /// Обработка выбора
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="choice">Опция выбора</param>
        /// <param name="subMenuTitle">Заголовок подменю</param>
        /// <param name="subMenuName">Имя меню</param>
        /// <param name="input">Интерфейс для ввода данных</param>
        private static void Choice<T>(int choice, string subMenuTitle, string subMenuName, IPanel<T> input) where T : class
        {
            var subMenu = new ConsoleSubMenu { Title = subMenuTitle + ":", Items = Tools.Menu.GetSubMenu(subMenuName) };
            var items = Tools.Storage.GetAll<T>(out Result result);

            if (!result.Success)
                Tools.Terminal.Output($"Ошибка при получении {subMenuName}");

            Tools.Menu.Go(subMenu, items, out choice, out bool proceed);

            if (proceed)
                Tools.Menu.Action(choice, Tools.Storage, input);
        }

        /// <summary>
        /// Консольный вывод об успехе операции
        /// </summary>
        private static void SuccessOutput()
        {
            Tools.Terminal.Output("Операция выполнена успешно. Нажмите любую клавишу.");
            Tools.Terminal.Input();
        }
    }
}