using Guitaria.Contracts;
using Guitaria.Controllers;
using Guitaria.Models.Home;
using Guitaria.Services;
using Guitaria.Test.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace Guitaria.Test.Controller
{
    public class HomeControllerTest
    {
        //[Fact]
        //public void ErrorShouldReturnView()
        //{
        //    //Arrange
        //    var homeController = new HomeController(ProductServiceMock.Instance);

        //    //Act
        //    var result = homeController.Error();

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.IsType<ViewResult>(result);
        ////}
        //[Fact]
        //public void IndexShouldReturnCorrectViewWithModel()
        //    => MyController<HomeController>
        //        .Instance(controller => controller
        //            .WithData())
        //        .Calling(c => c.Index())
        //        .ShouldHave()
        //        .MemoryCache(cache => cache
        //            .ContainingEntry(entry => entry
        //                .WithKey(C)
        //                .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(15))
        //                .WithValueOfType<List<Carousel>>()))
        //        .AndAlso()
        //        .ShouldReturn()
        //        .View(view => view
        //            .WithModelOfType<List<IndexViewModel>>()
        //            .Passing(model => model.Should().HaveCount(3)));

    }

}
