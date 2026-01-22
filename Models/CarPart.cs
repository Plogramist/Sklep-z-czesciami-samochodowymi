namespace SklepSamochodowy.Models
{
    public class CarPart
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal StockValue => Price * Quantity;

        public override string ToString()
        {
            return $"[{Id}] {Name} | {Category} | {Price:0.00} PLN | Ilość: {Quantity}";
        }
    }
}
