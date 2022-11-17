using Guitaria.Contracts;
using Guitaria.Data;
using Guitaria.Data.Models;
using Guitaria.Models.Product;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Guitaria.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        
        public ProductService(ApplicationDbContext _context, IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            context = _context;
            _httpContextAccessor = httpContextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
        }
        

        public async Task AddCategoryAsync(CreateCategoryViewModel model)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);
            var entity = new Category()
            {
                Id = Guid.NewGuid(),
                Name= model.Name
            };
            if(context.Categories.Any(c=>c.Name==model.Name))
            {
                tempData["Error"] = "Category already exists.";
                return;
            }
            await context.Categories.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> LoadCategoriesAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task RemoveCategoryAsync(CreateCategoryViewModel model)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);

            Category? tempCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == model.Name);
            if(tempCategory == null)
            {
                tempData["Error"] = "Category does not exist.";
                return;
            }
            if(tempCategory.Products.Any())
            {
                tempData["Error"] = "There are products in this category.";
                return;
            }
            context.Categories.Remove(tempCategory);
            await context.SaveChangesAsync();
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
