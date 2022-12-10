using Guitaria.Core.Models.Category;
using Guitaria.Data;
using Guitaria.Data.Models;
using Guitaria.Core.Services;
using Guitaria.Test.Mocks;
using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Guitaria.Test.Service
{
    public class CategoryServiceTest
    {
        private ApplicationDbContext data = DatabaseMock.Instance;
        private CategoryService categoryService;

        public CategoryServiceTest()
        {
            categoryService = new CategoryService(data);
            SeedDatabase();
        }
        
        [Fact]
        public async Task ServiceShouldGetAll()
        {
            //Act
            var categories = await categoryService.GetAllAsync();

            //Assert
            Assert.Equal(5, categories.Count());
        }

        [Fact]
        public async Task ServiceShouldAddCategory()
        {
            //Arrange
            var viewModel = new CreateCategoryViewModel()
            {
                Name = "Electro-Acoustic Guitars",
                ImageUrl = "testUrl"
            };

            //Act
            Assert.Equal(5, data.Categories.Count());
            await categoryService.AddCategoryAsync(viewModel);

            //Assert
            Assert.Equal(6, data.Categories.Count());

        }
        [Fact]
        public async Task AddCategoryShouldThrowIfNameExists()
        {
            //Arrange
            var viewModel = new CreateCategoryViewModel()
            {
                Name = "Acoustic Guitars",
                ImageUrl = "testUrl"
            };

            //Act
            var result=await Assert.ThrowsAsync<ArgumentException>(async ()=>await categoryService.AddCategoryAsync(viewModel));

            //Assert
            Assert.Equal("Category already exists.", result.Message);
        }

        [Fact]
        public async Task ServiceShouldRemoveCategory()
        {
            //Arrange
            var viewModel = new RemoveCategoryViewModel()
            {
                Name = "Acoustic Guitars"
            };

            //Act
            Assert.Equal(5, data.Categories.Count());
            await categoryService.RemoveCategoryAsync(viewModel);

            //Assert
            Assert.Equal(4, data.Categories.Count());
        }
        [Fact]
        public async Task RemoveCategoryShouldThrowIfCategoryNotExist()
        {
            //Arrange
            var viewModel = new RemoveCategoryViewModel()
            {
                Name = "Acoustic GuitarsError",
            };

            //Act
            var result = await Assert.ThrowsAsync<ArgumentException>(async () => await categoryService.RemoveCategoryAsync(viewModel));

            //Assert
            Assert.Equal("Category does not exist.", result.Message);
        }
        [Fact]
        public async Task RemoveCategoryShouldThrowIfCategoryNotEmpty()
        {
            //Arrange
            var viewModel = new RemoveCategoryViewModel()
            {
                Name = "Acoustic Guitars",
            };
            data.Products.Add(new Product()
            {
                Name = "TestGuitar",
                Description = "TestDescriptionTest",
                IsAvailable = true,
                Category = data.Categories.FirstOrDefault(c => c.Name == "Acoustic Guitars"),
                ImageUrl = "TestUrl",
                Price = 360,
                TimeAdded = DateTime.Now
            });
            data.SaveChanges();
            //Act
            var result = await Assert.ThrowsAsync<ArgumentException>(async () => await categoryService.RemoveCategoryAsync(viewModel));

            //Assert
            Assert.Equal("There are products in this category.", result.Message);
        }

        [Fact]
        public async Task ServiceShouldReturnCategory()
        {
            //Arrange
            string categoryName = "Acoustic Guitars";

            //Act
            var result = await categoryService.GetCategoryAsync(categoryName);

            //Assert
            Assert.True(result != null);
            Assert.IsType<CategoryViewModel>(result);
        }
        [Fact]
        public async Task GetCategoryShouldThrowIfNotExist()
        {
            //Arrange
            string categoryName = "Acoustic GuitarsError";

            //Act
            var result = await Assert.ThrowsAsync<ArgumentException>(async () => await categoryService.GetCategoryAsync(categoryName));

            //Assert
            Assert.Equal("Category does not exist.", result.Message);
        }

        [Fact]
        public async Task ServiceShouldEditCategory()
        {
            //Arrange
            var viewModel = new CategoryViewModel()
            {
                Name = "Changed Name",
                ImageUrl="Changed URL"
            };
            var categoryName = "Acoustic Guitars";

            //Act
            Assert.True(data.Categories.Where(c => c.Name == "Acoustic Guitars").Any());
            await categoryService.EditCategoryAsync(viewModel, categoryName);

            //Assert
            Assert.False(data.Categories.Where(c => c.Name == "Acoustic Guitars").Any());
            Assert.True(data.Categories.Where(c => c.Name == "Changed Name").Any());
        }
        [Fact]
        public async Task EditShouldThrowIfNameExists()
        {
            //Assign
            var viewModel = new CategoryViewModel()
            {
                Name = "Electric Guitars",
                ImageUrl = "Changed URL"
            };
            var categoryName = "Acoustic Guitars";

            //Act
            var result = await Assert.ThrowsAsync<ArgumentException>(async () => await categoryService.EditCategoryAsync(viewModel, categoryName));

            //Assert
            Assert.Equal("Category with this name already exists.", result.Message);
        }
        
        private void SeedDatabase()
        {
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name="Acoustic Guitars",
                    ImageUrl="testURL"
                },
                new Category()
                {
                    Name="Electric Guitars",
                    ImageUrl="testURL"
                }
                ,new Category()
                {
                    Name="Bass Guitars",
                    ImageUrl="testURL"
                }
                ,new Category()
                {
                    Name="Amplifiers",
                    ImageUrl="testURL"
                }
                ,new Category()
                {
                    Name="Accessories",
                    ImageUrl="testURL"
                }
            };
            data.Categories.AddRange(categories);
            data.SaveChanges();
        }
    }
}
