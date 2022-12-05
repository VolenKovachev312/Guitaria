using Guitaria.Data;
using Microsoft.EntityFrameworkCore;

namespace Guitaria.Infrastrcture
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<ApplicationDbContext>();

            data.Database.Migrate();

            return app;
        }

    }
}
