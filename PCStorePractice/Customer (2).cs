// TODO:
// 1. Реализовать учет данных заказчика и его требований
// 2. Реализовать оформление заказа на сборку
// 3. Реализовать историю заказов и обслуживания

using System;
using System.Collections.Generic;

namespace ComputerWorkshop
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string CustomerType { get; set; }

		private List<string> requirements = new List<string>();
        private List<Order> orders = new List<Order>(); // История заказов
        private List<ComputerBuild> savedConfigs = new List<ComputerBuild>(); // Сохраненные конфигурации
        
        public class Order
        {
            public int OrderNumber { get; set; }
            public ComputerBuild Build { get; set; }
            public DateTime OrderDate { get; set; }
            public DateTime? CompletionDate { get; set; } // Дата завершения сборки
            public decimal TotalCost { get; set; }
            public decimal PaidAmount { get; set; }
            public string Status { get; set; } = "Оформлен"; // Оформлен, Оплачен, В сборке, Готов, Выдан
            public string Notes { get; set; } // Примечания по сборке
        }
        
        // TODO 2: Создать новый заказ
        public Order CreateOrder(ComputerBuild build)
        {
            // Создать новый объект Order
            // Установить текущую дату как дату заказа
            // Рассчитать общую стоимость через build.CalculateTotalCost()
            // Установить начальный статус
            // Добавить заказ в историю orders
            // Вернуть созданный заказ
            return null;
        }
        
        // TODO 2: Оплатить заказ
        public bool PayForOrder(Order order, decimal amount)
        {
            // Проверить что сумма оплаты достаточна
            // Увеличить PaidAmount
            // Если оплачена полная сумма - изменить статус на "Оплачен"
            // Вернуть true если оплата успешна
            return false;
        }
        
        // TODO 3: Сохранить конфигурацию
        public void SaveConfiguration(ComputerBuild build)
        {
            // Добавить сборку в список savedConfigs
        }
        
        // TODO 3: Получить рекомендованные обновления
        public List<Component> GetRecommendedUpgrades(List<Component> availableComponents)
        {
            List<Component> upgrades = new List<Component>();
            
            // На основе истории заказов и требований заказчика
            // Предложить компоненты для апгрейда существующих систем
            return upgrades;
        }
        
        // TODO 1: Добавить требование
        public void AddRequirement(string requirement)
        {
            // Добавить требование в список requirements если его там еще нет
        }
        
        // TODO 3: Получить активные заказы
        public List<Order> GetActiveOrders()
        {
            List<Order> active = new List<Order>();
            
            // Найти все заказы со статусами "Оформлен", "Оплачен", "В сборке"
            return active;
        }
        
        // Показать информацию о заказчике
        public void ShowCustomerInfo()
        {
            Console.WriteLine($"Заказчик: {FullName}");
            Console.WriteLine($"Телефон: {Phone}");
            Console.WriteLine($"Email: {Email}");
            // TODO 1: Вывести тип заказчика
            Console.WriteLine($"Зарегистрирован: {RegistrationDate:dd.MM.yyyy}");
            Console.WriteLine($"Всего заказов: {orders.Count}");
            Console.WriteLine($"Активных заказов: {GetActiveOrders().Count}");
            Console.WriteLine($"Сохраненных конфигураций: {savedConfigs.Count}");
            
            if (requirements.Count > 0)
            {
                Console.WriteLine($"Требования: {string.Join(", ", requirements)}");
            }
        }
    }
}