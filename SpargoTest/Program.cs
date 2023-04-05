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
                            var productSubMenu = new ConsoleSubMenu { Title = ConsoleMenu.Products + ":", Items = menu.GetSubMenu("товар") };
                            var productConsole = new ProductConsole();
                            var products = storage.GetAll<Product>(out Result result);

                            if (!result.Success)
                                Console.WriteLine("Ошибка при получении продуктов");

                            menu.Go(productSubMenu, products, productConsole, out choice, out bool proceed);

                            if (proceed)
                                menu.Action(choice, storage, productConsole);
                            break;
                        case 2:
                            var pharmacySubMenu = new ConsoleSubMenu { Title = ConsoleMenu.Pharmacies + ":", Items = menu.GetSubMenu("аптеку") };
                            var pharmacyConsole = new PharmacyConsole();
                            var pharmacies = storage.GetAll<Pharmacy>(out result);

                            if (!result.Success)
                                Console.WriteLine("Ошибка при получении аптек");

                            menu.Go(pharmacySubMenu, pharmacies, pharmacyConsole, out choice, out proceed);

                            if (proceed)
                                menu.Action(choice, storage, pharmacyConsole);
                            break;
                        case 3:
                            var warehoseSubMenu = new ConsoleSubMenu { Title = ConsoleMenu.Warehouses + ":", Items = menu.GetSubMenu("склад") };
                            var warehouseConsole = new WarehouseConsole();
                            var warehouses = storage.GetAll<Warehouse>(out result);

                            if (!result.Success)
                                Console.WriteLine("Ошибка при получении складов");

                            menu.Go(warehoseSubMenu, warehouses, warehouseConsole, out choice, out proceed);
                            
                            if (proceed)
                                menu.Action(choice, storage, warehouseConsole);
                            break;
                        case 4:
                            var consignmentSubMenu = new ConsoleSubMenu { Title = ConsoleMenu.Consignments + ":", Items = menu.GetSubMenu("партию") };
                            var consignmentConsole = new ConsignmentConsole();
                            var consignments = storage.GetAll<Consignment>(out result);

                            if (!result.Success)
                                Console.WriteLine("Ошибка при получении складов");

                            menu.Go(consignmentSubMenu, consignments, consignmentConsole, out choice, out proceed);

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