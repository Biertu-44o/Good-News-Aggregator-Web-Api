using Entities_Context.Entities.UserNews;
using IServices;
using IServices.Repositories;
using IServices.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MVC.ControllerFactory;
using MVC.Extensions;
using MVC.Filters.Errors;
using MVC.Filters.Validation;
using MVC.Middlware;
using Repositories;
using Repositories.Implementations;
using Serilog;
using Services.Account;
using Services.Article;

namespace MVC
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddDbContext<UserArticleContext>(opt =>
            {
                var connString = builder.Configuration
                    .GetConnectionString("DefaultConnection");
                opt.UseSqlServer(connString);

            });

            builder.Services.AddHttpContextAccessor();

            builder.Host.UseSerilog((ctx, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(ctx.Configuration);

            });

            

            builder.Services.AddAutoMapper(typeof(Program));
            
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");

                    options.AccessDeniedPath = new PathString("/Account/Login");

                });
            
            builder.Services.AddMvc(options =>
            {
                options.Filters.Add<CustomExceptionFilter>();
            });

            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IServiceFactory, ServiceFactory>();
            builder.Services.AddGoodNewsAggregatorServices();
            builder.Services.AddGoodNewsAggregatorRepositories();
            builder.Services.AddGoodNewsAggregatorValidationFilters();

            var app = builder.Build();
            
            //if (!app.Environment.IsDevelopment())
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            
            app.UseMiddleware<ErrorMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Article}/{action=GetArticlesByPage}/{tag?}/{id?}");
            app.Run();
        }
    }
}