using System.ComponentModel.DataAnnotations;
using static Guitaria.Data.Constants.ConstantValues.Category;
namespace Guitaria.Models.CategoryFolder
{
    public class CategoryViewModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength =NameMinLength)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
