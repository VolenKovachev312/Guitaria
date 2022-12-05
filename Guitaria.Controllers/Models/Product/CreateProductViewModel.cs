using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Guitaria.Data.Constants.ConstantValues.Product;
using Guitaria.Data.Models;

namespace Guitaria.Models.Product
{
    public class CreateProductViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(NameMaxLength, MinimumLength =NameMinLength)]
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
        public Guid CategoryId { get; set; }

        public IEnumerable<Data.Models.Category> Categories = new List<Data.Models.Category>();

    }
}
