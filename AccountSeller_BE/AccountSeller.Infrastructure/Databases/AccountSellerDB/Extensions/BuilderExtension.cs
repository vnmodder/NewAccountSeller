using AccountSeller.Infrastructure.Constants;
using AccountSeller.Infrastructure.Databases.AccountSellerDB.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountSeller.Infrastructure.Databases.AccountSellerDB.Extensions
{
    public static class BuilderExtension
    {
        public static void AddDummyData(this ModelBuilder modelBuilder)
        {
            //add default for Role
            var roles = new List<IdentityRole<Guid>>()
            {
                new IdentityRole<Guid>() { Id = Guid.NewGuid(), Name = RoleConstants.ZERO, ConcurrencyStamp = "1", NormalizedName = RoleConstants.ZERO},
                new IdentityRole<Guid>() { Id = Guid.NewGuid(), Name = RoleConstants.ADMIN, ConcurrencyStamp = "2", NormalizedName = RoleConstants.ADMIN },
                new IdentityRole<Guid>() { Id = Guid.NewGuid(), Name = RoleConstants.USER, ConcurrencyStamp = "3", NormalizedName = RoleConstants.USER }
            };

            modelBuilder.Entity<IdentityRole<Guid>>()
                .HasData(roles);
        }
    }
}
