using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guitaria.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        public virtual PurchaseHistory PurchaseHistory { get; set; } = new PurchaseHistory();

        public virtual ShoppingCart ShoppingCart { get; set; } = new ShoppingCart();
        

    }
}
