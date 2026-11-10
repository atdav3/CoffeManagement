using System;
using System.Data;
using System.Data.SqlClient;
using CoffeeManagement.DAL.Database;
using CoffeeManagement.DTO.Entities;

namespace CoffeeManagement.DAL.Repositories
{
    public sealed class PromotionRepository
    {
        public DataTable GetAll(bool activeOnly)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand(activeOnly ? "dbo.sp_Promotion_GetActive" : "dbo.sp_Promotion_GetAll", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure; DataTable data = new DataTable("Promotions"); adapter.Fill(data); return data;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load promotions.", ex); }
        }
        public int Save(PromotionDto promotion)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Promotion_Save", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@PromotionId", SqlDbType.Int).Value = promotion.PromotionId == 0 ? (object)DBNull.Value : promotion.PromotionId;
                    command.Parameters.Add("@PromotionName", SqlDbType.NVarChar, 100).Value = promotion.PromotionName;
                    SqlParameter discount = command.Parameters.Add("@DiscountPercent", SqlDbType.Decimal); discount.Precision = 5; discount.Scale = 2; discount.Value = promotion.DiscountPercent;
                    command.Parameters.Add("@StartsAt", SqlDbType.DateTime2).Value = BusinessTime.ToUtc(promotion.StartsAt);
                    command.Parameters.Add("@EndsAt", SqlDbType.DateTime2).Value = BusinessTime.ToUtc(promotion.EndsAt);
                    connection.Open(); return Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to save promotion.", ex); }
        }
        public void Disable(int id)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Promotion_Disable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; command.Parameters.Add("@PromotionId", SqlDbType.Int).Value = id; connection.Open(); command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to disable promotion.", ex); }
        }
    }
}
