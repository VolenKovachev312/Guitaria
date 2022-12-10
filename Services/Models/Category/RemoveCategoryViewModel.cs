using System.ComponentModel.DataAnnotations;

namespace Guitaria.Core.Models.Category
{
    public class RemoveCategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        public IEnumerable<Data.Models.Category> Categories { get; set; } = new List<Data.Models.Category>();
    }
}
