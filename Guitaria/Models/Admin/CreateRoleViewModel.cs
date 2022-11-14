using System.ComponentModel.DataAnnotations;

namespace Guitaria.Models.Admin
{
    public class CreateRoleViewModel
    {
        [Required]
        public Guid Id { get; set; }= Guid.NewGuid();
        [Required]
        public string RoleName { get; set; }
    }
}
