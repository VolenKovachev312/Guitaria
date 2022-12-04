using System.ComponentModel.DataAnnotations.Schema;

namespace Guitaria.Data.Models
{
    public class ShoppingCartProduct
    {
        [ForeignKey(nameof(ShoppingCart))]
        public Guid ShoppingCartId { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
