using ProductManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Application.DTOs
{
    public class ProductDto
    {
        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [MinLength(3, ErrorMessage = "Nazwa musi mieć co najmniej 3 znaki.")]
        [MaxLength(20, ErrorMessage = "Nazwa może mieć maksymalnie 20 znaków.")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Nazwa może zawierać tylko litery i cyfry.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Kategoria jest wymagana.")]
        [EnumDataType(typeof(ProductCategory), ErrorMessage = "Nieprawidłowa kategoria.")]
        public ProductCategory Category { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Ilość musi być liczbą całkowitą i wynosić co najmniej 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Cena jest wymagana.")]
        public decimal Price { get; set; }
    }

}
