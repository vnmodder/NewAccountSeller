using AccountSeller.Application.Common.Interfaces;
using AccountSeller.Application.Common.Services;
using AccountSeller.Infrastructure.Databases.AccountSellerDB.Entities;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AccountSeller.Application
{
    public static class ApplicationServiceCollection
    {
        /// <summary>
        /// Adds the application services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            #region Common services
            // JWT
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILogService, LogService>();

            // User Management Service.
            services.AddScoped<UserManager<User>>();
            services.AddScoped<SignInManager<User>, SignInManager<User>>();

            // Auto mapper
            services.AddAutoMapper(typeof(ApplicationServiceCollection).Assembly);

            // Add Mediator
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            // HUFLT Service
            services.AddTransient<IFtpDirectoryService, FtpDirectoryService>();
            #endregion

            #region Business services
            //services.AddScoped<IApproveService, ApproveService>();
            #endregion
            return services;
        }
    }
}