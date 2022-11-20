using System.ComponentModel.DataAnnotations;

namespace Guitaria.Models.CategoryFolder
{
    public class CategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
