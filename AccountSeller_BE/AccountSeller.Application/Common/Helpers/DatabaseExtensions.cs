using AccountSeller.Domain.Messages;
using AccountSeller.Application.Common.Interfaces;
using AccountSeller.Domain.Exceptions;
using AccountSeller.Infrastructure.Databases.Common.BaseEntityModels;
using AccountSeller.Infrastructure.Databases.AccountSellerDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text;
namespace AccountSeller.Application.Common.Helpers
{
    public static class DatabaseExtensions
    {
        #region Query extensions
        public static IQueryable<T> Exist<T>(this IQueryable<T> query) where T : BaseEntity
        {
            return query.Where(x => x.DeleteDate == null);
        }

        public static IQueryable<T> Exist<T>(this DbSet<T> query) where T : BaseEntity
        {
            return query.Where(x => x.DeleteDate == null);
        }

        public static IQueryable<User> Exist(this DbSet<User> query)
        {
            return query.Where(x => x.DeleteDate == null);
        }

        public static IQueryable<User> Exist(this IQueryable<User> query)
        {
            return query.Where(x => x.DeleteDate == null);
        }

        public static bool IsSkipped<T>(this ICollection<T>? value) where T : struct => value == null || value.Any();
        public static bool IsSkipped(this object? value) => value == null || Convert.ToString(value) == string.Empty;

        public static IQueryable<T> NotExist<T>(this IQueryable<T> query) where T : BaseEntity
        {
            return query.Where(x => x.DeleteDate.HasValue);
        }

        public static IQueryable<T> NotExist<T>(this DbSet<T> query) where T : BaseEntity
        {
            return query.Where(x => x.DeleteDate.HasValue);
        }
        #endregion

        #region Entities state management extension

        public static void InsertTracking<T>(
            this DbSet<T> dbSet,
            T entity,
            Guid currentUserId) where T : BaseEntity
        {
            entity.InsertDate = DateTimeHelper.Now;
            entity.InsertUserId = currentUserId;
            dbSet.Add(entity);
        }

        public static void UpdateTracking<T>(
            this DbSet<T> dbSet,
            T entity,
            Guid currentUserId) where T : BaseEntity
        {
            entity.UpdateDate = DateTimeHelper.Now;
            entity.UpdateUserId = currentUserId;
            dbSet.Update(entity);
        }

        public static void DeleteTracking<T>(
            this DbSet<T> dbSet,
            T entity,
            Guid currentUserId) where T : BaseEntity
        {
            entity.DeleteDate = DateTimeHelper.Now;
            entity.DeleteUserId = currentUserId;
            entity.IsDeleted = true;
            dbSet.Update(entity);
        }

        public static void BulkInsertTracking<T>(
            this DbSet<T> dbSet,
            ICollection<T> entities,
            Guid currentUserId) where T : BaseEntity
        {
            var now = DateTimeHelper.Now;
            entities = entities.Select(e =>
            {
                e.InsertDate = now;
                e.InsertUserId = currentUserId;
                return e;
            }).ToList();

            dbSet.AddRange(entities);
        }

        public static void BulkUpdateTracking<T>(
            this DbSet<T> dbSet,
            ICollection<T> entities,
            Guid currentUserId) where T : BaseEntity
        {
            var now = DateTimeHelper.Now;
            entities = entities.Select(e =>
            {
                e.UpdateDate = now;
                e.UpdateUserId = currentUserId;
                return e;
            }).ToList();

            dbSet.UpdateRange(entities);
        }

        public static void BulkDeleteTracking<T>(
            this DbSet<T> dbSet,
            ICollection<T> entities,
            Guid currentUserId) where T : BaseEntity
        {
            var now = DateTimeHelper.Now;
            entities = entities.Select(e =>
            {
                e.DeleteDate = now;
                e.DeleteUserId = currentUserId;
                e.IsDeleted = true;
                return e;
            }).ToList();

            dbSet.UpdateRange(entities);
        }
        #endregion

        /// <summary>
        /// Insert entities that have no primary key into database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public static async Task InsertRangeSqlRawAsync<TEntity>(this DbSet<TEntity> dbSet, params TEntity[]? entities)
            where TEntity : class
        {
            if (entities == null)
            {
                return;
            }

            if (entities.Length == 0)
            {
                return;
            }

            var dbContext = dbSet.GetDbContext();
            if (dbContext == null)
            {
                return;
            }

            var sqlRawCommand = dbSet.GenerateInsertSqlComamnd(entities);
            if (string.IsNullOrEmpty(sqlRawCommand))
            {
                return;
            }

            var rowsEffected = await dbContext.Database.ExecuteSqlRawAsync(sqlRawCommand);

            if (rowsEffected != entities.Length)
            {
                throw new BusinessException(message: $"There are {entities.Length} record(s) to be inserted. But actually are {rowsEffected}", errorCode: nameof(ErrorMessages.EM0009));
            }
        }

        /// <summary>
        /// Insert entities that have no primary key into database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public static async Task InsertRangeSqlRawAsync<TEntity>(this DbSet<TEntity> dbSet, ICollection<TEntity>? entities)
            where TEntity : class
        {
            if (entities == null)
            {
                return;
            }

            if (entities.Count == 0)
            {
                return;
            }

            await dbSet.InsertRangeSqlRawAsync(entities.ToArray());
        }

        /// <summary>
        /// Generate a T-SQL INSERT statement to insert <paramref name="entities"/> from memory to database.
        /// <br></br>
        /// </summary>
        /// <typeparam name="TEntity">Class name of entity in database.</typeparam>
        /// <param name="dbSet"><see cref="DbSet{TEntity}"/> instance.</param>
        /// <param name="entities">Entities instances in type of <paramref name="dbSet"/>.</param>
        /// <returns></returns>
        private static string GenerateInsertSqlComamnd<TEntity>(this DbSet<TEntity> dbSet, params TEntity[] entities)
            where TEntity : class
        {
            var tableName = dbSet.GetDatabaseTableName();
            if (string.IsNullOrEmpty(tableName))
            {
                return string.Empty;
            }

            StringBuilder sqlcommandBuilder = new();
            // INSERT INTO [TABLE NAME] (
            sqlcommandBuilder
                .Append("INSERT INTO ")
                .Append(tableName)
                .AppendLine("(")
                .Append("\t ");

            string[] columnNames = entities.FirstOrDefault()!.GetType().GetProperties()
                .Select(prop => prop.Name).ToArray();

            string sqlColumnNames = string.Join("\r\n\t,", columnNames);

            //   ColumnA
            //  ,ColumnB
            //  ,ColumnC
            // ) VALUES
            sqlcommandBuilder
                .AppendLine(sqlColumnNames)
                .AppendLine(") VALUES");

            foreach (var (entity, index) in entities.WithIndex())
            {
                sqlcommandBuilder
                    .AppendLine(" (")
                    .Append("\t ");

                string[] columnValues = entity.GetType().GetProperties()
                    .Select(prop => prop.GetValue(entity).TransformToSqlValue() + "\t-- " + prop.Name).ToArray();

                string sqlColumnValues = string.Join("\r\n\t,", columnValues);

                // (
                //   Value1  -- ColumnA
                //  ,Value2  -- ColumnB
                //  ,Value3  -- ColumnC
                // )
                sqlcommandBuilder
                    .AppendLine(sqlColumnValues)
                    .Append(')');

                if (index != entities.Length - 1)
                {
                    sqlcommandBuilder.AppendLine(",");
                }
            }

            return sqlcommandBuilder.ToString();
        }

        /// <summary>
        /// Transform a C# value to SQL value for injecting to a INSERT statement.
        /// </summary>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        private static string TransformToSqlValue(this object? columnValue)
        {
            switch (columnValue)
            {
                case null:
                    return "null";
                case int or long or short or byte:
                    return columnValue.ToString();
                case DateTime:
                    {
                        if (DateTime.TryParse(columnValue.ToString(), out DateTime time))
                        {
                            return $"\'{time.ToString("yyyy-MM-ddTHH:mm:ss")}\'";
                        }

                        return "NULL";
                    }
                case bool:
                    return columnValue.ToString() == "True" ? "1" : "0";
                default:
                    return $"N\'{columnValue.ToString()}\'";
            }
        }

        /// <summary>
        /// Get table name in database of <see cref="DbSet{TEntity}"/> instance.
        /// </summary>
        /// <typeparam name="TEntity">Class name of entity in database.</typeparam>
        /// <param name="dbSet"><see cref="DbSet{TEntity}"/> instance.</param>
        /// <returns></returns>
        public static string GetDatabaseTableName<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class
        {
            var dbContext = dbSet.GetDbContext();
            if (dbContext == null)
            {
                return string.Empty;
            }

            var model = dbContext.Model;
            var entityTypes = model.GetEntityTypes();
            var entityType = entityTypes.First(t => t.ClrType == typeof(TEntity));
            var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");
            var tableName = tableNameAnnotation != null
                ? $"[{tableNameAnnotation.Value}]"
                : string.Empty;
            return tableName;
        }

        /// <summary>
        /// Get <see cref="DbContext"/> instance of current <see cref="DbSet{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">Class name of entity in database.</typeparam>
        /// <param name="dbSet"><see cref="DbSet{TEntity}"/> instance.</param>
        /// <returns></returns>
        public static DbContext? GetDbContext<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class
        {
            var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
            var serviceProvider = infrastructure.Instance;
            var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext;

            return currentDbContext?.Context;
        }

        /// <summary>
        /// Return a tuple of (element, index) of a collection for scanning.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<(T element, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((element, index) => (element, index));

       
       

        /// <summary>
        /// Check that web API can connect to database or not.
        /// </summary>
        /// <param name="autoThrowException">If this param value is <c>true</c>, throw a <see cref="BusinessException"/> instance with message <see cref="ErrorMessages.EM0103"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<bool> CheckDatabaseConnectionAsync(
            this DbContext dbContext,
            ILogService logger,
            bool autoThrowException = true,
            CancellationToken cancellationToken = default)
        {
            var connectable = await dbContext.Database.CanConnectAsync(cancellationToken);
            if (autoThrowException && !connectable)
            {
                logger.Warn($"Can not connect to database: [{dbContext.Database.ProviderName}].");
                throw ExceptionHelper.GenerateBusinessException(nameof(ErrorMessages.EM0103));
            }

            return connectable;
        }

        /// <summary>
        /// Check exclusive processing for master tables which only have date column to check inherited from <see cref="BaseEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="actionTime"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// <c>True</c>: There are another one update data before current user modifying. <br></br>
        /// <c>False</c>: There are no one update data before current user modifying, allow to updating.
        /// </returns>
        public static async Task<bool> CheckExclusiveProcessingForMasterTablesAsync<TEntity>(
            this DbSet<TEntity> dbSet,
            DateTime actionTime,
            CancellationToken cancellationToken = default) where TEntity : BaseEntity
        {
            return await dbSet.AsNoTracking()
                .AnyAsync(record => record.InsertDate > actionTime
                                 || record.UpdateDate > actionTime
                                 || record.DeleteDate > actionTime, cancellationToken);
        }
    }
}
