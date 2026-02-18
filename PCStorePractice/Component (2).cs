// TODO:
// 1. Добавить свойства для классификации компонента (тип, совместимость)
// 2. Реализовать проверку корректности данных (цена, гарантия)
// 3. Реализовать информативное строковое представление компонента

namespace ComputerWorkshop
{
    public class Component
    {
        public int Id { get; set; }                    // Артикул
        public string Name { get; set; }               // Название
        public string Manufacturer { get; set; }       // Производитель (Intel, AMD, NVIDIA, ASUS)
        public string Model { get; set; }              // Модель
        public decimal Price { get; set; }             // Цена
        public int WarrantyMonths { get; set; }        // Гарантия в месяцах
        public int StockQuantity { get; set; }         // Количество на складе

		public string ComponentType { get; set; }
		public string Specification { get; set; }

		public Component(int id, string name, string manufacturer, string model, 
                        decimal price, int warranty, int stock, string type, string specs)
        {
            Id = id;
            Name = name;
            Manufacturer = manufacturer;
            Model = model;
            
            // TODO 2: Проверить что цена не отрицательная
            // Если цена < 0, установить минимальную цену 100
            
            // TODO 2: Проверить что гарантия не отрицательная
            // Если warranty < 0, установить 0 (без гарантии)
            
            StockQuantity = stock;
            
            // TODO 1: Сохранить тип компонента и спецификации
        }
        
        public override string ToString()
        {
            // TODO 3: Вернуть строку в формате "Процессор Intel Core i5-13400F (Intel) - 25000 руб. (24 мес гарантии)"
            return $"{Name} ({Manufacturer})";
        }
        
        // Проверить наличие на складе
        public bool IsInStock(int quantity = 1)
        {
            return StockQuantity >= quantity;
        }
        
        // Продать компонент (уменьшить остаток)
        public bool Sell(int quantity)
        {
            if (StockQuantity >= quantity)
            {
                StockQuantity -= quantity;
                return true;
            }
            return false;
        }
        
        // Пополнить склад
        public void Restock(int quantity)
        {
            StockQuantity += quantity;
        }
        
        // Проверить совместимость с другим компонентом (базовая проверка)
        public bool IsCompatibleWith(Component other)
        {
            // Базовая проверка совместимости (например, Intel CPU с Intel-материнкой)
            return true;
        }
    }
}