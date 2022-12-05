using System.ComponentModel.DataAnnotations;
using static Guitaria.Data.Constants.ConstantValues.Category;

namespace Guitaria.Models.Category
{
    public class CreateCategoryViewModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }
        public IEnumerable<Data.Models.Category> Categories { get; set; } = new List<Data.Models.Category>();
    }
}
