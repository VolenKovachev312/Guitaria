using Guitaria.Contracts;
using Guitaria.Core.Services;
using Guitaria.Data;
using Guitaria.Data.Models;
using Guitaria.Services;
using Guitaria.Test.Mocks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Guitaria.Test.Service
{
    public class ShoppingCartServiceTest
    {
        private ApplicationDbContext data = DatabaseMock.Instance;
        private ShoppingCartService shoppingCartService;

        public ShoppingCartServiceTest()
        {
            shoppingCartService = new ShoppingCartService(data);
            SeedDatabase();
        }
        private void SeedDatabase()
        {
            var user = new User()
            {
                UserName = "TestUser",
                Email = "TestEmail@gmail.com"
            };
            data.Users.Add(user);
            var product = new Product()
            {
                Name = "TESTESTESTEST",
                IsAvailable = true,
                Description = "TESTESTSETESTSETESTES",
                Category = new Category()
                {
                    Name = "TESTSE",
                    ImageUrl = "TESTES"
                },
                ImageUrl = "TESTES",
                Price = 360
            };
            data.Products.Add(product);
            data.SaveChanges();
            var shoppingCart = data.ShoppingCarts.Include(sc => sc.ShoppingCartProducts).FirstOrDefault();
            shoppingCart.ShoppingCartProducts.Add(new ShoppingCartProduct()
            {
                Product = product,
                ShoppingCart = user.ShoppingCart
            });

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


        [Fact]
        public async Task ServiceShouldClearCart()
        {
            //Arrange
            var user = data.Users.Include(u => u.PurchaseHistory).Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).ThenInclude(sc => sc.Product).FirstOrDefault();
            var userId = user.Id.ToString();

            //Act
            Assert.True(user.ShoppingCart.ShoppingCartProducts.Any());
            await shoppingCartService.ClearCartAsync(userId);

            //Assert
            Assert.False(user.ShoppingCart.ShoppingCartProducts.Any());
        }

        [Fact]
        public async Task ServiceShouldLoadProducts()
        {
            //Arrange
            var user = data.Users.Include(u => u.PurchaseHistory).Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).ThenInclude(sc => sc.Product).FirstOrDefault();
            var userId = user.Id.ToString();

            //Act
            var products = await shoppingCartService.LoadProductsAsync(userId);

            //Arrage
            Assert.Equal(1, products.Count());
        }

        [Fact]
        public async Task ServiceShouldLoadProductsAtCheckout()
        {
            //Arrange
            var user = data.Users.Include(u => u.PurchaseHistory).Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).ThenInclude(sc => sc.Product).FirstOrDefault();
            var userId = user.Id.ToString();

            //Act
            var shoppingCart = await shoppingCartService.LoadProductsCheckoutAsync(userId);

            //Arrage
            Assert.Equal(1, shoppingCart.Count());
        }

        [Fact]
        public async Task ServiceShouldLoadPurchaseHistory()
        {
            //Arrange
            var user = data.Users.Include(u => u.PurchaseHistory).Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).ThenInclude(sc => sc.Product).FirstOrDefault();
            var userId = user.Id.ToString();

            //Act
            var purchaseHistory = await shoppingCartService.LoadPurchaseHistoryAsync(userId);

            //Assert
            Assert.True(purchaseHistory != null);
        }

        [Fact]
        public async Task ServiceShouldRemoveProductFromCart()
        {
            //Arrange
            var user = data.Users.Include(u => u.PurchaseHistory).Include(u => u.ShoppingCart).ThenInclude(sc => sc.ShoppingCartProducts).ThenInclude(sc => sc.Product).FirstOrDefault();
            var userId = user.Id.ToString();

            var productId = data.Products.Select(p => p.Id).FirstOrDefault();

            //Act
            Assert.Equal(1, user.ShoppingCart.ShoppingCartProducts.Count);
            await shoppingCartService.RemoveProductAsync(userId, productId);

            //Assert
            Assert.Equal(0, user.ShoppingCart.ShoppingCartProducts.Count);
        }

       
    }
}
