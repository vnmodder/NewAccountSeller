using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace AccountSeller.Infrastructure.Databases.CommandInterceptor
{
    /// <summary>
    /// REMOVE INTERCEPTOR AFTER RELEASE PRODUCTION.
    /// </summary>
    public abstract class AbstractCommandInterceptor : DbCommandInterceptor
    {
        protected string LogFilePath { get; } = string.Empty;

        public AbstractCommandInterceptor(string filePath)
        {
            LogFilePath = filePath;
        }

        public override DbCommand CommandInitialized(CommandEndEventData eventData, DbCommand result)
        {
            LogCommand(result);
            return base.CommandInitialized(eventData, result);
        }

        public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
        {
            if (!string.IsNullOrEmpty(result.CommandText))
            {
                LogCommand(result);
            }
            return base.CommandCreated(eventData, result);
        }

        public override InterceptionResult<int> NonQueryExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<int> result)
        {
            LogCommand(command);
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            LogCommand(command);
            return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            LogCommand(command);
            return base.ReaderExecuting(command, eventData, result);
        }

        public override DbDataReader ReaderExecuted(
            DbCommand command,
            CommandExecutedEventData eventData,
            DbDataReader result)
        {
            LogCommand(command);
            return base.ReaderExecuted(command, eventData, result);
        }

        /// <summary>
        /// Log command that executed into database.
        /// </summary>
        /// <param name="command"></param>
        private void LogCommand(DbCommand command)
        {
            string commandText = command.CommandText;
            if (commandText.StartsWith("SELECT"))
            {
                return;
            }

            foreach (DbParameter parameter in command.Parameters)
            {
                var delcareSql = $"DECLARE {parameter.ParameterName} {TransformToSqlType(parameter.Value)} = {TransformToSqlValue(parameter.Value)};\r\n";
                commandText = delcareSql + commandText;
            }

            File.AppendAllText(LogFilePath, commandText + "\r\nGO\r\n");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        private static string TransformToSqlType(object? columnValue)
        {
            return columnValue switch
            {
                int => "INT",
                long => "BIGINT",
                short => "SMALLINT",
                byte => "TINYINT",
                DateTime => "DATETIME",
                bool => "BIT",
                string => "NVARCHAR(MAX)",
                byte[] => "VARBINARY",
                double => "FLOAT",
                decimal => "DECIMAL",
                TimeSpan => "TIME",
                Guid => "UNIQUEIDENTIFIER",
                DateTimeOffset => "DATETIMEOFFSET",
                _ => "NVARCHAR(MAX)",
            };
        }

        /// <summary>
        /// Transform a C# value to SQL value for injecting to a INSERT statement.
        /// </summary>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        private static string TransformToSqlValue(object? columnValue)
        {
            if (columnValue == null)
            {
                return "NULL";
            }

            switch (columnValue)
            {
                case null:
                    return "NULL";
                case int or long or short or byte:
                    return columnValue.ToString();
                case DateTime:
                    {
                        if (DateTime.TryParse(columnValue.ToString(), out DateTime time))
                        {
                            return $"\'{time:yyyy-MM-ddTHH:mm:ss}\'";
                        }

                        return "NULL";
                    }
                case bool:
                    return columnValue.ToString() == "True" ? "1" : "0";
                default:
                    return $"N\'{columnValue}\'";
            }
        }
    }
}