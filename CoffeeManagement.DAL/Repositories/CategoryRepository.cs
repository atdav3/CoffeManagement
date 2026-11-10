using System;
using System.Data;
using System.Data.SqlClient;
using CoffeeManagement.DAL.Database;

namespace CoffeeManagement.DAL.Repositories
{
    public sealed class CategoryRepository
    {
        public DataTable GetAll()
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Category_GetAll", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    DataTable table = new DataTable("Categories");
                    adapter.Fill(table);
                    return table;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load categories.", ex); }
        }

        public int Save(int? categoryId, string name, string description)
        {
            return ExecuteScalar("dbo.sp_Category_Save", command =>
            {
                command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = (object)categoryId ?? DBNull.Value;
                command.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 80).Value = name;
                command.Parameters.Add("@Description", SqlDbType.NVarChar, 250).Value = (object)description ?? DBNull.Value;
            });
        }

        public void Delete(int id)
        {
            Execute("dbo.sp_Category_Delete", command => command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = id);
        }

        private static int ExecuteScalar(string procedure, Action<SqlCommand> addParameters)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand(procedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    addParameters(command);
                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to save category.", ex); }
        }

        private static void Execute(string procedure, Action<SqlCommand> addParameters)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand(procedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    addParameters(command);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to delete category.", ex); }
        }
    }
}
