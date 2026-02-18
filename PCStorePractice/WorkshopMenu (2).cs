// TODO:
// 1. Реализовать конфигуратор компьютера
// 2. Реализовать оформление заказа на сборку
// 3. Реализовать управление складом и заказами

using System;
using System.Collections.Generic;

namespace ComputerWorkshop
{
    public class WorkshopMenu
    {
        private WorkshopManager manager;
        
        public WorkshopMenu()
        {
            manager = new WorkshopManager();
            InitializeWorkshopData();
        }
        
        private void InitializeWorkshopData()
        {
            // Инициализация тестовых данных - компоненты
            manager.AddComponent(new Component(1, "Core i5-13400F", "Intel", "BX8071513400F", 
                25000, 36, 10, "процессор", "Сокет: LGA1700, Ядер: 10, Потоков: 16"));
            manager.AddComponent(new Component(2, "B760 Gaming X", "Gigabyte", "B760GX", 
                15000, 36, 8, "материнская плата", "Сокет: LGA1700, Чипсет: B760"));
            manager.AddComponent(new Component(3, "RTX 4060 Ti", "NVIDIA", "RTX4060TI", 
                45000, 36, 5, "видеокарта", "Память: 8GB GDDR6, PCI-E 4.0"));
            manager.AddComponent(new Component(4, "DDR4 16GB", "Kingston", "KF432C16BB1/16", 
                5000, 60, 20, "ОЗУ", "Объем: 16GB, Тип: DDR4, Частота: 3200MHz"));
            manager.AddComponent(new Component(5, "SSD 1TB", "Samsung", "980 PRO", 
                8000, 60, 15, "SSD", "Объем: 1TB, Интерфейс: NVMe PCIe 4.0"));
            manager.AddComponent(new Component(6, "750W 80+ Gold", "Corsair", "RM750x", 
                12000, 120, 12, "блок питания", "Мощность: 750W, Сертификат: 80+ Gold"));
            manager.AddComponent(new Component(7, "Meshify C", "Fractal Design", "FD-CA-MESH-C", 
                8000, 24, 7, "корпус", "Форм-фактор: ATX, Материал: сталь/пластик"));
            
             // Инициализация шаблонных сборок
            ComputerBuild gamingBuild = manager.CreateTemplateBuild("Игровой ПК среднего класса", "игровой");
           
            // TODO: Добавить компоненты в сборку
            var cpu = manager.FindComponentsByType("процессор").FirstOrDefault();
            var mb = manager.FindComponentsByType("материнская плата").FirstOrDefault();
            var gpu = manager.FindComponentsByType("видеокарта").FirstOrDefault();
            var ram = manager.FindComponentsByType("ОЗУ").FirstOrDefault();
            var ssd = manager.FindComponentsByType("SSD").FirstOrDefault();
            var psu = manager.FindComponentsByType("блок питания").FirstOrDefault();
            var casePC = manager.FindComponentsByType("корпус").FirstOrDefault();

            if (cpu != null) gamingBuild.AddMainComponent(cpu, "процессор");
            if (mb != null) gamingBuild.AddMainComponent(mb, "материнская плата");
            if (gpu != null) gamingBuild.AddMainComponent(gpu, "видеокарта");
            if (ram != null) gamingBuild.AddMainComponent(ram, "ОЗУ");
            if (ssd != null) gamingBuild.AddMainComponent(ssd, "SSD");
            if (psu != null) gamingBuild.AddMainComponent(psu, "блок питания");
            if (casePC != null) gamingBuild.AddMainComponent(casePC, "корпус");
        }
        
        // TODO 1: Конфигуратор компьютера
        public void ConfigureComputer()
        {
            Console.WriteLine("=== КОНФИГУРАТОР КОМПЬЮТЕРА ===");
            
            // 1. Запросить название и назначение сборки
            // 2. Создать новую сборку через manager.CreateTemplateBuild()
            // 3. Поочередно выбирать компоненты по типам через manager.FindComponentsByType()
            // 4. Добавлять выбранные компоненты в сборку через build.AddMainComponent()
            // 5. После выбора всех основных компонентов предложить дополнительные
            // 6. Проверить совместимость и показать предупреждения
            // 7. Рассчитать стоимость и оценку производительности
            // 8. Предложить сохранить конфигурацию
        }
        
        // TODO 1: Показать шаблонные сборки
        public void ShowTemplateBuilds()
        {
            Console.WriteLine("=== ГОТОВЫЕ КОНФИГУРАЦИИ ===");
            
            // Получить все шаблонные сборки через manager.GetTemplateBuilds()
            // Сгруппировать сборки по назначению (игровые, офисные и т.д.)
            // Для каждой сборки вызвать ShowBuildInfo()
        }
        
        // TODO 2: Оформить заказ на сборку
        public void PlaceOrder()
        {
            Console.WriteLine("=== ОФОРМЛЕНИЕ ЗАКАЗА ===");
            
            // 1. Найти или зарегистрировать заказчика
            // 2. Выбрать сборку (из шаблонных или создать новую)
            // 3. Проверить наличие всех компонентов через manager.CheckBuildAvailability()
            // 4. Если компонентов нет - предложить аналоги или ожидание
            // 5. Создать заказ через customer.CreateOrder()
            // 6. Рассчитать стоимость
            // 7. Принять оплату через customer.PayForOrder()
            // 8. Зафиксировать продажу через manager.RecordSale()
            // 9. Выдать номер заказа и примерные сроки сборки
        }
        
        // TODO 2: Управление заказами
        public void ManageOrders()
        {
            Console.WriteLine("=== УПРАВЛЕНИЕ ЗАКАЗАМИ ===");
            
            // 1. Показать все активные заказы
            // 2. Возможность изменить статус заказа (в сборку, готов, выдан)
            // 3. Добавление примечаний по сборке
            // 4. Отслеживание этапов сборки
        }
        
        // TODO 3: Управление складом
        public void ManageInventory()
        {
            Console.WriteLine("=== УПРАВЛЕНИЕ СКЛАДОМ ===");
            
            // 1. Показать все компоненты с остатками
            // 2. Показать компоненты с низким остатком (< 3 шт.)
            // 3. Возможность пополнения склада
            // 4. Статистика продаж компонентов
            // 5. Поиск компонентов по параметрам
        }
        
        // TODO 3: Консультация по апгрейду
        public void ProvideUpgradeConsultation()
        {
            Console.WriteLine("=== КОНСУЛЬТАЦИЯ ПО АПГРЕЙДУ ===");
            
            // 1. Найти заказчика
            // 2. Просмотреть его предыдущие заказы
            // 3. Узнать текущие потребности и бюджет
            // 4. На основе существующей системы предложить варианты апгрейда
            // 5. Проверить совместимость новых компонентов со старыми
            // 6. Рассчитать стоимость и ожидаемый прирост производительности
        }
        
        // TODO 3: Показать статистику мастерской
        public void ShowWorkshopStats()
        {
            Console.WriteLine("=== СТАТИСТИКА МАСТЕРСКОЙ ===");
            
            // Вывести общую выручку через manager.GetTotalRevenue()
            // Вывести количество зарегистрированных заказчиков
            // Вывести самые популярные компоненты
            // Вывести статистику по типам сборок (игровые/офисные и т.д.)
            // Вывести компоненты с низким остатком на складе
        }
        
        // Готовый метод - главное меню
        public void ShowMainMenu()
        {
            bool running = true;
            
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== МАСТЕРСКАЯ 'КОМПЬЮТЕРНЫЙ ГЕНИЙ' ===");
                Console.WriteLine("1. Конфигуратор компьютера");
                Console.WriteLine("2. Готовые конфигурации");
                Console.WriteLine("3. Оформить заказ");
                Console.WriteLine("4. Управление заказами");
                Console.WriteLine("5. Управление складом");
                Console.WriteLine("6. Консультация по апгрейду");
                Console.WriteLine("7. Статистика мастерской");
                Console.WriteLine("8. Поиск заказчика");
                Console.WriteLine("9. Выход");
                Console.Write("Выберите: ");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        ConfigureComputer();
                        break;
                    case "2":
                        ShowTemplateBuilds();
                        break;
                    case "3":
                        PlaceOrder();
                        break;
                    case "4":
                        ManageOrders();
                        break;
                    case "5":
                        ManageInventory();
                        break;
                    case "6":
                        ProvideUpgradeConsultation();
                        break;
                    case "7":
                        ShowWorkshopStats();
                        break;
                    case "8":
                        SearchCustomer();
                        break;
                    case "9":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
                
                if (running)
                {
                    Console.WriteLine("\nНажмите Enter...");
                    Console.ReadLine();
                }
            }
        }
        
        // Метод поиска заказчика
        private void SearchCustomer()
        {
            Console.Write("Введите телефон заказчика: ");
            string phone = Console.ReadLine();
            
            Customer customer = manager.FindCustomerByPhone(phone);
            if (customer != null)
            {
                customer.ShowCustomerInfo();
            }
            else
            {
                Console.WriteLine("Заказчик не найден");
            }
        }
    }
}