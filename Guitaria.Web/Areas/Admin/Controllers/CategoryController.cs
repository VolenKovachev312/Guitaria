using Guitaria.Contracts;
using Guitaria.Core.Models.Category;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Guitaria.Web.Areas.Admin.Controllers
{
    public class CategoryController : AdminController
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService, RoleManager<IdentityRole<Guid>> _roleManager) : base(_roleManager)
        {
            this.categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> AddCategory()
        {
            CreateCategoryViewModel model = new CreateCategoryViewModel()
            {
                Categories = await categoryService.LoadCategoriesAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CreateCategoryViewModel model)
        {
            model.Categories = await categoryService.LoadCategoriesAsync();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await categoryService.AddCategoryAsync(model);
            }
            catch (ArgumentException ae)
            {
                TempData["Error"] = ae.Message;
                return View(model);
            }
            model.Categories = await categoryService.LoadCategoriesAsync();

            return RedirectToAction("All", "Category", new { area = "" });
        }
        [HttpGet]
        public async Task<IActionResult> RemoveCategory()
        {
            RemoveCategoryViewModel model = new RemoveCategoryViewModel()
            {
                Categories = await categoryService.LoadCategoriesAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCategory(RemoveCategoryViewModel? model)
        {
            model.Categories = await categoryService.LoadCategoriesAsync();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await categoryService.RemoveCategoryAsync(model);
            }
            catch (ArgumentException ae)
            {
                TempData["Error"] = ae.Message;
                return View(model);
            }
            model.Categories = await categoryService.LoadCategoriesAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string categoryName)
        {
            CategoryViewModel model = new CategoryViewModel();
            try
            {
                model = await categoryService.GetCategoryAsync(categoryName);

            }
            catch (ArgumentException ae)
            {
                TempData["Error"] = ae.Message;
                return RedirectToAction("All","Category", new { area = "" });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel model, string categoryName)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await categoryService.EditCategoryAsync(model, categoryName);

            }
            catch (ArgumentException ae)
            {
                TempData["Error"] = ae.Message;
                return View(model);
            }
            return RedirectToAction("All", "Category", new { area = "" });
        }
    }
}
