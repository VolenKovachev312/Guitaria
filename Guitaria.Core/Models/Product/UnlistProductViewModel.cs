using System.ComponentModel.DataAnnotations;

namespace Guitaria.Core.Models.Product
{
    public class UnlistProductViewModel
    {
        [Required]
        public string Name { get; set; }

        public IEnumerable<Data.Models.Product> Products { get; set; } = new List<Data.Models.Product>();
    }
}
