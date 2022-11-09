using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Guitaria.Data.Constants.ConstantValues.ProductType;

namespace Guitaria.Data.Models
{
    public class ProductType
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }
        = new List<Product>();

    }
}
