using System.Reflection;
using Data.CQS.QueriesHandlers;
using Entities_Context.Entities.UserNews;
using Microsoft.EntityFrameworkCore;
using Serilog;
using FluentValidation;
using Hangfire;
using Microsoft.OpenApi.Models;
using Web_Api_Controllers.Validators;
using Web_Api_Controllers.Extensions;

namespace Web_Api_Controllers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<UserArticleContext>(opt =>
            {
                var connString = builder.Configuration
                    .GetConnectionString("DefaultConnection");
                opt.UseSqlServer(connString);

            });

            builder.JwtConfiguration();

            builder.Services.AddHttpContextAccessor();

            builder.Host.UseSerilog((ctx, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(ctx.Configuration);

            });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(builder.Configuration
                        .GetConnectionString("DefaultConnection")));

            builder.Services.AddGoodNewsAggregatorServices();

            builder.Services.AddHangfireServer();

            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            
                builder.Services.AddMediatR(
                cfg =>
                    cfg.RegisterServicesFromAssemblyContaining<GetArticleByPageQueryHandler>());
            
            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            app.UseHangfireDashboard(
            //    "/hangfire", new DashboardOptions
            //{

            //    Authorization = new[] { new MyAuthorizationFilter() }

            //}
            );
            app.UseSerilogRequestLogging();
            app.MapControllers();

            builder.Services.GoodNewsAggregatorRecurringJobs();

            app.Run();
        }
    }
}