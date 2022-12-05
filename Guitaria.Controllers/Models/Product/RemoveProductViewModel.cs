using System.ComponentModel.DataAnnotations;

namespace Guitaria.Models.Product
{
    public class RemoveProductViewModel
    {
        [Required]
        public string Name { get; set; }

        public IEnumerable<Data.Models.Product> Products { get; set; } = new List<Data.Models.Product>();
    }
}
