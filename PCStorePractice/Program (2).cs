using System;

namespace ComputerWorkshop
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== МАСТЕРСКАЯ 'КОМПЬЮТЕРНЫЙ ГЕНИЙ' ===\n");
            
            WorkshopMenu menu = new WorkshopMenu();
            menu.ShowMainMenu();
            
            Console.WriteLine("\nСпасибо за обращение! Удачной сборки!");
            Console.ReadKey();
        }
    }
}