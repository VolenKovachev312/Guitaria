using Guitaria.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Guitaria.Models.Product
{
    public class CreateCategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }
        public IEnumerable<Guitaria.Data.Models.Category> Categories { get; set; } = new List<Category>();
    }
}
