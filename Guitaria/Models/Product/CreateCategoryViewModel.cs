using Guitaria.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Guitaria.Models.Product
{
    public class CreateCategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
