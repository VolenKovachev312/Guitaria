using Guitaria.Services;
using Moq;

namespace Guitaria.Test.Mocks
{
    public static class ProductServiceMock
    {
        public static ProductService Instance
        {
            get
            {
                var serviceMock = new Mock<ProductService>();

                return serviceMock.Object;
            }
        }
    }
}
