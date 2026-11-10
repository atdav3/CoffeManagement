using System;
using System.Data.SqlClient;

namespace CoffeeManagement.DAL.Database
{
    public enum DatabaseFailureCategory
    {
        Configuration,
        Network,
        Authentication,
        Database
    }

    public sealed class DataAccessException : Exception
    {
        public int? NativeErrorCode { get; private set; }
        public DatabaseFailureCategory Category { get; private set; }

        public DataAccessException(string message, Exception innerException = null)
            : this(message, innerException, Classify(innerException)) { }

        public DataAccessException(string message, Exception innerException, DatabaseFailureCategory category)
            : base(message, innerException)
        {
            SqlException sqlException = innerException as SqlException;
            if (sqlException != null) NativeErrorCode = sqlException.Number;
            Category = category;
        }

        private static DatabaseFailureCategory Classify(Exception exception)
        {
            if (exception is System.Configuration.ConfigurationErrorsException || exception is ArgumentException)
                return DatabaseFailureCategory.Configuration;

            SqlException sqlException = exception as SqlException;
            if (sqlException == null) return DatabaseFailureCategory.Database;

            switch (sqlException.Number)
            {
                case 18452:
                case 18456:
                case 18470:
                case 18486:
                case 18487:
                case 18488:
                    return DatabaseFailureCategory.Authentication;
                case -1:
                case 2:
                case 53:
                case 258:
                case 11001:
                case 10060:
                case 10061:
                    return DatabaseFailureCategory.Network;
                default:
                    return DatabaseFailureCategory.Database;
            }
        }
    }
}
