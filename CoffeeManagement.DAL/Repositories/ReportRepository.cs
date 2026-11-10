using System;
using System.Data;
using System.Data.SqlClient;
using CoffeeManagement.DAL.Database;
using CoffeeManagement.DTO.Enums;

namespace CoffeeManagement.DAL.Repositories
{
    public sealed class ReportRepository
    {
        public DataTable Get(ReportKind kind, DateTime start, DateTime endExclusive)
        {
            string procedure;
            switch (kind)
            {
                case ReportKind.SalesByDay: procedure = "dbo.sp_Report_SalesByPeriod"; break;
                case ReportKind.SalesByMonth: procedure = "dbo.sp_Report_SalesByMonthPeriod"; break;
                case ReportKind.SalesByYear: procedure = "dbo.sp_Report_SalesByYear"; break;
                case ReportKind.BestSellingProducts: procedure = "dbo.sp_Report_BestSellers"; break;
                case ReportKind.SalesByEmployee: procedure = "dbo.sp_Report_SalesByEmployee"; break;
                case ReportKind.Invoices: procedure = "dbo.sp_Report_OrderInvoices"; break;
                default: throw new ArgumentOutOfRangeException("kind");
            }
            try
            {
                using (SqlConnection connection = DbConnectionFactory.Create())
                using (SqlCommand command = new SqlCommand(procedure, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Start", SqlDbType.DateTime2).Value = BusinessTime.ToUtc(start);
                    command.Parameters.Add("@EndExclusive", SqlDbType.DateTime2).Value = BusinessTime.ToUtc(endExclusive);
                    DataTable result = new DataTable("Report"); adapter.Fill(result); return result;
                }
            }
            catch (SqlException ex) { throw new DataAccessException("Unable to load report data.", ex); }
        }
    }
}
