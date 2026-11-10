using System;
using System.Data;
using System.Data.SqlClient;
using CoffeeManagement.DAL.Database;

namespace CoffeeManagement.DAL.Repositories
{
    public sealed class DashboardRepository
    {
        public DataSet GetSummary()
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Dashboard_Summary", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Date", SqlDbType.Date).Value = BusinessTime.Today;
                    DataSet data = new DataSet("Dashboard");
                    adapter.Fill(data);
                    return data;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load dashboard totals.", ex); }
        }
    }
}
