using Guitaria.Data;
using Guitaria.Data.Models;
using Guitaria.Models.Category;
using Guitaria.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Guitaria.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        private HttpContext? httpContext;
        private ITempDataDictionary tempData;
        public CategoryService(ApplicationDbContext _context, IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            context = _context;
            _httpContextAccessor = httpContextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            httpContext = _httpContextAccessor.HttpContext;
            tempData = _tempDataDictionaryFactory.GetTempData(httpContext);
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
        public async Task AddCategoryAsync(CreateCategoryViewModel model)
        {
            var entity = new Category()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                ImageUrl = model.ImageUrl
            };
            if (context.Categories.Any(c => c.Name == model.Name))
            {
                tempData["Error"] = "Category already exists.";
                return;
            }
            await context.Categories.AddAsync(entity);
            await context.SaveChangesAsync();
        }
        public async Task RemoveCategoryAsync(RemoveCategoryViewModel model)
        {

            Category? tempCategory = await context.Categories.Include(c=>c.Products).FirstOrDefaultAsync(c => c.Name == model.Name);
            if (tempCategory == null)
            {
                tempData["Error"] = "Category does not exist.";
                return;
            }
            if (tempCategory.Products.Any())
            {
                tempData["Error"] = "There are products in this category.";
                return;
            }
            context.Categories.Remove(tempCategory);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Category>> LoadCategoriesAsync()
        {
            return await context.Categories.Include(c=>c.Products).ToListAsync();
        }

        public async Task<CategoryViewModel> GetCategoryAsync(string categoryName)
        {
            Category? tempCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
            if (tempCategory == null)
            {
                tempData["Error"] = "Category does not exist.";
            }
            CategoryViewModel model = new  CategoryViewModel()
            {
                Name = tempCategory.Name,
                ImageUrl = tempCategory.ImageUrl
            };
            return model;
        }

        public async Task EditCategoryAsync(CategoryViewModel model, string categoryName)
        {
            var category = context.Categories.FirstOrDefault(c => c.Name == categoryName);
            category.Name = model.Name;
            category.ImageUrl = model.ImageUrl;
            await context.SaveChangesAsync();
        }
    }
}
