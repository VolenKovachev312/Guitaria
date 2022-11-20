using Guitaria.Models.CategoryFolder;

namespace Guitaria.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllAsync();
    }
}
