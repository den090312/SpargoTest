using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;

using SpargoTest.Panel;
using SpargoTest.Interfaces;
using SpargoTest.Menu;
using SpargoTest.Services;
using SpargoTest.Models;
using SpargoTest.Repository;

namespace SpargoTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            IContainer container = new TransientContainer();
            RegisterContainer(ref container);

            var terminal = container.Resolve<ITerminal>();

            var exit = false;

            while (!exit)
            {
                WriteMainMenu(terminal);

                if (!int.TryParse(terminal.Input(), out int choice))
                    terminal.Output("Неверный ввод. Пожалуйста, введите число от 1 до 7.");
                else
                    ChoiceProcessing(ref exit, choice, terminal, container);
            }
        }

        /// <summary>
        /// Регистрация зависимостей в контейнере
        /// </summary>
        /// <returns>Контейнер</returns>
        private static void  RegisterContainer(ref IContainer container)
        {
            container.Register<ICrud, Storage>();
            container.Register<IDatabaseProvider, SqlExpressProvider>();
            container.Register<IMainMenu, ConsoleMainMenu>();
            container.Register<ISubMenu, ConsoleSubMenu>();
            container.Register<ITerminal, ConsoleTerminal>();

            container.Register<IPanel<Product>, ProductPanel>();
            container.Register<IPanel<Pharmacy>, PharmacyPanel>();
            container.Register<IPanel<Warehouse>, WarehousePanel>();
            container.Register<IPanel<Consignment>, ConsignmentPanel>();
        }

        /// <summary>
        /// Обработка выбора пункта меню
        /// </summary>
        /// <param name="exit">Индикатор выхода из меню</param>
        /// <param name="choice">Маркер выбора</param>
        /// <param name="terminal">Терминал ввода-вывода</param>
        /// <param name="container">Контейнер зависимостей</param>
        private static void ChoiceProcessing(ref bool exit, int choice, ITerminal terminal, IContainer container)
        {
            var crud = container.Resolve<ICrud>();
            var menu = container.Resolve<IMainMenu>();
            var provider = container.Resolve<IDatabaseProvider>();

            switch (choice)
            {
                case 1:
                    Choice(choice, ConsoleMainMenu.Products, "товар", container.Resolve<IPanel<Product>>(), crud, menu);
                    break;
                case 2:
                    Choice(choice, ConsoleMainMenu.Pharmacies, "аптеку", container.Resolve<IPanel<Pharmacy>>(), crud, menu);
                    break;
                case 3:
                    Choice(choice, ConsoleMainMenu.Warehouses, "склад", container.Resolve<IPanel<Warehouse>>(), crud, menu);
                    break;
                case 4:
                    Choice(choice, ConsoleMainMenu.Consignments, "партию", container.Resolve<IPanel<Consignment>>(), crud, menu);
                    break;
                case 5:
                    GetProductByPharmacy(terminal);
                    break;
                case 6:
                    CreateDatabase(terminal, provider);
                    break;
                case 7:
                    exit = true;
                    break;
            }
        }

        /// <summary>
        /// Пересоздание базы данных
        /// </summary>
        /// <param name="terminal">Терминал ввода-вывода</param>
        /// <param name="provider">Провайдер базы данных</param>
        private static void CreateDatabase(ITerminal terminal, IDatabaseProvider provider)
        {
            provider.Initialize(out Result result);

            if (result.Success)
            {
                terminal.Output("База данных успешно создана. Нажмите Enter");
                terminal.Input();
            }
            else
            {
                terminal.Output($"Ошибка создания базы: '{result.ErrorMessage}'. Нажмите Enter");
                terminal.Input();
            }
        }

        /// <summary>
        /// Получение продукта по аптеке
        /// <param name="terminal">Терминал ввода-вывода</param>
        /// </summary>
        private static void GetProductByPharmacy(ITerminal terminal)
        {
            var pharmacyId = terminal.CheckId<Pharmacy>("Введите идентификатор аптеки:");
            var sqlDbProvider = new SqlExpressProvider();

            if (!sqlDbProvider.ServerOnline())
            {
                Console.WriteLine("Не могу подключиться");

                return;
            }

            var products = sqlDbProvider.GetProductsByPharmacy(pharmacyId);

            terminal.Output(products);
            terminal.WriteSuccessMessage();
        }

        /// <summary>
        /// Главное меню
        /// <param name="terminal">Терминал ввода-вывода</param>
        /// </summary>
        private static void WriteMainMenu(ITerminal terminal)
        {
            Console.Clear();
            terminal.Output("Главное меню:");
            terminal.Output($"1. {ConsoleMainMenu.Products}");
            terminal.Output($"2. {ConsoleMainMenu.Pharmacies}");
            terminal.Output($"3. {ConsoleMainMenu.Warehouses}");
            terminal.Output($"4. {ConsoleMainMenu.Consignments}");
            terminal.Output("5. Вывести список товаров и их количество в выбранной аптеке");
            terminal.Output("6. Пересоздать базу данных");
            terminal.Output("7. Выход");
        }

        /// <summary>
        /// Обработка выбора
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="choice">Опция выбора</param>
        /// <param name="subMenuTitle">Заголовок подменю</param>
        /// <param name="subMenuName">Имя меню</param>
        /// <param name="panel">Панель данных</param>
        /// <param name="crud">Интерфейс операций с объектами</param>
        /// <param name="crud">Интерфейс главного меню</param>
        private static void Choice<T>(int choice, string subMenuTitle, string subMenuName, IPanel<T> panel, ICrud crud, IMainMenu menu) where T : class
        {
            var subMenu = new ConsoleSubMenu { Title = subMenuTitle + ":", Items = menu.GetSubMenu(subMenuName) };
            var items = crud.GetAll<T>(out Result result);

            if (!result.Success)
                panel.Output($"Ошибка при получении {subMenuName}");

            menu.Go(subMenu, items, out choice, out bool proceed);

            if (proceed)
                menu.Action(choice, crud, panel);
        }
    }
}