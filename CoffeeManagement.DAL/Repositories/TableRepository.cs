using System;
using System.Data;
using System.Data.SqlClient;
using CoffeeManagement.DAL.Database;

namespace CoffeeManagement.DAL.Repositories
{
    public sealed class TableRepository
    {
        public DataTable GetAll()
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Table_GetAll", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    DataTable result = new DataTable("Tables"); adapter.Fill(result); return result;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load tables.", ex); }
        }

        public int Save(CoffeeManagement.DTO.Entities.CafeTableDto table)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Table_Save", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@TableId", SqlDbType.Int).Value = table.TableId == 0 ? (object)DBNull.Value : table.TableId;
                    command.Parameters.Add("@TableName", SqlDbType.NVarChar, 40).Value = table.TableName;
                    command.Parameters.Add("@AreaName", SqlDbType.NVarChar, 60).Value = table.AreaName;
                    connection.Open(); return System.Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to save table.", ex); }
        }

        public void Delete(int id)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Table_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@TableId", SqlDbType.Int).Value = id;
                    connection.Open(); command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to deactivate table.", ex); }
        }

        public void SetStatus(int id, string status)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Table_SetStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@TableId", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@TableStatus", SqlDbType.VarChar, 12).Value = status;
                    connection.Open(); command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to update table status.", ex); }
        }
    }
}
