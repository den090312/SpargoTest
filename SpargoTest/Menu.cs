using SpargoTest.Interfaces;
using SpargoTest.Models;

namespace SpargoTest
{
    public class Menu
    {
        private static readonly string _listTitle = "Список";
        
        public static string CreateTitle { get; } = "Создать";
        
        public static string DeleteTitle { get; } = "Удалить";
        
        public static string ProductListTitle { get; } = $"{_listTitle} товарных наименований";
        
        public static string PharmacyListTitle { get; } = $"{_listTitle} аптек";
        
        public static string WarehouseListTitle { get; } = $"{_listTitle} складов";
        
        public static string ConsignmentListTitle { get; } = $"{_listTitle} партий";

        public void Go(string menuTitle, IEnumerable<string> menuItems, ref int choice, out bool proceed)
        {
            var exit = false;
            proceed = false;

            while (!exit)
            {
                Console.WriteLine(menuTitle);

                var i = 1;

                foreach (var item in menuItems)
                {
                    Console.WriteLine($"{i}. {item}");
                    i++;
                }

                Console.WriteLine($"{menuItems.Count() + 1}. Назад");

                if (!int.TryParse(Console.ReadLine(), out choice))
                    WriteError(menuItems);
                else
                {
                    if (choice < 1 || choice > menuItems.Count())
                    {
                        if (choice != menuItems.Count() + 1)
                            WriteError(menuItems);
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

        public void Action<T>(int choice, ICrud crud)
        {
            if (choice == 1)
            {
                Console.WriteLine("Введите наименование:");

                crud.Create<Product>(new Product { Name = Console.ReadLine() }, out CrudResult crudResult);
            }

            if (choice == 2)
            {
                Console.WriteLine("Введите идентификатор для удаления:");

                var ProductIdExit = false;

                while (!ProductIdExit)
                {
                    if (!int.TryParse(Console.ReadLine(), out int Id))
                        Console.WriteLine("Введите корректное число!");
                    else
                    {
                        ProductIdExit = true;

                        crud.Delete<T>(Id, out CrudResult crudResult);
                    }
                }
            }
        }

        private static void WriteError(IEnumerable<string> menuItems) 
            => Console.WriteLine("Неверный ввод. Пожалуйста, введите число от 1 до {0}.", menuItems.Count() + 1);
    }
}
