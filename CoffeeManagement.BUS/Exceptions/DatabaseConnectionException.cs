using System;
using CoffeeManagement.DAL.Database;

namespace CoffeeManagement.BUS.Exceptions
{
    public sealed class DatabaseConnectionException : Exception
    {
        public DatabaseFailureCategory Category { get; private set; }

        public DatabaseConnectionException(DatabaseFailureCategory category, string message, Exception innerException)
            : base(message, innerException)
        {
            Category = category;
        }
    }
}
