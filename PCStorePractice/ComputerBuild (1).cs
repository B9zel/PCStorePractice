// TODO:
// 1. Реализовать создание конфигураций компьютеров
// 2. Реализовать проверку совместимости компонентов
// 3. Реализовать расчет стоимости сборки

using System;
using System.Collections.Generic;

namespace ComputerWorkshop
{
    public class ComputerBuild
    {
        public int Id { get; set; }                    // Номер сборки
        public string Name { get; set; }               // Название конфигурации
        public string Purpose { get; set; }            // Назначение (игровой, офисный, рабочая станция)
        public DateTime CreationDate { get; set; }     // Дата создания конфигурации
        
        // Основные компоненты
        public Component Processor { get; set; }       // Процессор
        public Component Motherboard { get; set; }     // Материнская плата
        public Component GraphicsCard { get; set; }    // Видеокарта
        public Component RAM { get; set; }             // Оперативная память
        public Component Storage { get; set; }         // Накопитель (SSD/HDD)
        public Component PowerSupply { get; set; }     // Блок питания
        public Component Case { get; set; }            // Корпус
        
        private List<Component> additionalComponents = new List<Component>(); // Доп. компоненты
        private decimal assemblyCost = 3000;           // Стоимость сборки

		private string BuildStatus { get; set; }      // (статус: в проекте, компоненты заказаны, в сборке, готов)
        
        public ComputerBuild(int id, string name, string purpose)
        {
            Id = id;
            Name = name;
            Purpose = purpose;
            CreationDate = DateTime.Now;
			BuildStatus = "в проекте";
		}
        
        // TODO 2: Добавить основной компонент
        public bool AddMainComponent(Component component, string componentType)
        {
            // Проверить тип компонента и установить в соответствующее свойство
            // Проверить базовую совместимость с уже установленными компонентами
            // Если совместимость есть - добавить и вернуть true
            // Если нет - вернуть false
            return false;
        }
        
        // TODO 2: Добавить дополнительный компонент
        public void AddAdditionalComponent(Component component)
        {
            // Добавить компонент в список additionalComponents
        }
        
        // TODO 3: Проверить совместимость всех компонентов
        public List<string> CheckCompatibility()
        {
            List<string> issues = new List<string>();
            
            // Проверить основные пары совместимости:
            // 1. Процессор и материнская плата (сокет)
            // 2. Блок питания и видеокарта (мощность)
            // 3. Корпус и материнская плата (форм-фактор)
            // 4. ОЗУ и материнская плата (тип и частота)
            // Добавить найденные проблемы в список issues
            
            return issues;
        }
        
        // TODO 3: Рассчитать общую стоимость
        public decimal CalculateTotalCost()
        {
            decimal total = assemblyCost;
            
            // Сложить цены всех основных компонентов (если они есть)
            // Сложить цены всех дополнительных компонентов
            return total;
        }
        
        // TODO 3: Рассчитать примерную производительность
        public string EstimatePerformance()
        {
            // На основе компонентов определить примерную производительность:
            // - Для игрового ПК: оценка в FPS в современных играх
            // - Для рабочего ПК: оценка скорости рендеринга/компиляции
            // - Для офисного ПК: оценка общей производительности
            return "Средняя";
        }
        
        // Показать информацию о сборке
        public void ShowBuildInfo()
        {
            Console.WriteLine($"=== СБОРКА #{Id}: {Name} ===");
            Console.WriteLine($"Назначение: {Purpose}");
            Console.WriteLine($"Дата создания: {CreationDate:dd.MM.yyyy}");
            Console.WriteLine($"Статус: {/* TODO 1: Вывести статус */}");
            
            Console.WriteLine("\nКомпоненты:");
            if (Processor != null) Console.WriteLine($"  Процессор: {Processor}");
            if (Motherboard != null) Console.WriteLine($"  Материнская плата: {Motherboard}");
            if (GraphicsCard != null) Console.WriteLine($"  Видеокарта: {GraphicsCard}");
            if (RAM != null) Console.WriteLine($"  ОЗУ: {RAM}");
            if (Storage != null) Console.WriteLine($"  Накопитель: {Storage}");
            if (PowerSupply != null) Console.WriteLine($"  Блок питания: {PowerSupply}");
            if (Case != null) Console.WriteLine($"  Корпус: {Case}");
            
            if (additionalComponents.Count > 0)
            {
                Console.WriteLine("\nДополнительные компоненты:");
                foreach (var comp in additionalComponents)
                {
                    Console.WriteLine($"  {comp}");
                }
            }
            
            Console.WriteLine($"\nОбщая стоимость: {CalculateTotalCost()} руб.");
            Console.WriteLine($"Оценка производительности: {EstimatePerformance()}");
            
            var issues = CheckCompatibility();
            if (issues.Count > 0)
            {
                Console.WriteLine("\n⚠ Предупреждения по совместимости:");
                foreach (var issue in issues)
                {
                    Console.WriteLine($"  - {issue}");
                }
            }
        }
    }
}