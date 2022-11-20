using System.ComponentModel.DataAnnotations;

namespace Guitaria.Models.Product
{
    public class RemoveProductViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
