using Guitaria.Data;
using Guitaria.Data.Models;
using Guitaria.Models.Product;
using Guitaria.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Guitaria.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        private HttpContext? httpContext;
        private ITempDataDictionary tempData;

        public ProductService(ApplicationDbContext _context, IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            context = _context;
            _httpContextAccessor = httpContextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            httpContext = _httpContextAccessor.HttpContext;
            tempData = _tempDataDictionaryFactory.GetTempData(httpContext);
        }
        public async Task AddProductToCartAsync(string userId, string productName)
        {

            var user = await context.Users.Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
            {
                tempData["ViewProductError"] = "User not logged in.";
                return;
            }
            var product = await context.Products.FirstOrDefaultAsync(p => p.Name == productName);
            if (product == null)
            {
                tempData["ViewProductError"] = "Invalid product.";
                return;
            }
            if (!product.IsAvailable)
            {
                tempData["ViewProductError"] = "Product is unavailable at the moment.";
                return;
            }
            var userProducts = user.ShoppingCart.ShoppingCartProducts.Select(p => p.Product).ToList();
            if (userProducts.Contains(product))
            {
                tempData["ViewProductError"] = "Item is already in cart.";
            }
            else
            {
                tempData["ViewProductSuccess"] = "Successfully added product to shopping cart!";
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
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }
        public async Task RemoveProductAsync(RemoveProductViewModel model)
        {

            Product? product = await context.Products.FirstOrDefaultAsync(c => c.Name == model.Name);
            if (product == null)
            {
                tempData["Error"] = "Product does not exist.";
                return;
            }
            product.IsAvailable = false;
            await context.SaveChangesAsync();
        }
        public async Task<AllProductsViewModel> GetAllAsync(string categoryName, string searchQuery, int currentPage)
        {
            IEnumerable<Product> entities;
            bool isSearchQuery = false;
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
            else //if(searchQuery!=null)
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
                tempData["Error"] = "Product does not exist!";
                throw new Exception();
            }
            ProductViewModel model = new ProductViewModel()
            {
                Name = tempProduct.Name,
                ImageUrl = tempProduct.ImageUrl,
                Description = tempProduct.Description,
                Price = tempProduct.Price
            };
            return model;
        }

        public async Task EditProductAsync(ProductViewModel model, string productName)
        {
            var product = context.Products.FirstOrDefault(c => c.Name == productName);
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
            return await context.Products.OrderByDescending(p => p.Price).Take(4).ToListAsync();
        }

        public async Task<IEnumerable<Product>> LoadLatestAsync()
        {
            return await context.Products.OrderByDescending(p => p.TimeAdded).Take(4).ToListAsync();
        }
    }
}
