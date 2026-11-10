using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CoffeeManagement.DAL.Database
{
    public sealed class DatabaseConnectionTestResult
    {
        public string ServerName { get; internal set; }
        public string InstanceName { get; internal set; }
        public string DatabaseName { get; internal set; }
        public bool UsersTableAvailable { get; internal set; }
        public bool UserCountProcedureAvailable { get; internal set; }
        public long? UserCount { get; internal set; }
    }

    public static class DatabaseConnectionTest
    {
        public static DatabaseConnectionTestResult Run()
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                {
                    connection.Open();
                    DatabaseConnectionTestResult result = new DatabaseConnectionTestResult();

                    using (SqlCommand command = new SqlCommand(
                        "SELECT DB_NAME(), CONVERT(nvarchar(128), @@SERVERNAME), " +
                        "CONVERT(nvarchar(128), SERVERPROPERTY('InstanceName'));", connection))
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (!reader.Read())
                            throw new DataAccessException("The server information query returned no result.");
                        result.DatabaseName = reader.IsDBNull(0) ? null : reader.GetString(0);
                        result.ServerName = reader.IsDBNull(1) ? null : reader.GetString(1);
                        result.InstanceName = reader.IsDBNull(2) ? null : reader.GetString(2);
                    }

                    using (SqlCommand command = new SqlCommand(
                        "SELECT CASE WHEN OBJECT_ID(N'dbo.Users', N'U') IS NULL THEN 0 ELSE 1 END, " +
                        "CASE WHEN OBJECT_ID(N'dbo.sp_User_Count', N'P') IS NULL THEN 0 ELSE 1 END;", connection))
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            result.UsersTableAvailable = reader.GetInt32(0) == 1;
                            result.UserCountProcedureAvailable = reader.GetInt32(1) == 1;
                        }
                    }

                    using (SqlCommand command = new SqlCommand(
                        "IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL " +
                        "SELECT COUNT_BIG(*) FROM dbo.Users ELSE SELECT CAST(NULL AS bigint);", connection))
                    {
                        object count = command.ExecuteScalar();
                        if (count != null && count != DBNull.Value) result.UserCount = Convert.ToInt64(count);
                    }

                    return result;
                }
            }
            catch (DataAccessException) { throw; }
            catch (SqlException ex)
            {
                throw new DataAccessException("Unable to connect to or read from CoffeeManagementDB.", ex);
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new DataAccessException("Database connection configuration could not be loaded.", ex,
                    DatabaseFailureCategory.Configuration);
            }
            catch (ArgumentException ex)
            {
                throw new DataAccessException("Database connection configuration is invalid.", ex,
                    DatabaseFailureCategory.Configuration);
            }
        }
    }
}
