using System.ComponentModel.DataAnnotations;
using static Guitaria.Data.Constants.ConstantValues.Product;
namespace Guitaria.Models.Product
{
    public class ProductViewModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        [Range(typeof(decimal), PriceMinValue, PriceMaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Required]
        public bool IsAvailable { get; set; }
    }
}
