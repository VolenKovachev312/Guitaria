using Guitaria.Contracts;
using Guitaria.Data.Models;
using Guitaria.Models.Category;
using Guitaria.Models.Product;
using Guitaria.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Guitaria.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService _categoryService)
        {
            this.categoryService = _categoryService;
        }

        public async Task<IActionResult> All()
        {
            var model = await categoryService.GetAllAsync();

            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddCategory()
        {
            CreateCategoryViewModel model = new CreateCategoryViewModel()
            {
                Categories = await categoryService.LoadCategoriesAsync()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
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
                return View(model);
            }
            model.Categories = await categoryService.LoadCategoriesAsync();

            return RedirectToAction("All", "Category");
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemoveCategory()
        {
            RemoveCategoryViewModel model = new RemoveCategoryViewModel()
            {
                Categories = await categoryService.LoadCategoriesAsync()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
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
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
            model.Categories = await categoryService.LoadCategoriesAsync();

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> Edit(string categoryName)
        {
            CategoryViewModel model = new CategoryViewModel();
            try
            {
                model = await categoryService.GetCategoryAsync(categoryName);

            }
            catch (Exception)
            {
                return RedirectToAction("All");
            } 

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(CategoryViewModel model, string categoryName)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            await categoryService.EditCategoryAsync(model,categoryName);
            return RedirectToAction("All", "Category");
        }
    }
}
