using System;
using CoffeeManagement.BUS.Exceptions;
using CoffeeManagement.DAL.Database;
using CoffeeManagement.DTO.Entities;

namespace CoffeeManagement.BUS.Services
{
    public sealed class DatabaseConnectionService
    {
        public DatabaseConnectionResultDto TestConnection()
        {
            try
            {
                DatabaseConnectionTestResult result = DatabaseConnectionTest.Run();
                return new DatabaseConnectionResultDto
                {
                    ServerName = result.ServerName,
                    InstanceName = result.InstanceName,
                    DatabaseName = result.DatabaseName,
                    UsersTableAvailable = result.UsersTableAvailable,
                    UserCountProcedureAvailable = result.UserCountProcedureAvailable,
                    UserCount = result.UserCount
                };
            }
            catch (DataAccessException ex)
            {
                throw new DatabaseConnectionException(ex.Category, FriendlyMessage(ex.Category), ex);
            }
        }

        private static string FriendlyMessage(DatabaseFailureCategory category)
        {
            switch (category)
            {
                case DatabaseFailureCategory.Configuration:
                    return "Database configuration error. Check the CoffeeManagementDB entry in CoffeeManagement.GUI.exe.config.";
                case DatabaseFailureCategory.Network:
                    return "SQL Server could not be reached. Check that MSI\\SQLEXPRESS is running and reachable.";
                case DatabaseFailureCategory.Authentication:
                    return "Windows authentication failed. Confirm the application runs as MSI\\ADMIN and that this account can access CoffeeManagementDB.";
                default:
                    return "The connection opened, but SQL Server rejected the database or read query. Check database access and stored procedure dbo.sp_User_Count.";
            }
        }
    }
}
