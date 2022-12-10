using Guitaria.Data;
using Guitaria.Data.Models;
using Guitaria.Core.Models.Product;
using Guitaria.Core.Services;
using Guitaria.Test.Mocks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Guitaria.Test.Service
{
    public class ProductServiceTest
    {
        private ApplicationDbContext data = DatabaseMock.Instance;
        private ProductService productService;

        public ProductServiceTest()
        {
            productService = new ProductService(data);
            SeedDatabase();
        }

        //ADD TO CART
        [Fact]
        public async Task AddToCartShouldAddProduct()
        {
            var productService = new ProductService(data);
            var user = new User()
            {
                Email = "testemail@gmail.com",
                UserName = "testUserName",
                PasswordHash = "testHash"
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
            data.Users.Add(user);
            data.Products.AddRange(products);
            data.SaveChanges();
            var shoppingCart = data.ShoppingCarts.Include(sc => sc.ShoppingCartProducts).FirstOrDefault(sc => sc.UserId == user.Id);
            var productName = "GuitarTest";
            Assert.Equal(0, shoppingCart.ShoppingCartProducts.Count);
            await productService.AddProductToCartAsync(user.Id.ToString(), productName);


            Assert.Equal(1, shoppingCart.ShoppingCartProducts.Count);
        }

        //GET PRODUCT
        [Fact]
        public async Task GetProductShouldReturnCorrectViewModel()
        {
            //Arrange
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
            var comparisonProduct = products.FirstOrDefault(p => p.Name == "GuitarTest");
            //Act
            var product = await productService.GetProductAsync("GuitarTest");

            //Assert
            Assert.NotNull(product);
            Assert.Equal(product.Name, comparisonProduct.Name);
            Assert.Equal(product.Description, comparisonProduct.Description);
            Assert.IsType<ProductViewModel>(product);
        }
        [Fact]
        public async Task GetProductShouldThrowWhenProductNotExist()
        {
            //Act
            var result = await Assert.ThrowsAsync<ArgumentException>(async () => await productService.GetProductAsync("GuitarTestError"));

            //Assert
            Assert.Equal("Product does not exist!", result.Message);
        }

        //EDIT
        [Fact]
        public async Task EditShouldChangeProperties()
        {
            //Arrange
            ProductViewModel viewModel = new ProductViewModel()
            {
                IsAvailable = true,
                Description = "changedDescription",
                Name = "ChangedGuitar",
                Price = 35000,
                ImageUrl = "changedUrl"
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

            Assert.True(data.Products.Where(p => p.Name == "ChangedGuitar").Any());
        }
        [Fact]
        public async Task EditNameShouldThrowWhenNameExists()
        {
            //Arrange
            ProductViewModel viewModel = new ProductViewModel()
            {
                IsAvailable = true,
                Description = "changedDescription",
                Name = "GuitarTest2",
                Price = 35000,
                ImageUrl = "changedUrl"
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
            var result = await Assert.ThrowsAsync<ArgumentException>(async () => await productService.EditProductAsync(viewModel, "GuitarTest"));

            //Assert

            Assert.Equal("Product with this name already exists.", result.Message);
        }

        //LOADING
        [Fact]
        public async Task ServiceShouldLoadCategories()
        {
            //Act
            var result = await productService.LoadCategoriesAsync();

            //Assert
            Assert.Equal(6, result.Count());
        }
        [Fact]
        public async Task ServiceShouldLoadLatestProducts()
        {
            //Arrange
            var latestProductsTest = data.Products.OrderByDescending(p => p.TimeAdded).Take(4).ToList();
           
            //Act
            var latestProducts = await productService.LoadLatestAsync();

            //Assert
            Assert.Equal(4, latestProducts.Count());
            Assert.False(latestProducts.Where(p => p.IsAvailable == false).Any());
            Assert.Equal(latestProducts, latestProductsTest);
        }
        [Fact]
        public async Task ServiceShouldLoadCarouselProducts()
        {
            //Arrange
            List<Product> mostExpensive = data.Products.OrderByDescending(p => p.Price).Take(4).ToList();

            //Act
            var carouselProducts = await productService.LoadCarouselAsync();

            //Arrange
            Assert.Equal(4, carouselProducts.Count());
            Assert.False(carouselProducts.Where(p => p.IsAvailable == false).Any());
            Assert.Equal(carouselProducts, mostExpensive);
        }
        [Fact]
        public async Task ServiceShouldLoadProducts()
        {

            //Act
            var result = await productService.LoadProductsAsync();

            //Assert

            Assert.Equal(6, result.Count());
        }

        //CREATE PRODUCT
        [Fact]
        public async Task ServiceShouldCreateProduct()
        {
            //Arrange
            CreateProductViewModel viewModel = new CreateProductViewModel()
            {
                Name = "Test",
                Description = "TestDescriptionOfProduct",
                ImageUrl = "TestUrl",
                Price = 420,
                CategoryId = Guid.NewGuid()
            };

            //Act
            await productService.AddProductAsync(viewModel);

            //Assert
            Assert.Equal(7, data.Products.Count());
        }
        [Fact]
        public async Task CreateProductShouldThrowWhenNameExists()
        {
            //Arrange
            CreateProductViewModel viewModel = new CreateProductViewModel()
            {
                Name = "GuitarTest1",
                Description = "TestDescriptionOfProduct",
                ImageUrl = "TestUrl",
                Price = 420,
                CategoryId = Guid.NewGuid()
            };

            //Act
            var result = await Assert.ThrowsAsync<ArgumentException>(async () => await productService.AddProductAsync(viewModel));

            //Assert
            Assert.Equal("Product with this name already exists!", result.Message);
        }

        //UNLIST
        [Fact]
        public async Task UnlistProductShouldThrowWhenProductAlreadyUnlisted()
        {
            //Arrange
            UnlistProductViewModel viewModel = new UnlistProductViewModel()
            {
                Name = "GuitarTest1"
            };

            //Act
            Assert.Equal(6, data.Products.Where(p => p.IsAvailable).Count());
            await productService.UnlistProductAsync(viewModel);
            Assert.Equal(5, data.Products.Where(p => p.IsAvailable).Count());
            var result = await Assert.ThrowsAsync<ArgumentException>(async () => await productService.UnlistProductAsync(viewModel));

            //Assert

            Assert.Equal("Product is already unlisted.", result.Message);
        }
        [Fact]
        public async Task UnlistProductShouldThrowWhenProductDoesNotExist()
        {
            //Arrange
            UnlistProductViewModel viewModel = new UnlistProductViewModel()
            {
                Name = "GuitarTestNotExist"
            };

            //Act
            var result = await Assert.ThrowsAsync<ArgumentException>(async () => await productService.UnlistProductAsync(viewModel));

            //Assert

            Assert.Equal("Product does not exist.", result.Message);
        }
        [Fact]
        public async Task ServiceShouldUnlistProduct()
        {
            //Arrange
            UnlistProductViewModel viewModel = new UnlistProductViewModel()
            {
                Name = "GuitarTest1"
            };

            //Act
            Assert.Equal(6, data.Products.Where(p => p.IsAvailable).Count());
            await productService.UnlistProductAsync(viewModel);

            //Assert
            Assert.Equal(5, data.Products.Where(p => p.IsAvailable).Count());
        }

        [Fact]
        public async Task ServiceShouldGetAllByCategoryName()
        {
            //Arrange
            string categoryName = "TestCategory1";
            string searchQuery = string.Empty;
            int currentPage = 0;
            data.Products.Add(new Product()
            {
                Name = "GuitarTest12",
                Description = "testDescription",
                IsAvailable = true,
                TimeAdded = DateTime.Now,
                ImageUrl = "testUrl",
                Price = 10000,
                Category = data.Categories.FirstOrDefault(c => c.Name == "TestCategory1")
            });
            data.SaveChanges();
            //Act
            var result = await productService.GetAllAsync(categoryName, searchQuery, currentPage);
            //Assert
            Assert.Equal(2, result.Products.Count());
        }
        [Fact]
        public async Task ServiceShouldGetAllBySearchQuery()
        {
            //Arrange
            string categoryName = string.Empty;
            string searchQuery = "1";
            int currentPage = 0;
            data.Products.Add(new Product()
            {
                Name = "GuitarTest12",
                Description = "testDescription",
                IsAvailable = true,
                TimeAdded = DateTime.Now,
                ImageUrl = "testUrl",
                Price = 10000,
                Category = data.Categories.FirstOrDefault(c => c.Name == "TestCategory1")
            });
            data.SaveChanges();
            //Act
            var result = await productService.GetAllAsync(categoryName, searchQuery, currentPage);
            //Assert
            Assert.Equal(2, result.Products.Count());
        }
        
        private void SeedDatabase()
        {
            List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Name="GuitarTest1",
                    Description="testDescription",
                    IsAvailable=true,
                    TimeAdded=DateTime.Now,
                    ImageUrl="testUrl",
                    Price=10000,
                    Category=new Category()
                    {
                        Name="TestCategory1",
                        ImageUrl="TestUrl"
                    }
                },
                new Product()
                {
                    Name="GuitarTest2",
                    Description="testDescription2",
                    IsAvailable=true,
                    TimeAdded=DateTime.Now,
                    ImageUrl="testUrl2",
                    Price=3001,
                    Category=new Category()
                    {
                        Name="TestCategory2",
                        ImageUrl="TestUrl"
                    }
                },
                new Product()
                {
                    Name="GuitarTest3",
                    Description="testDescription",
                    IsAvailable=true,
                    TimeAdded=DateTime.Now,
                    ImageUrl="testUrl",
                    Price=20000,
                    Category=new Category()
                    {
                        Name="TestCategory3",
                        ImageUrl="TestUrl"
                    }
                },
                new Product()
                {
                    Name="GuitarTest4",
                    Description="testDescription",
                    IsAvailable=true,
                    TimeAdded=DateTime.Now,
                    ImageUrl="testUrl",
                    Price=3000,
                    Category=new Category()
                    {
                        Name="TestCategory4",
                        ImageUrl="TestUrl"
                    }
                },
                new Product()
                {
                    Name="GuitarTest5",
                    Description="testDescription",
                    IsAvailable=true,
                    TimeAdded=DateTime.Now,
                    ImageUrl="testUrl",
                    Price=60000,
                    Category=new Category()
                    {
                        Name="TestCategory5",
                        ImageUrl="TestUrl"
                    }
                },
                new Product()
                {
                    Name="GuitarTest6",
                    Description="testDescription",
                    IsAvailable=true,
                    TimeAdded=DateTime.Now,
                    ImageUrl="testUrl",
                    Price=30005,
                    Category=new Category()
                    {
                        Name="TestCategory6",
                        ImageUrl="TestUrl"
                    }
                },
            };
            data.Products.AddRange(products);
            data.SaveChanges();
        }
    }
}
