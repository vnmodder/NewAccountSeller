using AccountSeller.Infrastructure.Databases.AccountSellerDB.Entities;
using AccountSeller.Infrastructure.Databases.Common.BaseEntityConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountSeller.Infrastructure.Databases.AccountSellerDB.EntityConfigurations
{
    public class InformationConfiguration : BaseEntityConfiguration<Information>
    {
        public override void Configure(EntityTypeBuilder<Information> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.TotalMoney)
              .HasColumnName("FullName");
            builder.Property(e => e.FullName)
              .IsUnicode(true)
              .HasColumnName("FullName");
        }
    }
}
