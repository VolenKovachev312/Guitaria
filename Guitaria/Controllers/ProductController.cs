﻿using Guitaria.Contracts;
using Guitaria.Models.CategoryFolder;
using Guitaria.Models.Product;
using Guitaria.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Guitaria.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }

        public async Task<IActionResult> All(string categoryName)
        {
            var models = await productService.GetAllAsync(categoryName);
            return View(models);
        }

        public async Task<IActionResult> ViewProduct(string productName)
        {
            ProductViewModel model = await productService.GetProductAsync(productName);

            return View(model);
        }

        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddProduct()
        {
            CreateProductViewModel model = new CreateProductViewModel()
            {
                Categories = await productService.LoadCategoriesAsync()
            };
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddProduct(CreateProductViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await productService.AddProductAsync(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
            return RedirectToAction("All", "Product");
        }
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemoveProduct()
        {
            RemoveProductViewModel model = new RemoveProductViewModel()
            {
                Products = await productService.LoadProductsAsync()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemoveProduct(RemoveProductViewModel model)
        {
            model.Products = await productService.LoadProductsAsync();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await productService.RemoveProductAsync(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
            model.Products = await productService.LoadProductsAsync();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(string productName)
        {
            var model = await productService.GetProductAsync(productName);
            if (model == null)
            {
                ModelState.AddModelError("", "Category doesn't exist");
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(ProductViewModel model, string productName)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await productService.EditProductAsync(model, productName);
            return RedirectToAction("All", "Product");
        }
       

    }
}
