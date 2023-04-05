using System;
using System.Diagnostics;
using System.Reflection;

using SpargoTest.CustomConsole;
using SpargoTest.Interfaces;
using SpargoTest.Models;

namespace SpargoTest
{
    public class Program
    {
        public static string ModelsFolderName => "Models";

        public static string BaseDirectory()
        {
            var appRoot = AppDomain.CurrentDomain.BaseDirectory;
            
            #if DEBUG
                appRoot = Path.GetFullPath(Path.Combine(appRoot, @"..\..\..\"));
            #endif

            return appRoot;
        }

        static void Main(string[] args)
        {
            MainMenu();
        }

        public static void MainMenu()
        {
            var exit = false;

            while (!exit)
            {
                Console.WriteLine("Главное меню:");
                Console.WriteLine($"1. {ConsoleMenu.Products}");
                Console.WriteLine($"2. {ConsoleMenu.Pharmacies}");
                Console.WriteLine($"3. {ConsoleMenu.Warehouses}");
                Console.WriteLine($"4. {ConsoleMenu.Consignments}");
                Console.WriteLine("5. Вывести список товаров и их количество в выбранной аптеке");
                Console.WriteLine("6. Выход");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                    Console.WriteLine("Неверный ввод. Пожалуйста, введите число от 1 до 6.");
                else
                {
                    var menu = new ConsoleMenu();
                    var dbProvider = new SqlServerProvider(createTables: true);

                    if (!dbProvider.ConnectionIsOk())
                    {
                        Console.WriteLine("Не удалось соединиться с базой данных");

                        return;
                    }

                    var storage = new Storage(dbProvider);

                    switch (choice)
                    {
                        case 1:
                            var productConsole = new ProductConsole();
                            var productSubMenu = new ConsoleSubMenu { Title = ConsoleMenu.Products + ":", Items = menu.GetSubMenu("товар") };

                            menu.Go(productSubMenu, storage, productConsole, out choice, out bool proceed);

                            if (proceed)
                                menu.Action(choice, storage, productConsole);
                            break;
                        case 2:
                            var pharmacyConsole = new PharmacyConsole();
                            var pharmacySubMenu = new ConsoleSubMenu { Title = ConsoleMenu.Pharmacies + ":", Items = menu.GetSubMenu("аптеку") };

                            menu.Go(pharmacySubMenu, storage, pharmacyConsole, out choice, out proceed);

                            if (proceed)
                                menu.Action(choice, storage, pharmacyConsole);
                            break;
                        case 3:
                            var warehouseConsole = new WarehouseConsole();
                            var warehoseSubMenu = new ConsoleSubMenu { Title = ConsoleMenu.Warehouses + ":", Items = menu.GetSubMenu("склад") };

                            menu.Go(warehoseSubMenu, storage, warehouseConsole, out choice, out proceed);
                            
                            if (proceed)
                                menu.Action(choice, storage, warehouseConsole);
                            break;
                        case 4:
                            var consignmentConsole = new ConsignmentConsole();
                            var consignmentSubMenu = new ConsoleSubMenu { Title = ConsoleMenu.Consignments + ":", Items = menu.GetSubMenu("партию") };

                            menu.Go(consignmentSubMenu, storage, consignmentConsole, out choice, out proceed);

                            if (proceed)
                                menu.Action(choice, storage, consignmentConsole);
                            break;
                        case 5:
                            // Вывод списка товаров и их количества в выбранной аптеке
                            break;
                        case 6:
                            exit = true;
                            break;
                    }
                }
            }
        }

        private static List<string> GetMenuItemsList(string menuItem) 
            => new List<string> { $"{ConsoleMenu.CreateTitle} {menuItem}", $"{ConsoleMenu.DeleteTitle} {menuItem}" };
    }
}