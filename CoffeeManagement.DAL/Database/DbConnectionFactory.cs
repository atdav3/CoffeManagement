using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CoffeeManagement.DAL.Database
{
    public static class DbConnectionFactory
    {
        public static SqlConnection Create()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["CoffeeManagementDB"];
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new DataAccessException(
                    "Connection string 'CoffeeManagementDB' is missing or empty.", null,
                    DatabaseFailureCategory.Configuration);

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(settings.ConnectionString);
                if (string.IsNullOrWhiteSpace(builder.DataSource) || string.IsNullOrWhiteSpace(builder.InitialCatalog))
                    throw new ArgumentException("The connection string must specify Data Source and Initial Catalog.");
                return new SqlConnection(builder.ConnectionString);
            }
            catch (ArgumentException ex)
            {
                throw new DataAccessException("Connection string 'CoffeeManagementDB' is invalid.", ex,
                    DatabaseFailureCategory.Configuration);
            }
        }
    }
}
