using System;
using System.Diagnostics;

using Microsoft.Data.Sqlite;

using SpargoTest.Models;

namespace SpargoTest
{
    public class Program
    {
        public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

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
                Console.WriteLine($"1. {Menu.ProductListTitle}");
                Console.WriteLine($"2. {Menu.PharmacyListTitle}");
                Console.WriteLine($"3. {Menu.WarehouseListTitle}");
                Console.WriteLine($"4. {Menu.ConsignmentListTitle}");
                Console.WriteLine("5. Вывести список товаров и их количество в выбранной аптеке");
                Console.WriteLine("6. Выход");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                    Console.WriteLine("Неверный ввод. Пожалуйста, введите число от 1 до 6.");
                else
                {
                    var menu = new Menu();
                    var storage = new Storage(new CustomSQLiteProvider());

                    switch (choice)
                    {
                        case 1:
                            menu.Go(Menu.ProductListTitle + ":", 
                                new List<string> { $"{Menu.CreateTitle} товар", $"{Menu.DeleteTitle} товар" }, ref choice, out bool proceed);
                            if (proceed)
                                menu.Action<Product>(choice, storage);
                            break;
                        case 2:
                            menu.Go(Menu.PharmacyListTitle + ":", 
                                new List<string> { $"{Menu.CreateTitle} аптеку", $"{Menu.DeleteTitle} аптеку" }, ref choice, out proceed);
                            break;
                        case 3:
                            menu.Go(Menu.WarehouseListTitle + ":", 
                                new List<string> { $"{Menu.CreateTitle} склад", $"{Menu.DeleteTitle} склад" }, ref choice, out proceed);
                            break;
                        case 4:
                            menu.Go(Menu.ConsignmentListTitle + ":", 
                                new List<string> { $"{Menu.CreateTitle} партию", $"{Menu.DeleteTitle} партию" }, ref choice, out proceed);
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
    }
}