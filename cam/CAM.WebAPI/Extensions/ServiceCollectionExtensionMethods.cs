using CAM.Contracts;
using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities;
using CAM.Identity;
using CAM.LoggerService;
using CAM.Repository;
using CAM.Repository.Helpers;
using CAM.Repository.NewRepositoryWrapper;
using CAM.WebAPI.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog.Web;
using OracleModels.DBContext;
using System;
using System.IO;

namespace CAM.WebAPI.Extensions
{
    public static class ServiceCollectionExtensionMethods
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureMsSqlContext(this IServiceCollection services, IConfiguration config)
        {
            try
            {
                var connectionString = config["ConnectionStrings:oracle"];
                services.AddDbContext<ModelContext>(o => o.UseOracle(connectionString));
                services.AddDbContext<RepositoryContext>(o => o.UseOracle(connectionString));
                services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<ModelContext>()
                    .AddDefaultTokenProviders();

                var connectionStringNew = config["ConnectionStrings:oraclenew"];
                services.AddDbContext<ModelContextNew>(o => o.UseOracle(connectionStringNew));
                services.AddDbContext<RepositoryContextNew>(o => o.UseOracle(connectionStringNew));
                services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<ModelContextNew>()
                    .AddDefaultTokenProviders();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

         
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper,RepositoryWrapper>();
            services.AddScoped<IRepositoryWrapper,RepositoryWrapperNew>();
        }
    }
}
