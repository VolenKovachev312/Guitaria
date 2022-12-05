using System.ComponentModel.DataAnnotations.Schema;

namespace Guitaria.Data.Models
{
    public class OrderProduct
    {
        [ForeignKey(nameof(Order))]
        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
