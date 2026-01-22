using System.Collections.Generic;
using System.Linq;
using SklepSamochodowy.Models;



namespace SklepSamochodowy.Data
{
    public class CarPartRepository
    {
        private readonly List<CarPart> _parts = new();
        private int _nextId = 1;

        public List<CarPart> GetAll()
        {
            return _parts.ToList();
        }

        public CarPart? GetById(int id)
        {
            return _parts.FirstOrDefault(p => p.Id == id);
        }

        public List<CarPart> SearchByName(string phrase)
        {
            phrase = phrase.ToLower().Trim();
            return _parts
                .Where(p => p.Name.ToLower().Contains(phrase))
                .ToList();
        }

        public CarPart Add(string name, string category, decimal price, int quantity)
        {
            var part = new CarPart
            {
                Id = _nextId++,
                Name = name,
                Category = category,
                Price = price,
                Quantity = quantity
            };

            _parts.Add(part);
            return part;
        }

        public bool Update(int id, string name, string category, decimal price, int quantity)
        {
            var part = GetById(id);
            if (part == null) return false;

            part.Name = name;
            part.Category = category;
            part.Price = price;
            part.Quantity = quantity;
            return true;
        }

        public bool Delete(int id)
        {
            var part = GetById(id);
            if (part == null) return false;

            _parts.Remove(part);
            return true;
        }

        public (int items, int totalQty, decimal totalValue, CarPart? mostExpensive) GetStats()
        {
            int items = _parts.Count;
            int totalQty = _parts.Sum(p => p.Quantity);
            decimal totalValue = _parts.Sum(p => p.StockValue);
            CarPart? mostExpensive = _parts.OrderByDescending(p => p.Price).FirstOrDefault();

            return (items, totalQty, totalValue, mostExpensive);
        }
    }
}
