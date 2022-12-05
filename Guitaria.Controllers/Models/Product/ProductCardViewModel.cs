using Guitaria.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Guitaria.Models.Product
{
    public class ProductCardViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        IEnumerable<Data.Models.Category> Categories { get; set; }
    }
}
