using System.ComponentModel.DataAnnotations;

namespace Guitaria.Models.Cart
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = " ")]
        public string FullName { get; set; }

        [Required(ErrorMessage = " ")]
        public string Address { get; set; }

        [Required(ErrorMessage = " ")]
        public string Email { get; set; }

        [Required(ErrorMessage = " ")]
        public string NameOnCard { get; set; }

        [Required(ErrorMessage = " ")]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessage = " ")]
        public string City { get; set; }

        [Required(ErrorMessage = " ")]
        public string ExpMonth { get; set; }

        [Required(ErrorMessage = " ")]
        public string State { get; set; }

        [Required(ErrorMessage = " ")]
        public string Zip { get; set; }

        [Required(ErrorMessage = " ")]
        public string ExpYear { get; set; }

        [Required(ErrorMessage =" ")]
        public string CVV { get; set; }

        public ICollection<Data.Models.Product> Products { get; set; } = new List<Data.Models.Product>();
    }
}
