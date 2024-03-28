using AccountSeller.Infrastructure.Databases.CommandInterceptor;
using AccountSeller.Infrastructure.Databases.AccountSellerDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountSeller.Infrastructure.Databases
{
    public interface IDbContextFactory
    {
        /// <summary>
        /// Creates the key seeb context instance.
        /// </summary>
        /// <returns></returns>
        public AccountSellerDbContext CreateKeySeebContextInstance();
    }
    public class DbContextFactory : IDbContextFactory
    {
        private readonly DbContextOptions<AccountSellerDbContext> _keySeeDbContextOptions;

        public DbContextFactory(IConfiguration configuration)
        {
            if (!Directory.Exists($"{configuration["FtpServer:Directory"]}Logs/")){
                Directory.CreateDirectory($"{configuration["FtpServer:Directory"]}Logs/");
            }
            _keySeeDbContextOptions = new DbContextOptionsBuilder<AccountSellerDbContext>()
                .AddInterceptors(new KeySeeDbContextCommandInterceptor($"{configuration["FtpServer:Directory"]}Logs/AccountSellerDbContextSqlCommandLog.txt"))
                .UseSqlServer(
                    connectionString: configuration.GetConnectionString("AccountSellerDB"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.CommandTimeout((int)TimeSpan.FromSeconds(SettingConstants.DatabaseSettings.TIMEOUT_FROM_SECONDS).TotalSeconds);
                        //sqlOptions.EnableRetryOnFailure();
                    })
                .EnableDetailedErrors()
                .Options;
        }

        public AccountSellerDbContext CreateKeySeebContextInstance()
        {
            return new AccountSellerDbContext(_keySeeDbContextOptions);
        }
    }
}
