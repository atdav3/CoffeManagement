using System;
using System.Data;
using System.Data.SqlClient;
using CoffeeManagement.DAL.Database;
using CoffeeManagement.DTO.Entities;

namespace CoffeeManagement.DAL.Repositories
{
    public sealed class UserRepository
    {
        public UserCredential FindByUsername(string userName)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_User_GetByUsername", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) return null;
                        return new UserCredential
                        {
                            Session = new UserSessionDto
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                RoleName = reader.GetString(reader.GetOrdinal("RoleName")),
                                EmployeeName = reader.IsDBNull(reader.GetOrdinal("EmployeeName")) ? null : reader.GetString(reader.GetOrdinal("EmployeeName")),
                                EmployeeId = reader.IsDBNull(reader.GetOrdinal("EmployeeId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("EmployeeId"))
                            },
                            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        };
                    }
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to read account information.", ex); }
        }

        public void RecordLogin(int userId)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_User_RecordLogin", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to record sign in.", ex); }
        }

        public void CreateFirstAdmin(string userName, string passwordHash)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_User_CreateFirstAdmin", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName;
                    command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 256).Value = passwordHash;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to create the initial administrator.", ex); }
        }

        public long CountUsers()
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_User_Count", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    return Convert.ToInt64(command.ExecuteScalar());
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to check initial setup status.", ex); }
        }

        public System.Data.DataTable GetAll()
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_User_GetAll", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    System.Data.DataTable data = new System.Data.DataTable("Users"); adapter.Fill(data); return data;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load accounts.", ex); }
        }

        public System.Data.DataTable GetRoles()
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Role_GetAll", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    System.Data.DataTable data = new System.Data.DataTable("Roles"); adapter.Fill(data); return data;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load roles.", ex); }
        }

        public void CreateAccount(string userName, string passwordHash, string roleName, int employeeId, int actorUserId)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_User_Create", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName;
                    command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 256).Value = passwordHash;
                    command.Parameters.Add("@RoleName", SqlDbType.NVarChar, 30).Value = roleName;
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;
                    command.Parameters.Add("@ActorUserId", SqlDbType.Int).Value = actorUserId;
                    connection.Open(); command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to create account.", ex); }
        }

        public void SetActive(int userId, bool active, int actorUserId)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_User_SetActive", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = active;
                    command.Parameters.Add("@ActorUserId", SqlDbType.Int).Value = actorUserId;
                    connection.Open(); command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to update account status.", ex); }
        }
    }

    public sealed class UserCredential
    {
        public UserSessionDto Session { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
    }
}
