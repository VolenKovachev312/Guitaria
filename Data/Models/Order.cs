using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guitaria.Data.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(PurchaseHistory))]
        public Guid PurchaseHistoryId { get; set; }

        public virtual PurchaseHistory PurchaseHistory { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        =new List<OrderProduct>();

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal FinalPrice { get; set; }
    }
}
