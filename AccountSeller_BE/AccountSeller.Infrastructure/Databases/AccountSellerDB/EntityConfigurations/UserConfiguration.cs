using AccountSeller.Infrastructure.Databases.AccountSellerDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountSeller.Infrastructure.Databases.AccountSellerDB.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(entity => entity.Name)
                .IsRequired()
                .HasColumnName("Name");

            builder.Property(entity => entity.InsertUserId)
                .HasColumnName("InsertUserID")
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasComment("InsertUserId");

            builder.Property(entity => entity.InsertDate)
                .HasColumnName("InsertDate")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("InsertDate");

            builder.Property(entity => entity.UpdateUserId)
                .HasColumnName("UpdateUserID")
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasComment("UpdateUserId");

            builder.Property(entity => entity.UpdateDate)
                .HasColumnName("UpdateDate")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("UpdateDate");

            builder.Property(entity => entity.DeleteUserId)
                .HasColumnName("DeleteUserID")
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasComment("DeleteUserId");

            builder.Property(entity => entity.DeleteDate)
                .HasColumnName("DeleteDate")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("DeleteDate");

            builder.Property(entity => entity.Status)
              .HasColumnName("Status")
              .HasColumnType("INT")
              .HasComment("Status");
        }
    }
}
