using Guitaria.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Guitaria.Models.CategoryFolder
{
    public class RemoveCategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
