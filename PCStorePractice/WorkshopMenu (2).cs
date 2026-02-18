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
        
        public void ConfigureComputer()
        {
            Console.WriteLine("=== КОНФИГУРАТОР КОМПЬЮТЕРА ===");

			ComputerBuild build = CreateBuildInteractively();
			if (build == null)
			{
				Console.WriteLine("Сборка не была создана.");
				return;
			}

			Console.WriteLine("\nИтоговая конфигурация:");
			build.ShowBuildInfo();

			Console.Write("\nКонфигурация уже добавлена в список шаблонов. Оставить как есть? (Enter - да, n - удалить из шаблонов): ");
			string answer = Console.ReadLine();
			if (!string.IsNullOrEmpty(answer) && answer.Trim().ToLower() == "n")
			{
				var templates = manager.GetTemplateBuilds();
				templates.Remove(build);
				Console.WriteLine("Конфигурация не сохранена как шаблон.");
			}
		}
        
        // TODO 1: Показать шаблонные сборки
        public void ShowTemplateBuilds()
        {
            Console.WriteLine("=== ГОТОВЫЕ КОНФИГУРАЦИИ ===");

			var Group = manager.GetTemplateBuilds().GroupBy(item => item.Purpose);
			foreach (var group in Group)
			{
				Console.WriteLine(group.Key);
				foreach (var item in group)
				{
					item.ShowBuildInfo();
				}
			}
		}
        
        // TODO 2: Оформить заказ на сборку
        public void PlaceOrder()
        {
            Console.WriteLine("=== ОФОРМЛЕНИЕ ЗАКАЗА ===");

			// 1. Найти или зарегистрировать заказчика
			Customer customer = GetOrRegisterCustomer();
			if (customer == null)
			{
				Console.WriteLine("Заказчик не выбран. Отмена оформления заказа.");
				return;
			}

			// 2. Выбрать сборку (из шаблонных или создать новую)
			ComputerBuild build = ChooseBuildForOrder();
			if (build == null)
			{
				Console.WriteLine("Сборка не выбрана. Отмена оформления заказа.");
				return;
			}

			// 3. Проверить наличие всех компонентов через manager.CheckBuildAvailability()
			if (!manager.CheckBuildAvailability(build))
			{
				Console.WriteLine("Не все компоненты для выбранной сборки есть в наличии.");
				Console.WriteLine("Вы можете изменить конфигурацию или подождать пополнения склада.");
				return;
			}

			// 5. Создать заказ через customer.CreateOrder()
			Customer.Order order = customer.CreateOrder(build);
			order.OrderNumber = manager.GetNextOrderNumber();

			// 6. Рассчитать стоимость (уже в заказе)
			Console.WriteLine($"\nСтоимость заказа: {order.TotalCost} руб.");

			// 7. Принять оплату
			Console.Write("Введите сумму оплаты (0 - без оплаты): ");
			string amountText = Console.ReadLine();
			if (decimal.TryParse(amountText, out decimal amount) && amount > 0)
			{
				bool paid = customer.PayForOrder(order, amount);
				if (paid)
				{
					// 8. Зафиксировать продажу
					manager.RecordSale(amount);
					Console.WriteLine("Оплата успешно принята.");
				}
				else
				{
					Console.WriteLine("Не удалось принять оплату.");
				}
			}

			// 9. Вывести номер заказа и сроки
			Console.WriteLine($"\nЗаказ успешно создан. Номер заказа: {order.OrderNumber}");
			Console.WriteLine("Примерный срок сборки: 3-5 рабочих дней.");
		}
        
        // TODO 2: Управление заказами
        public void ManageOrders()
        {
            Console.WriteLine("=== УПРАВЛЕНИЕ ЗАКАЗАМИ ===");

			// 1. Показать все активные заказы
			Console.Write("Введите телефон заказчика (пусто - показать всех): ");
			string phone = Console.ReadLine();

			List<Customer> customers = new List<Customer>();
			if (string.IsNullOrWhiteSpace(phone))
			{
				customers = manager.GetAllCustomers();
			}
			else
			{
				var customer = manager.FindCustomerByPhone(phone);
				if (customer != null)
				{
					customers.Add(customer);
				}
			}

			if (customers.Count == 0)
			{
				Console.WriteLine("Заказчики не найдены.");
				return;
			}

			foreach (var cust in customers)
			{
				Console.WriteLine($"\nЗаказчик: {cust.FullName} ({cust.Phone})");
				var activeOrders = cust.GetActiveOrders();
				if (activeOrders.Count == 0)
				{
					Console.WriteLine("  Активных заказов нет.");
				}
				else
				{
					foreach (var order in activeOrders)
					{
						Console.WriteLine($"  Заказ #{order.OrderNumber}: {order.Status}, сумма {order.TotalCost} руб., оплачено {order.PaidAmount} руб.");
					}
				}
			}

			// 2. Возможность изменить статус заказа
			Console.Write("\nВведите телефон заказчика для изменения заказа (или Enter для выхода): ");
			string editPhone = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(editPhone))
			{
				return;
			}

			var editCustomer = manager.FindCustomerByPhone(editPhone);
			if (editCustomer == null)
			{
				Console.WriteLine("Заказчик не найден.");
				return;
			}

			var editOrders = editCustomer.GetActiveOrders();
			if (editOrders.Count == 0)
			{
				Console.WriteLine("У заказчика нет активных заказов.");
				return;
			}

			Console.Write("Введите номер заказа для редактирования: ");
			string orderNumberText = Console.ReadLine();
			if (!int.TryParse(orderNumberText, out int orderNumber))
			{
				Console.WriteLine("Неверный номер заказа.");
				return;
			}

			var editOrder = editOrders.FirstOrDefault(o => o.OrderNumber == orderNumber);
			if (editOrder == null)
			{
				Console.WriteLine("Заказ не найден среди активных.");
				return;
			}

			Console.WriteLine("Выберите действие:");
			Console.WriteLine("1. Изменить статус заказа");
			Console.WriteLine("2. Добавить примечание");
			Console.Write("Ваш выбор: ");
			string action = Console.ReadLine();

			if (action == "1")
			{
				Console.WriteLine("Введите новый статус (Оформлен / Оплачен / В сборке / Готов / Выдан): ");
				string newStatus = Console.ReadLine();
				if (!string.IsNullOrWhiteSpace(newStatus))
				{
					editOrder.Status = newStatus;
					if (newStatus == "Готов" || newStatus == "Выдан")
					{
						editOrder.CompletionDate = DateTime.Now;
					}
					Console.WriteLine("Статус заказа обновлён.");
				}
			}
			else if (action == "2")
			{
				Console.Write("Введите примечание: ");
				string note = Console.ReadLine();
				if (!string.IsNullOrWhiteSpace(note))
				{
					if (string.IsNullOrEmpty(editOrder.Notes))
					{
						editOrder.Notes = note;
					}
					else
					{
						editOrder.Notes += Environment.NewLine + note;
					}
					Console.WriteLine("Примечание добавлено.");
				}
			}
		}
        
        // TODO 3: Управление складом
        public void ManageInventory()
        {
            Console.WriteLine("=== УПРАВЛЕНИЕ СКЛАДОМ ===");

			// 1. Показать все компоненты с остатками
			var components = manager.GetAllComponents();
			if (components.Count == 0)
			{
				Console.WriteLine("На складе нет компонентов.");
				return;
			}

			Console.WriteLine("\nВсе компоненты:");
			foreach (var comp in components)
			{
				Console.WriteLine($"{comp.Id}. {comp} | Остаток: {comp.StockQuantity}");
			}

			// 2. Показать компоненты с низким остатком (< 3 шт.)
			var lowStock = components.Where(c => c.StockQuantity < 3).ToList();
			if (lowStock.Count > 0)
			{
				Console.WriteLine("\nКомпоненты с низким остатком (< 3):");
				foreach (var comp in lowStock)
				{
					Console.WriteLine($"{comp.Id}. {comp} | Остаток: {comp.StockQuantity}");
				}
			}

			// 3. Возможность пополнения склада
			string restockAnswer;
			do
			{
				Console.Write("\nХотите пополнить склад? (y/n): ");
				restockAnswer = Console.ReadLine();
			} while (restockAnswer != "y" && restockAnswer != "n");

			if (!string.IsNullOrEmpty(restockAnswer) && restockAnswer.Trim().ToLower() == "y")
			{
				Console.Write("Введите ID компонента: ");
				string idText = Console.ReadLine();
				Console.Write("Введите количество для добавления: ");
				string qtyText = Console.ReadLine();

				if (int.TryParse(idText, out int id) && int.TryParse(qtyText, out int qty) && qty > 0)
				{
					var comp = components.FirstOrDefault(c => c.Id == id);
					if (comp != null)
					{
						comp.Restock(qty);
						Console.WriteLine("Склад успешно пополнен.");
					}
					else
					{
						Console.WriteLine("Компонент с таким ID не найден.");
					}
				}
				else
				{
					Console.WriteLine("Неверные данные для пополнения склада.");
				}
			}

			// 5. Поиск компонентов по параметрам (по типу)
			Console.Write("\nВведите тип компонента для поиска (например, 'процессор') или Enter для пропуска: ");
			string typeSearch = Console.ReadLine();
			if (!string.IsNullOrWhiteSpace(typeSearch))
			{
				var found = components
					.Where(c => c.ComponentType != null && c.ComponentType.IndexOf(typeSearch, StringComparison.OrdinalIgnoreCase) >= 0)
					.ToList();
				if (found.Count == 0)
				{
					Console.WriteLine("Компоненты по указанному типу не найдены.");
				}
				else
				{
					Console.WriteLine("\nНайденные компоненты:");
					foreach (var comp in found)
					{
						Console.WriteLine($"{comp.Id}. {comp} | Остаток: {comp.StockQuantity}");
					}
				}
			}
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

		// Вспомогательный метод: создание сборки в интерактивном режиме
		private ComputerBuild CreateBuildInteractively()
		{
			string name;
			do
			{
				Console.Write("Введите название сборки: ");
				name = Console.ReadLine();

			} while (string.IsNullOrEmpty(name));

			string purpose;
			do
			{
				Console.Write("Введите назначение сборки: ");
				purpose = Console.ReadLine();

			} while (string.IsNullOrEmpty(purpose));

			ComputerBuild build = manager.CreateTemplateBuild(name, purpose);

			Console.WriteLine("\nВыбор основных компонентов.");
			var cpu = SelectComponentByType("процессор");
			if (cpu != null) build.AddMainComponent(cpu, "процессор");

			var mb = SelectComponentByType("материнская плата");
			if (mb != null) build.AddMainComponent(mb, "материнская плата");

			var gpu = SelectComponentByType("видеокарта");
			if (gpu != null) build.AddMainComponent(gpu, "видеокарта");

			var ram = SelectComponentByType("ОЗУ");
			if (ram != null) build.AddMainComponent(ram, "ОЗУ");

			var ssd = SelectComponentByType("SSD");
			if (ssd != null) build.AddMainComponent(ssd, "SSD");

			var psu = SelectComponentByType("блок питания");
			if (psu != null) build.AddMainComponent(psu, "блок питания");

			var @case = SelectComponentByType("корпус");
			if (@case != null) build.AddMainComponent(@case, "корпус");

			// Дополнительные компоненты
			Console.Write("\nДобавить дополнительные компоненты? (y/n): ");
			string addMore = Console.ReadLine();
			while (!string.IsNullOrEmpty(addMore) && addMore.Trim().ToLower() == "y")
			{
				var allComponents = manager.GetAllComponents();
				Console.WriteLine("\nДоступные компоненты для добавления:");
				foreach (var comp in allComponents)
				{
					Console.WriteLine($"{comp.Id}. {comp} | Остаток: {comp.StockQuantity}");
				}

				Console.Write("Введите ID компонента для добавления (0 - отмена): ");
				string idText = Console.ReadLine();
				if (int.TryParse(idText, out int id) && id > 0)
				{
					var comp = allComponents.FirstOrDefault(c => c.Id == id);
					if (comp != null)
					{
						build.AddAdditionalComponent(comp);
						Console.WriteLine("Компонент добавлен.");
					}
					else
					{
						Console.WriteLine("Компонент с таким ID не найден.");
					}
				}

				Console.Write("Добавить ещё один дополнительный компонент? (y/n): ");
				addMore = Console.ReadLine();
			}

			return build;
		}

		// Вспомогательный метод: выбор компонента по типу
		private Component SelectComponentByType(string componentType)
		{
			var list = manager.FindComponentsByType(componentType);
			if (list.Count == 0)
			{
				Console.WriteLine($"\nКомпоненты типа '{componentType}' не найдены.");
				return null;
			}

			Console.WriteLine($"\nДоступные компоненты ({componentType}):");
			for (int i = 0; i < list.Count; i++)
			{
				Console.WriteLine($"{i + 1}. {list[i]} | Остаток: {list[i].StockQuantity}");
			}

			Console.Write("Выберите номер компонента (0 - пропустить): ");
			string choiceText = Console.ReadLine();
			if (int.TryParse(choiceText, out int choice) &&
				choice > 0 && choice <= list.Count)
			{
				return list[choice - 1];
			}

			return null;
		}

		// Вспомогательный метод: поиск или регистрация заказчика
		private Customer GetOrRegisterCustomer()
		{
			Console.Write("Введите телефон заказчика: ");
			string phone = Console.ReadLine();

			Customer customer = manager.FindCustomerByPhone(phone);
			if (customer != null)
			{
				Console.WriteLine("Найден существующий заказчик:");
				customer.ShowCustomerInfo();
				return customer;
			}

			Console.Write("Заказчик не найден. Зарегистрировать нового? (y/n): ");
			string answer = Console.ReadLine();
			if (string.IsNullOrEmpty(answer) || answer.Trim().ToLower() != "y")
			{
				return null;
			}

			Console.Write("Введите ФИО: ");
			string fullName = Console.ReadLine();
			Console.Write("Введите Email: ");
			string email = Console.ReadLine();
			string type;
			do
			{
				Console.Write("Введите тип заказчика (частное лицо / компания / учебное заведение): ");
				type = Console.ReadLine();
			} while (string.IsNullOrEmpty(type) || type != "частное лицо" && type != "компания" && type != "учебное заведение");

			customer = manager.RegisterCustomer(fullName, phone, email, type);
			Console.WriteLine("Заказчик успешно зарегистрирован.");
			return customer;
		}

		// Вспомогательный метод: выбор сборки для заказа
		private ComputerBuild ChooseBuildForOrder()
		{
			var builds = manager.GetTemplateBuilds();
			if (builds.Count == 0)
			{
				Console.WriteLine("Шаблонные сборки отсутствуют.");
				Console.Write("Создать новую сборку? (y/n): ");
				string answer = Console.ReadLine();
				if (!string.IsNullOrEmpty(answer) && answer.Trim().ToLower() == "y")
				{
					return CreateBuildInteractively();
				}

				return null;
			}

			Console.WriteLine("\nДоступные сборки:");
			for (int i = 0; i < builds.Count; i++)
			{
				Console.WriteLine($"{i + 1}. #{builds[i].Id} {builds[i].Name} ({builds[i].Purpose})");
			}

			Console.Write("Выберите номер сборки (0 - создать новую): ");
			string choiceText = Console.ReadLine();
			if (int.TryParse(choiceText, out int choice))
			{
				if (choice == 0)
				{
					return CreateBuildInteractively();
				}

				if (choice > 0 && choice <= builds.Count)
				{
					return builds[choice - 1];
				}
			}

			return null;
		}
	}
}