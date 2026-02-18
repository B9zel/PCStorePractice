// TODO:
// 1. Регистрация новых заказчиков
// 2. Управление складом компонентов
// 3. Контроль заказов и сборок

using System;
using System.Collections.Generic;

namespace ComputerWorkshop
{
    public class WorkshopManager
    {
        private List<Customer> customers = new List<Customer>();
        private List<Component> components = new List<Component>();
        private List<ComputerBuild> templateBuilds = new List<ComputerBuild>(); // Шаблонные сборки
        
        private int nextCustomerId = 1000;
        private int nextOrderNumber = 1;
        private int nextBuildId = 1;
        private decimal totalRevenue = 0;
        
        // TODO 1: Зарегистрировать нового заказчика
        public Customer RegisterCustomer(string fullName, string phone, string email, string customerType)
        {
			Customer customer = new Customer
			{
				Id = nextCustomerId,
				FullName = fullName,
				Phone = phone,
				Email = email,
				CustomerType = customerType,
				RegistrationDate = DateTime.Now
			};

			customers.Add(customer);
			nextCustomerId++;

			return customer;
		}
        
        // TODO 2: Найти заказчика по телефону
        public Customer FindCustomerByPhone(string phone)
        {
			return customers.Find(x => x.Phone == phone);
		}
        
        // TODO 2: Найти компоненты по типу
        public List<Component> FindComponentsByType(string componentType)
        {
			List<Component> result = new List<Component>();

			foreach (Component component in components)
			{
				if (component.ComponentType == componentType && component.StockQuantity > 0)
				{
					result.Add(component);
				}
			}
			return result;
		}
        
        // TODO 3: Создать шаблонную сборку
        public ComputerBuild CreateTemplateBuild(string name, string purpose)
        {
			ComputerBuild NewBuild = new ComputerBuild(nextBuildId, name, purpose);
			AddTemplateBuild(NewBuild);
			nextBuildId++;

			return NewBuild;
		}
        
        // TODO 3: Проверить наличие компонентов для сборки
        public bool CheckBuildAvailability(ComputerBuild build)
        {
			foreach (var item in build.GetAdditionalComponents())
			{
				if (!HasComponentInStock(item))
				{
					return false;
				}
			}
			return HasComponentInStock(build.Processor) && HasComponentInStock(build.Motherboard) && HasComponentInStock(build.GraphicsCard) &&
					HasComponentInStock(build.RAM) && HasComponentInStock(build.Storage) && HasComponentInStock(build.PowerSupply) &&
					HasComponentInStock(build.Case);
		}
		private bool HasComponentInStock(Component comp)
		{
			if (comp == null) return true;

			foreach (Component component in components)
			{
				if (component.IsInStock() && component.Manufacturer == comp.Manufacturer && component.Name == comp.Name
					&& component.Model == comp.Model && component.ComponentType == comp.ComponentType)
				{
					return true;
				}
			}
			return false;
		}

		// TODO 3: Зафиксировать продажу
		public void RecordSale(decimal amount)
        {
            // Увеличить totalRevenue на amount
        }
        
        // TODO 3: Получить следующий номер заказа
        public int GetNextOrderNumber()
        {
            return nextOrderNumber++;
        }
        
        // Готовые методы:
        public void AddComponent(Component component)
        {
            components.Add(component);
        }
        
        public void AddTemplateBuild(ComputerBuild build)
        {
            templateBuilds.Add(build);
        }
        
        public List<Component> GetAllComponents()
        {
            return components;
        }
        
        public List<ComputerBuild> GetTemplateBuilds()
        {
            return templateBuilds;
        }
        
        public List<Customer> GetAllCustomers()
        {
            return customers;
        }
        
        public decimal GetTotalRevenue()
        {
            return totalRevenue;
        }
        
        public int GetCustomerCount()
        {
            return customers.Count;
        }
        
        public int GetNextBuildId()
        {
            return nextBuildId++;
        }
    }
}