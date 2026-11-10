using System;
using System.Data;
using System.Data.SqlClient;
using CoffeeManagement.DAL.Database;
using CoffeeManagement.DTO.Entities;

namespace CoffeeManagement.DAL.Repositories
{
    public sealed class OrderRepository
    {
        public DataTable GetAll(DateTime start, DateTime endExclusive, string status, int? employeeId)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Order_GetAll", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Start", SqlDbType.DateTime2).Value = BusinessTime.ToUtc(start);
                    command.Parameters.Add("@EndExclusive", SqlDbType.DateTime2).Value = BusinessTime.ToUtc(endExclusive);
                    command.Parameters.Add("@OrderStatus", SqlDbType.VarChar, 12).Value = (object)status ?? DBNull.Value;
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = (object)employeeId ?? DBNull.Value;
                    DataTable result = new DataTable("Orders"); adapter.Fill(result); return result;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load orders.", ex); }
        }

        public DataTable GetDetails(long orderId)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Order_GetDetails", connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrderId", SqlDbType.BigInt).Value = orderId;
                    DataTable result = new DataTable("OrderDetails"); adapter.Fill(result); return result;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load order details.", ex); }
        }

        public void Cancel(long orderId, int userId)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Order_Cancel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrderId", SqlDbType.BigInt).Value = orderId;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    connection.Open(); command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to cancel order.", ex); }
        }

        public void TransferTable(long orderId, int? tableId, int userId)
        {
            Execute("dbo.sp_Order_TransferTable", command =>
            {
                command.Parameters.Add("@OrderId", SqlDbType.BigInt).Value = orderId;
                command.Parameters.Add("@TargetTableId", SqlDbType.Int).Value = (object)tableId ?? DBNull.Value;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            });
        }

        public void Merge(long sourceId, long targetId, int userId)
        {
            Execute("dbo.sp_Order_Merge", command =>
            {
                command.Parameters.Add("@SourceOrderId", SqlDbType.BigInt).Value = sourceId;
                command.Parameters.Add("@TargetOrderId", SqlDbType.BigInt).Value = targetId;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
            });
        }

        public long Split(long sourceId, int? tableId, int userId, DataTable lines)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Order_Split", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@SourceOrderId", SqlDbType.BigInt).Value = sourceId;
                    command.Parameters.Add("@TargetTableId", SqlDbType.Int).Value = (object)tableId ?? DBNull.Value;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    SqlParameter tvp = command.Parameters.AddWithValue("@Lines", lines); tvp.SqlDbType = SqlDbType.Structured; tvp.TypeName = "dbo.OrderSplitLineType";
                    connection.Open(); return Convert.ToInt64(command.ExecuteScalar());
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to split order.", ex); }
        }

        private static void Execute(string procedure, Action<SqlCommand> configure)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand(procedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure; configure(command); connection.Open(); command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to update order.", ex); }
        }

        public long Save(OrderDto order)
        {
            DataTable lines = new DataTable();
            lines.Columns.Add("ProductId", typeof(int));
            lines.Columns.Add("Quantity", typeof(decimal));
            foreach (OrderLineDto line in order.Lines) lines.Rows.Add(line.ProductId, line.Quantity);
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Order_Save", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrderId", SqlDbType.BigInt).Value = (object)order.OrderId ?? DBNull.Value;
                    command.Parameters.Add("@TableId", SqlDbType.Int).Value = (object)order.TableId ?? DBNull.Value;
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = order.EmployeeId;
                    command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = (object)order.CustomerId ?? DBNull.Value;
                    command.Parameters.Add("@PromotionId", SqlDbType.Int).Value = (object)order.PromotionId ?? DBNull.Value;
                    command.Parameters.Add("@Note", SqlDbType.NVarChar, 500).Value = (object)order.Note ?? DBNull.Value;
                    SqlParameter tvp = command.Parameters.AddWithValue("@Lines", lines);
                    tvp.SqlDbType = SqlDbType.Structured;
                    tvp.TypeName = "dbo.OrderLineType";
                    connection.Open();
                    return Convert.ToInt64(command.ExecuteScalar());
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to save order.", ex); }
        }

        public decimal CollectPayment(long orderId, string method, decimal amount, int userId, string reference, decimal? tenderedAmount)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                    using (SqlCommand command = new SqlCommand("dbo.sp_Payment_Collect", connection, transaction))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OrderId", SqlDbType.BigInt).Value = orderId;
                        command.Parameters.Add("@PaymentMethod", SqlDbType.VarChar, 16).Value = method;
                        SqlParameter amountParameter = command.Parameters.Add("@Amount", SqlDbType.Decimal);
                        amountParameter.Precision = 18; amountParameter.Scale = 2; amountParameter.Value = amount;
                        command.Parameters.Add("@ReceivedByUserId", SqlDbType.Int).Value = userId;
                        command.Parameters.Add("@ReferenceCode", SqlDbType.NVarChar, 100).Value = (object)reference ?? DBNull.Value;
                        SqlParameter tendered = command.Parameters.Add("@TenderedAmount", SqlDbType.Decimal);
                        tendered.Precision = 18; tendered.Scale = 2; tendered.Value = (object)tenderedAmount ?? DBNull.Value;
                        decimal balance = Convert.ToDecimal(command.ExecuteScalar());
                        transaction.Commit();
                        return balance;
                    }
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to collect payment.", ex); }
        }

        public decimal GetTotal(long orderId)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Order_GetHeader", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrderId", SqlDbType.BigInt).Value = orderId;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) throw new DataAccessException("Order was not found.");
                        return reader.GetDecimal(reader.GetOrdinal("TotalAmount"));
                    }
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to read order total.", ex); }
        }

        public decimal GetBalance(long orderId)
        {
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand("dbo.sp_Order_GetHeader", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@OrderId", SqlDbType.BigInt).Value = orderId;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) throw new DataAccessException("Order was not found.");
                        return reader.GetDecimal(reader.GetOrdinal("TotalAmount")) - reader.GetDecimal(reader.GetOrdinal("PaidAmount"));
                    }
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to read order balance.", ex); }
        }
    }
}
