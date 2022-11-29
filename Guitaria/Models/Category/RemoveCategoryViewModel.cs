using System.ComponentModel.DataAnnotations;

namespace Guitaria.Models.Category
{
    public class RemoveCategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        public IEnumerable<Data.Models.Category> Categories { get; set; } = new List<Data.Models.Category>();
    }
}
