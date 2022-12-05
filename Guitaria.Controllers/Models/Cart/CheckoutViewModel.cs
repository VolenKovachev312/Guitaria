using System.ComponentModel.DataAnnotations;

namespace Guitaria.Models.Cart
{
    public class CheckoutViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string NameOnCard { get; set; }

        [Required]
        public string CreditCardNumber { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string ExpMonth { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }

        [Required]
        public string ExpYear { get; set; }

        [Required]
        public string CVV { get; set; }

        public ICollection<Data.Models.Product> Products { get; set; } = new List<Data.Models.Product>();
    }
}
