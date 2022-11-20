using Guitaria.Contracts;
using Guitaria.Data;
using Guitaria.Models.CategoryFolder;
using Microsoft.EntityFrameworkCore;

namespace Guitaria.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext context;
        public CategoryService(ApplicationDbContext _context)
        {
            this.context = _context;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllAsync()
        {
            var entities=await context.Categories.ToListAsync();

            return entities.Select(e => new CategoryViewModel
            {
                Name = e.Name,
                ImageUrl = e.ImageUrl,
            });
        }
    }
}
