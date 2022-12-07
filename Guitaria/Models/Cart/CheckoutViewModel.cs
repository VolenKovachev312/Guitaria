using System.ComponentModel.DataAnnotations;

namespace Guitaria.Models.Cart
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage =" ")]
        [StringLength(100, MinimumLength = 10,ErrorMessage ="Invalid Name.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = " ")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Invalid Address.")]
        public string Address { get; set; }

        [Required(ErrorMessage = " ")]
        [EmailAddress(ErrorMessage = "Invalid Email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = " ")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Invalid Name.")]
        public string NameOnCard { get; set; }

        [Required(ErrorMessage = " ")]
        [StringLength(12,MinimumLength =12, ErrorMessage = "Invalid Credit Card Number.")]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessage = " ")]
        [StringLength (100,MinimumLength =3, ErrorMessage = "Invalid City Name.")]
        public string City { get; set; }

        [Required(ErrorMessage = " ")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Invalid Month.")]
        public string ExpMonth { get; set; }

        [Required(ErrorMessage = " ")]
        [StringLength(5, MinimumLength = 3, ErrorMessage = "Invalid Zip.")]
        public string Zip { get; set; }

        [Required(ErrorMessage = " ")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Invalid Year.")]
        public string ExpYear { get; set; }

        [Required(ErrorMessage =" ")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Invalid CVV.")]
        public string CVV { get; set; }

        public ICollection<Data.Models.Product> Products { get; set; } = new List<Data.Models.Product>();
    }
}
