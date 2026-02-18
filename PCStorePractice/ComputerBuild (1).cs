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
        
        public bool AddMainComponent(Component component, string componentType)
        {
			if (component == null || string.IsNullOrWhiteSpace(componentType))
			{
				return false;
			}

			string type = componentType.ToLower();
			Component previous = null;

			switch (type)
			{
				case "процессор":
					previous = Processor;
					Processor = component;
					break;
				case "материнская плата":
					previous = Motherboard;
					Motherboard = component;
					break;
				case "видеокарта":
					previous = GraphicsCard;
					GraphicsCard = component;
					break;
				case "озу":
					previous = RAM;
					RAM = component;
					break;
				case "ssd":
				case "накопитель":
					previous = Storage;
					Storage = component;
					break;
				case "блок питания":
					previous = PowerSupply;
					PowerSupply = component;
					break;
				case "корпус":
					previous = Case;
					Case = component;
					break;
				default:
					return false;
			}

			// Базовая проверка совместимости после добавления
			var issues = CheckCompatibility();
			if (issues.Count > 0)
			{
				// Возвращаем предыдущее состояние, если есть проблемы
				switch (type)
				{
					case "процессор":
						Processor = previous;
						break;
					case "материнская плата":
						Motherboard = previous;
						break;
					case "видеокарта":
						GraphicsCard = previous;
						break;
					case "озу":
						RAM = previous;
						break;
					case "ssd":
					case "накопитель":
						Storage = previous;
						break;
					case "блок питания":
						PowerSupply = previous;
						break;
					case "корпус":
						Case = previous;
						break;
				}

				return false;
			}

			return true;
		}
        
        public void AddAdditionalComponent(Component component)
        {
			additionalComponents.Add(component);
		}
        
        public List<string> CheckCompatibility()
        {
            List<string> issues = new List<string>();

			if (Processor != null && Motherboard != null &&
					  !string.IsNullOrEmpty(Processor.Specification) &&
					  !string.IsNullOrEmpty(Motherboard.Specification))
			{
				string cpuSocket = ExtractValue(Processor.Specification, "Сокет");
				string mbSocket = ExtractValue(Motherboard.Specification, "Сокет");

				if (!string.IsNullOrEmpty(cpuSocket) &&
					!string.IsNullOrEmpty(mbSocket) &&
					!string.Equals(cpuSocket, mbSocket, StringComparison.OrdinalIgnoreCase))
				{
					issues.Add($"Несовместимость сокета процессора ({cpuSocket}) и материнской платы ({mbSocket}).");
				}
			}

			// 2. Блок питания и видеокарта: простая проверка по мощности БП
			if (PowerSupply != null && GraphicsCard != null &&
				!string.IsNullOrEmpty(PowerSupply.Specification))
			{
				int psuWatts = ExtractFirstNumber(PowerSupply.Specification);
				if (psuWatts > 0 && psuWatts < 500)
				{
					issues.Add("Рекомендуется более мощный блок питания для данной видеокарты (меньше 500W).");
				}
			}

			// 3. Корпус и материнская плата: проверка форм-фактора
			if (Case != null && Motherboard != null &&
				!string.IsNullOrEmpty(Case.Specification) &&
				!string.IsNullOrEmpty(Motherboard.Specification))
			{
				string caseFormFactor = ExtractValue(Case.Specification, "Форм-фактор");
				string mbFormFactor = ExtractValue(Motherboard.Specification, "Форм-фактор");

				// Если форм-фактор корпуса меньше материнской платы - проблема
				// Например: корпус mATX не подойдет для ATX материнской платы
				if (!string.IsNullOrEmpty(caseFormFactor) && !string.IsNullOrEmpty(mbFormFactor))
				{
					string caseFF = caseFormFactor.ToUpper();
					string mbFF = mbFormFactor.ToUpper();

					// Проверка совместимости форм-факторов
					if (mbFF.Contains("ATX") && !caseFF.Contains("ATX"))
					{
						issues.Add($"Форм-фактор корпуса ({caseFormFactor}) может быть несовместим с материнской платой ({mbFormFactor}).");
					}
					else if (mbFF.Contains("MICRO-ATX") || mbFF.Contains("M-ATX"))
					{
						if (!caseFF.Contains("ATX") && !caseFF.Contains("MICRO") && !caseFF.Contains("M-ATX"))
						{
							issues.Add($"Форм-фактор корпуса ({caseFormFactor}) может быть несовместим с материнской платой ({mbFormFactor}).");
						}
					}
					else if (mbFF.Contains("MINI-ITX") || mbFF.Contains("ITX"))
					{
						if (!caseFF.Contains("ITX") && !caseFF.Contains("ATX") && !caseFF.Contains("MICRO"))
						{
							issues.Add($"Форм-фактор корпуса ({caseFormFactor}) может быть несовместим с материнской платой ({mbFormFactor}).");
						}
					}
				}
			}

			// 4. ОЗУ и материнская плата: проверка типа и частоты памяти
			if (RAM != null && Motherboard != null &&
				!string.IsNullOrEmpty(RAM.Specification) &&
				!string.IsNullOrEmpty(Motherboard.Specification))
			{
				string ramType = ExtractValue(RAM.Specification, "Тип");
				string mbRamType = ExtractValue(Motherboard.Specification, "Память");

				// Проверка типа памяти
				if (!string.IsNullOrEmpty(ramType) && !string.IsNullOrEmpty(mbRamType))
				{
					if (!mbRamType.ToUpper().Contains(ramType.ToUpper()))
					{
						issues.Add($"Тип памяти ОЗУ ({ramType}) может быть несовместим с материнской платой (поддерживается: {mbRamType}).");
					}
				}

				// Проверка частоты памяти
				int ramFreq = ExtractFirstNumber(RAM.Specification);
				if (ramFreq > 0 && !string.IsNullOrEmpty(Motherboard.Specification))
				{
					// Проверяем, поддерживает ли материнская плата указанную частоту
					// Обычно материнские платы поддерживают частоты до определенного максимума
					// Для простоты проверяем, что частота не слишком высокая (например, больше 5000 MHz может быть проблемой)
					if (ramFreq > 5000)
					{
						issues.Add($"Частота ОЗУ ({ramFreq} MHz) может быть слишком высокой для данной материнской платы. Проверьте совместимость.");
					}
				}
			}

			return issues;
        }
        
        public decimal CalculateTotalCost()
        {
			decimal total = assemblyCost;

			// Сложить цены всех основных компонентов (если они есть)
			// Сложить цены всех дополнительных компонентов
			if (Processor != null) total += Processor.Price;
			if (Motherboard != null) total += Motherboard.Price;
			if (GraphicsCard != null) total += GraphicsCard.Price;
			if (RAM != null) total += RAM.Price;
			if (Storage != null) total += Storage.Price;
			if (PowerSupply != null) total += PowerSupply.Price;
			if (Case != null) total += Case.Price;

			foreach (var comp in additionalComponents)
			{
				if (comp != null)
				{
					total += comp.Price;
				}
			}

			return total;
		}
        
        public string EstimatePerformance()
        {
			// На основе компонентов определить примерную производительность:
			// - Для игрового ПК: оценка в FPS в современных играх
			// - Для рабочего ПК: оценка скорости рендеринга/компиляции
			// - Для офисного ПК: оценка общей производительности
			decimal cpuPrice = Processor?.Price ?? 0;
			decimal gpuPrice = GraphicsCard?.Price ?? 0;

			if (Purpose != null && Purpose.ToLower().Contains("игров"))
			{
				if (gpuPrice >= 40000 && cpuPrice >= 20000)
				{
					return "Высокая (игровой ПК)";
				}
				if (gpuPrice >= 25000)
				{
					return "Средняя (игровой ПК)";
				}
				return "Базовая (игровой ПК)";
			}

			if (Purpose != null && Purpose.ToLower().Contains("офис"))
			{
				return "Достаточная для офисных задач";
			}

			if (Purpose != null && Purpose.ToLower().Contains("рабочая станция"))
			{
				return "Высокая (рабочая станция)";
			}

			return "Средняя";
		}

		private static string ExtractValue(string specification, string key)
		{
			if (string.IsNullOrEmpty(specification) || string.IsNullOrEmpty(key))
			{
				return null;
			}

			string[] parts = specification.Split(',');
			foreach (var part in parts)
			{
				var trimmed = part.Trim();
				if (trimmed.StartsWith(key, StringComparison.OrdinalIgnoreCase))
				{
					int colonIndex = trimmed.IndexOf(':');
					if (colonIndex >= 0 && colonIndex < trimmed.Length - 1)
					{
						return trimmed.Substring(colonIndex + 1).Trim();
					}
				}
			}

			return null;
		}

		// Вспомогательный метод: найти первое число в строке
		private static int ExtractFirstNumber(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return 0;
			}

			int number = 0;
			string current = "";

			foreach (char c in text)
			{
				if (char.IsDigit(c))
				{
					current += c;
				}
				else if (current.Length > 0)
				{
					break;
				}
			}

			if (current.Length > 0 && int.TryParse(current, out number))
			{
				return number;
			}

			return 0;
		}

		public List<Component> GetAdditionalComponents()
		{
			return additionalComponents;
		}

		// Показать информацию о сборке
		public void ShowBuildInfo()
        {
            Console.WriteLine($"=== СБОРКА #{Id}: {Name} ===");
            Console.WriteLine($"Назначение: {Purpose}");
            Console.WriteLine($"Дата создания: {CreationDate:dd.MM.yyyy}");
            Console.WriteLine($"Статус: {BuildStatus}");
            
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