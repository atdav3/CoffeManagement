using System;
using System.Data;
using System.Data.SqlClient;
using CoffeeManagement.DAL.Database;
using CoffeeManagement.DTO.Entities;

namespace CoffeeManagement.DAL.Repositories
{
    public sealed class InventoryRepository
    {
        public DataTable GetIngredients() { return Query("dbo.sp_Ingredient_GetAll", "Ingredients"); }
        public DataTable GetUnits() { return Query("dbo.sp_Unit_GetAll", "Units"); }
        public int SaveUnit(UnitDto unit)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Unit_Save", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UnitId", SqlDbType.Int).Value = unit.UnitId == 0 ? (object)DBNull.Value : unit.UnitId;
                    command.Parameters.Add("@UnitName", SqlDbType.NVarChar, 40).Value = unit.UnitName;
                    command.Parameters.Add("@Abbreviation", SqlDbType.NVarChar, 12).Value = unit.Abbreviation;
                    connection.Open(); return Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to save unit.", ex); }
        }
        public void DeleteUnit(int id)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Unit_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; command.Parameters.Add("@UnitId", SqlDbType.Int).Value = id;
                    connection.Open(); command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to delete unit.", ex); }
        }
        public DataTable GetHistory(DateTime start, DateTime endExclusive)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Inventory_GetHistory", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Start", SqlDbType.DateTime2).Value = BusinessTime.ToUtc(start);
                    command.Parameters.Add("@EndExclusive", SqlDbType.DateTime2).Value = BusinessTime.ToUtc(endExclusive);
                    DataTable result = new DataTable("InventoryHistory"); adapter.Fill(result); return result;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load inventory history.", ex); }
        }

        private DataTable Query(string procedure, string tableName)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand(procedure, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    DataTable result = new DataTable(tableName); adapter.Fill(result); return result;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load inventory data.", ex); }
        }
        public int Save(IngredientDto ingredient)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Ingredient_Save", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IngredientId", SqlDbType.Int).Value = ingredient.IngredientId == 0 ? (object)DBNull.Value : ingredient.IngredientId;
                    command.Parameters.Add("@IngredientName", SqlDbType.NVarChar, 120).Value = ingredient.IngredientName;
                    command.Parameters.Add("@UnitId", SqlDbType.Int).Value = ingredient.UnitId;
                    SqlParameter reorder = command.Parameters.Add("@ReorderLevel", SqlDbType.Decimal); reorder.Precision = 18; reorder.Scale = 3; reorder.Value = ingredient.ReorderLevel;
                    connection.Open(); return Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to save ingredient.", ex); }
        }
        public void Post(int ingredientId, int userId, string type, decimal quantity, decimal? unitCost, string note)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Inventory_Post", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@IngredientId", SqlDbType.Int).Value = ingredientId;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    command.Parameters.Add("@TransactionType", SqlDbType.VarChar, 12).Value = type;
                    SqlParameter qty = command.Parameters.Add("@Quantity", SqlDbType.Decimal); qty.Precision = 18; qty.Scale = 3; qty.Value = quantity;
                    SqlParameter cost = command.Parameters.Add("@UnitCost", SqlDbType.Decimal); cost.Precision = 18; cost.Scale = 2; cost.Value = (object)unitCost ?? DBNull.Value;
                    command.Parameters.Add("@Note", SqlDbType.NVarChar, 300).Value = (object)note ?? DBNull.Value;
                    connection.Open(); command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to post inventory transaction.", ex); }
        }
    }
}
