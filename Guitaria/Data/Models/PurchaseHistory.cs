using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Guitaria.Data.Models
{
    public class PurchaseHistory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public IEnumerable<ShoppingCart> PurchasedProducts { get; set; }
            = new List<ShoppingCart>();

    }
}
