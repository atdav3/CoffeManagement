using System;
using System.Data;
using System.Data.SqlClient;
using CoffeeManagement.DAL.Database;
using CoffeeManagement.DTO.Entities;

namespace CoffeeManagement.DAL.Repositories
{
    public sealed class ProductRepository
    {
        public DataTable GetAll(string search, int? categoryId)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Product_GetAll", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Search", SqlDbType.NVarChar, 120).Value = (object)search ?? DBNull.Value;
                    command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = (object)categoryId ?? DBNull.Value;
                    DataSet data = new DataSet("Products");
                    adapter.Fill(data);
                    return data.Tables[0];
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load products.", ex); }
        }

        public int Save(ProductDto product)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Product_Save", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ProductId", SqlDbType.Int).Value = product.ProductId == 0 ? (object)DBNull.Value : product.ProductId;
                    command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = product.CategoryId;
                    command.Parameters.Add("@ProductName", SqlDbType.NVarChar, 120).Value = product.ProductName;
                    command.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = (object)product.Description ?? DBNull.Value;
                    command.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = product.UnitPrice;
                    command.Parameters["@UnitPrice"].Precision = 18; command.Parameters["@UnitPrice"].Scale = 2;
                    command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 500).Value = (object)product.ImagePath ?? DBNull.Value;
                    command.Parameters.Add("@IsAvailable", SqlDbType.Bit).Value = product.IsAvailable;
                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to save product.", ex); }
        }

        public void Delete(int productId)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Product_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
                    connection.Open(); command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to delete product.", ex); }
        }
    }
}
