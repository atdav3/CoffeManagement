using System;
using System.Data;
using System.Data.SqlClient;
using CoffeeManagement.DAL.Database;
using CoffeeManagement.DTO.Entities;

namespace CoffeeManagement.DAL.Repositories
{
    public sealed class EmployeeRepository
    {
        public DataTable GetAll(string search, string jobTitle = null)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Employee_GetAll", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Search", SqlDbType.NVarChar, 120).Value = (object)search ?? DBNull.Value;
                    command.Parameters.Add("@JobTitle", SqlDbType.NVarChar, 50).Value = (object)jobTitle ?? DBNull.Value;
                    DataTable result = new DataTable("Employees"); adapter.Fill(result); return result;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load employees.", ex); }
        }

        public int Save(EmployeeDto employee)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Employee_Save", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employee.EmployeeId == 0 ? (object)DBNull.Value : employee.EmployeeId;
                    command.Parameters.Add("@EmployeeCode", SqlDbType.NVarChar, 20).Value = employee.EmployeeCode;
                    command.Parameters.Add("@FullName", SqlDbType.NVarChar, 120).Value = employee.FullName;
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = (object)employee.Phone ?? DBNull.Value;
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 254).Value = (object)employee.Email ?? DBNull.Value;
                    command.Parameters.Add("@JobTitle", SqlDbType.NVarChar, 50).Value = (object)employee.JobTitle ?? DBNull.Value;
                    command.Parameters.Add("@HiredAt", SqlDbType.Date).Value = DBNull.Value;
                    connection.Open(); return Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to save employee.", ex); }
        }

        public void Deactivate(int id)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Employee_Deactivate", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = id;
                    connection.Open(); command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to deactivate employee.", ex); }
        }
    }
}
