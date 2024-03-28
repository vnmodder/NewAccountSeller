using AccountSeller.Infrastructure.Databases.Common.BaseEntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountSeller.Infrastructure.Databases.Common.BaseEntityConfiguration
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(entity => entity.Id)
                .HasColumnName("ID")
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasComment("Khóa chính");

            builder.Property(entity => entity.InsertUserId)
                .HasColumnName("InsertUserID")
                .HasColumnType("UNIQUEIDENTIFIER");

            builder.Property(entity => entity.InsertDate)
                .HasColumnName("InsertDate")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(entity => entity.UpdateUserId)
                .HasColumnName("UpdateUserID")
                .HasColumnType("UNIQUEIDENTIFIER");

            builder.Property(entity => entity.UpdateDate)
                .HasColumnName("UpdateDate")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(entity => entity.DeleteUserId)
                .HasColumnName("DeleteUserID")
                .HasColumnType("UNIQUEIDENTIFIER");

            builder.Property(entity => entity.DeleteDate)
                .HasColumnName("DeleteDate")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(entity => entity.IsDeleted)
                .HasColumnName("IsDeleted")
                .HasColumnType("BIT")
                .HasDefaultValueSql("0");

        }
    }
}
