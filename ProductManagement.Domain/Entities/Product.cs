using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Domain.Enums;

namespace ProductManagement.Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } 
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductCategory Category { get; set; }

        private static readonly Dictionary<ProductCategory, (decimal Min, decimal Max)> PriceRanges = new()
    {
        { ProductCategory.Electronics, (50, 50000) },
        { ProductCategory.Books, (5, 500) },
        { ProductCategory.Clothes, (10, 5000) }
    };

        public Product(string name, ProductCategory category, decimal price, int quantity)
        {
            SetName(name);
            SetCategory(category);
            SetPrice(price);
            SetQuantity(quantity);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 3 || name.Length > 20)
                throw new ArgumentException("Nazwa musi mieć od 3 do 20 znaków.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(name, "^[a-zA-Z0-9]+$"))
                throw new ArgumentException("Nazwa może zawierać tylko litery i cyfry.");

            Name = name;
        }

        public void SetCategory(ProductCategory category)
        {
            if (!PriceRanges.ContainsKey(category))
                throw new ArgumentException("Nieznana kategoria produktu.");

            Category = category;
        }

        public void SetPrice(decimal price)
        {
            if (!PriceRanges.TryGetValue(Category, out var range))
                throw new ArgumentException("Nieznana kategoria produktu.");

            if (price < range.Min || price > range.Max)
                throw new ArgumentException($"Cena dla {Category} musi wynosić od {range.Min} do {range.Max} PLN.");

            Price = price;
        }

        public void SetQuantity(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Ilość produktów nie może być ujemna.");

            Quantity = quantity;
        }
    }
}
