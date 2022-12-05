using Guitaria.Data;
using Guitaria.Data.Models;
using Guitaria.Models.Product;
using Guitaria.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Guitaria.Services.Services
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
                }) ;
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
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProductCardViewModel>> GetAllAsync(string categoryName)
        {
            IEnumerable<Product> entities;
            if(categoryName!=null &&
                context.Categories.FirstOrDefault(c => c.Name == categoryName) != null)
            {
                 entities = await context.Products.Include(p => p.Category).Where(p => p.Category.Name == categoryName).ToListAsync();
            }
            else
            {
                 entities = await context.Products.ToListAsync();
            }

            return entities.Select(e => new ProductCardViewModel
            {
                Name=e.Name,
                ImageUrl=e.ImageUrl,
                Price=e.Price
            });
        }

        public async Task<ProductViewModel> GetProductAsync(string productName)
        {
            
            Product? tempProduct = await context.Products.FirstOrDefaultAsync(c => c.Name == productName);
            if (tempProduct == null)
            {
                //REDIRECT TO PAGE DOES NOT EXIST
            }
            ProductViewModel model = new ProductViewModel()
            {
                Name = tempProduct.Name,
                ImageUrl = tempProduct.ImageUrl,
                Description=tempProduct.Description,
                Price=tempProduct.Price
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
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> LoadProductsAsync()
        {
            return await context.Products.Include(p=>p.Category).ToListAsync();
        }

        





        //public async Task AddMovieToCollectionAsync(int movieId, string userId)
        //{
        //    var user = await context.Users
        //        .Where(u => u.Id == userId)
        //        .Include(u => u.UsersMovies)
        //        .FirstOrDefaultAsync();

        //    if (user == null)
        //    {
        //        throw new ArgumentException("Invalid user ID");
        //    }

        //    var movie = await context.Movies.FirstOrDefaultAsync(u => u.Id == movieId);

        //    if (movie == null)
        //    {
        //        throw new ArgumentException("Invalid Movie ID");
        //    }

        //    if (!user.UsersMovies.Any(m => m.MovieId == movieId))
        //    {
        //        user.UsersMovies.Add(new UserMovie()
        //        {
        //            MovieId = movie.Id,
        //            UserId = user.Id,
        //            Movie = movie,
        //            User = user
        //        });

        //        await context.SaveChangesAsync();
        //    }
        //}

        //public async Task<IEnumerable<MovieViewModel>> GetAllAsync()
        //{
        //    var entities = await context.Movies
        //        .Include(m => m.Genre)
        //        .ToListAsync();

        //    return entities
        //        .Select(m => new MovieViewModel()
        //        {
        //            Director = m.Director,
        //            Genre = m?.Genre?.Name,
        //            Id = m.Id,
        //            ImageUrl = m.ImageUrl,
        //            Rating = m.Rating,
        //            Title = m.Title
        //        });
        //}

        //public async Task<IEnumerable<Genre>> GetGenresAsync()
        //{
        //    return await context.Genres.ToListAsync();
        //}

        //public async Task<IEnumerable<MovieViewModel>> GetWatchedAsync(string userId)
        //{
        //    var user = await context.Users
        //        .Where(u => u.Id == userId)
        //        .Include(u => u.UsersMovies)
        //        .ThenInclude(um => um.Movie)
        //        .ThenInclude(m => m.Genre)
        //        .FirstOrDefaultAsync();

        //    if (user == null)
        //    {
        //        throw new ArgumentException("Invalid user ID");
        //    }

        //    return user.UsersMovies
        //        .Select(m => new MovieViewModel()
        //        {
        //            Director = m.Movie.Director,
        //            Genre = m.Movie.Genre?.Name,
        //            Id = m.MovieId,
        //            ImageUrl = m.Movie.ImageUrl,
        //            Title = m.Movie.Title,
        //            Rating = m.Movie.Rating,
        //        });
        //}

        //public async Task RemoveMovieFromCollectionAsync(int movieId, string userId)
        //{
        //    var user = await context.Users
        //        .Where(u => u.Id == userId)
        //        .Include(u => u.UsersMovies)
        //        .FirstOrDefaultAsync();

        //    if (user == null)
        //    {
        //        throw new ArgumentException("Invalid user ID");
        //    }

        //    var movie = user.UsersMovies.FirstOrDefault(m => m.MovieId == movieId);

        //    if (movie != null)
        //    {
        //        user.UsersMovies.Remove(movie);

        //        await context.SaveChangesAsync();
        //    }

        //}
    }
}
