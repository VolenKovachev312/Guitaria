using Guitaria.Controllers;
using Guitaria.Data.Models;
using Guitaria.Models.Product;
using Guitaria.Services;
using Guitaria.Test.Mocks;
using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Guitaria.Test.Service
{
    public class ProductServiceTest
    {
        [Fact]
        public async Task ServiceShouldLoadCategories()
        {
            //Arrange
            using var data = DatabaseMock.Instance;

            var categories = new List<Category>()
            {
                new Category()
                {
                    Name="Test1",
                    ImageUrl="test"
                },
               new Category()
                {
                    Name="Test2",
                    ImageUrl="test"
                },new Category()
                {
                    Name="Test3",
                    ImageUrl="test"
                }
            };
            data.Categories.AddRange(categories);
            data.SaveChanges();

            var productService = new ProductService(data);

            //Act
           var result = await productService.LoadCategoriesAsync();


            //Assert

            Assert.Equal(3, result.Count());
        }
        [Fact]
        public async Task EditShouldChangeProperties()
        {
            //Arrange
            var data = DatabaseMock.Instance;
            var productService = new ProductService(data);
            ProductViewModel viewModel = new ProductViewModel()
            {
                IsAvailable = true,
                Description="changedDescription",
                Name="ChangedGuitar",
                Price=35000,
                ImageUrl="changedUrl"
            };
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Name="GuitarTest",
                    Description="testDescription",
                    IsAvailable=true,
                    TimeAdded=DateTime.Now,
                    ImageUrl="testUrl",
                    Price=300
                },
                new Product()
                {
                    Name="GuitarTest2",
                    Description="testDescription2",
                    IsAvailable=true,
                    TimeAdded=DateTime.Now,
                    ImageUrl="testUrl2",
                    Price=3001
                }
            };
            data.Products.AddRange(products);
            data.SaveChanges();

            //Act
            await productService.EditProductAsync(viewModel, "GuitarTest");

            //Assert

            Assert.True(data.Products.Where(p=>p.Name== "ChangedGuitar").Any());
        }
    }
}
