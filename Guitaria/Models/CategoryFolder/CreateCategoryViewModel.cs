using Guitaria.Data.Models;
using System.ComponentModel.DataAnnotations;
using static Guitaria.Data.Constants.ConstantValues.Category;

namespace Guitaria.Models.CategoryFolder
{
    public class CreateCategoryViewModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
