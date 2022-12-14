using Guitaria.Data;
using Guitaria.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Guitaria.Core.Services;
using Guitaria.Contracts;
using Microsoft.AspNetCore.Mvc;
using Guitaria.Infrastrcture;

namespace Guitaria;
public class StartUp
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services
            .AddAntiforgery()
            .AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
        });
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

        builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/User/Login");

        builder.Services.AddMemoryCache();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        //Seed Database
        app.PrepareDatabase();
        app.InitializeDatabase();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapAreaControllerRoute(
        name: "Areas",
        areaName: "Admin",
        pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
