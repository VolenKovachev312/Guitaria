﻿using Guitaria.Data.Models;
using Guitaria.Core.Models.Category;
using Guitaria.Core.Models.Product;
namespace Guitaria.Contracts
{
    public interface IProductService
    {
        Task<AllProductsViewModel> GetAllAsync(string categoryName,string searchQuery,int currentPage);

        Task AddProductAsync(CreateProductViewModel model);

        Task UnlistProductAsync(UnlistProductViewModel model);

        Task<IEnumerable<Category>> LoadCategoriesAsync();

        Task<ProductViewModel> GetProductAsync(string productName);

        Task EditProductAsync(ProductViewModel model, string productName);
        Task<IEnumerable<Product>> LoadProductsAsync();

        Task AddProductToCartAsync(string userId, string productName);

        Task<IEnumerable<Product>> LoadCarouselAsync();

        Task<IEnumerable<Product>> LoadLatestAsync();
    }
}
