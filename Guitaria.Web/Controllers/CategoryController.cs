using Guitaria.Contracts;
using Guitaria.Data.Models;
using Guitaria.Core.Models.Category;
using Guitaria.Core.Models.Product;
using Guitaria.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Guitaria.Web.Controllers
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
        
    }
}
