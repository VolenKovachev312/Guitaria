using Guitaria.Data;
using Guitaria.Data.Models;
using Guitaria.Core.Models.Product;
using Guitaria.Contracts;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Guitaria.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;

        public ProductService(ApplicationDbContext _context)
        {
            context = _context;
            
        }

        public async Task AddProductToCartAsync(string userId, string productName)
        {

            var user = await context.Users.Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            var product = await context.Products.FirstOrDefaultAsync(p => p.Name == productName);
            var userProducts = user.ShoppingCart.ShoppingCartProducts.Select(p => p.Product).ToList();
            if (userProducts.Contains(product))
            {
                throw new ArgumentException("Item is already in shopping cart!");
            }
            else
            {
                user.ShoppingCart.ShoppingCartProducts.Add(new ShoppingCartProduct()
                {
                    ShoppingCartId = user.ShoppingCart.Id,
                    ProductId = product.Id
                });
            }
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Category>> LoadCategoriesAsync()
        {
            return await context.Categories.ToListAsync();
        }
        public async Task AddProductAsync(CreateProductViewModel model)
        {
            Product product = new Product()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Price = model.Price,
                ImageUrl = model.ImageUrl
            };
            if (context.Products.Where(p=>p.Name==model.Name).Any())
            {
                throw new ArgumentException("Product with this name already exists!");
            }
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }
        public async Task UnlistProductAsync(UnlistProductViewModel model)
        {

            Product? product = await context.Products.FirstOrDefaultAsync(c => c.Name == model.Name);
            if (product == null)
            {
                throw new ArgumentException("Product does not exist.");
            }
            if(product.IsAvailable==false)
            {
                throw new ArgumentException("Product is already unlisted.");
            }
            product.IsAvailable = false;
            await context.SaveChangesAsync();
        }
        public async Task<AllProductsViewModel> GetAllAsync(string categoryName, string searchQuery, int currentPage)
        {
            IEnumerable<Product> entities;
            bool isSearchQuery = false;
            if(currentPage==0)
            {
                currentPage = 1;
            }
            var model = new AllProductsViewModel()
            {
                CurrentPage = currentPage,
                SearchQuery = searchQuery

            };
            if (categoryName != null &&
                context.Categories.FirstOrDefault(c => c.Name == categoryName) != null)
            {
                entities = await context.Products.Include(p => p.Category).Where(p => p.Category.Name == categoryName&&p.IsAvailable).ToListAsync();
            }
            else 
            {
                entities = await context.Products.Include(p => p.Category).Where(p => p.Name.Contains(searchQuery)&&p.IsAvailable).ToListAsync();
                isSearchQuery = true;
            }
            model.NumberOfPages = entities.Count() / AllProductsViewModel.ProductsPerPage;
            if (entities.Count() % AllProductsViewModel.ProductsPerPage != 0)
            {
                model.NumberOfPages++;
            }
            if (currentPage > model.NumberOfPages)
            {
                model.CurrentPage = model.NumberOfPages;
            }
            if (!isSearchQuery)
            {
                model.Products = entities
                .Skip((model.CurrentPage - 1) * AllProductsViewModel.ProductsPerPage)
                .Take(AllProductsViewModel.ProductsPerPage)
                .Select(e => new ProductCardViewModel
                {
                    Name = e.Name,
                    ImageUrl = e.ImageUrl,
                    Price = e.Price,
                    isAvailable = e.IsAvailable,
                    CategoryName = e.Category.Name
                });
            }
            else
            {
                model.Products = entities
                .Skip((model.CurrentPage - 1) * AllProductsViewModel.ProductsPerPage)
                .Take(AllProductsViewModel.ProductsPerPage)
                .Select(e => new ProductCardViewModel
                {
                    Name = e.Name,
                    ImageUrl = e.ImageUrl,
                    Price = e.Price,
                    isAvailable = e.IsAvailable,
                    CategoryName = String.Empty
                });
            }

            return model;
        }
        public async Task<ProductViewModel> GetProductAsync(string productName)
        {

            Product? tempProduct = await context.Products.FirstOrDefaultAsync(c => c.Name == productName);
            if (tempProduct == null)
            {
                throw new ArgumentException("Product does not exist!");
            }
            ProductViewModel model = new ProductViewModel()
            {
                Name = tempProduct.Name,
                ImageUrl = tempProduct.ImageUrl,
                Description = tempProduct.Description,
                Price = tempProduct.Price,
                IsAvailable=tempProduct.IsAvailable
            };
            return model;
        }
        public async Task EditProductAsync(ProductViewModel model, string productName)
        {
            var product = context.Products.FirstOrDefault(c => c.Name == productName);
            var checkProduct = context.Products.FirstOrDefault(p => p.Name == model.Name);
            if (checkProduct!=null&&product!=checkProduct)
            {
                throw new ArgumentException("Product with this name already exists.");
            }
            product.Name = model.Name;
            product.ImageUrl = model.ImageUrl;
            product.Description = model.Description;
            product.Price = model.Price;
            product.IsAvailable = model.IsAvailable;
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Product>> LoadProductsAsync()
        {
            return await context.Products.Include(p => p.Category).ToListAsync();
        }
        public async Task<IEnumerable<Product>> LoadCarouselAsync()
        {
            return await context.Products.OrderByDescending(p => p.Price).Where(p=>p.IsAvailable).Take(4).ToListAsync();
        }
        public async Task<IEnumerable<Product>> LoadLatestAsync()
        {
            return await context.Products.OrderByDescending(p => p.TimeAdded).Where(p => p.IsAvailable).Take(4).ToListAsync();
        }
    }
}
