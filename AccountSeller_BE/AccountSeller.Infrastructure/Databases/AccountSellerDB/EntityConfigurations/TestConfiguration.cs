using AccountSeller.Infrastructure.Databases.AccountSellerDB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AccountSeller.Infrastructure.Databases.Common.BaseEntityConfiguration;

namespace AccountSeller.Infrastructure.Databases.AccountSellerDB.EntityConfigurations
{
    public class TestConfiguration : BaseEntityConfiguration<Test>
    {
        public override void Configure(EntityTypeBuilder<Test> builder)
        {
            base.Configure(builder);

            builder
                .HasNoKey()
                .ToTable("Test");

            builder.Property(e => e.TestName)
              .HasMaxLength(12)
              .IsUnicode(false)
              .IsFixedLength()
              .HasColumnName("TestName");
        }
    }
}
