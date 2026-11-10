using System;
using System.Data;
using System.Data.SqlClient;
using CoffeeManagement.DAL.Database;

namespace CoffeeManagement.DAL.Repositories
{
    public sealed class AuditRepository
    {
        public DataTable Get(DateTime start, DateTime endExclusive)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_AuditLog_Get", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Start", SqlDbType.DateTime2).Value = BusinessTime.ToUtc(start);
                    command.Parameters.Add("@EndExclusive", SqlDbType.DateTime2).Value = BusinessTime.ToUtc(endExclusive);
                    DataTable result = new DataTable("AuditLogs"); adapter.Fill(result); return result;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load activity log.", ex); }
        }
    }
}
