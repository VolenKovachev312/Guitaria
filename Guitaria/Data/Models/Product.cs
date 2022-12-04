using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Guitaria.Data.Constants.ConstantValues.Product;

namespace Guitaria.Data.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [Range(typeof(decimal),PriceMinValue,PriceMaxValue)]
        public decimal Price { get; set; }

        [NotMapped]
        [Range(MinQuantity,MaxQuantity)]
        public int Quantity { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

    }
}
