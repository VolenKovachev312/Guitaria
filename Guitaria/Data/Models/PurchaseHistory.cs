using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Guitaria.Data.Models
{
    public class PurchaseHistory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        public ICollection<Order> PurchasedProducts { get; set; }
        = new List<Order>();

    }
}
