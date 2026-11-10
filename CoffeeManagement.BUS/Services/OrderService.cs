using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CoffeeManagement.BUS.Exceptions;
using CoffeeManagement.DAL.Database;
using CoffeeManagement.DAL.Repositories;
using CoffeeManagement.DTO.Entities;

namespace CoffeeManagement.BUS.Services
{
    public sealed class OrderService
    {
        private readonly OrderRepository repository = new OrderRepository();

        public long Save(OrderDto order)
        {
            if (order == null || order.EmployeeId <= 0) throw new BusinessException("A logged-in employee is required to create an order.");
            if (order.Lines == null || order.Lines.Count == 0) throw new BusinessException("Add at least one product to the order.");
            if (order.Lines.Any(line => line == null || line.ProductId <= 0 || line.Quantity <= 0 || line.Quantity > 9999 || HasMoreThanDecimals(line.Quantity, 3)))
                throw new BusinessException("Order quantities must be greater than zero, at most 9,999 and have no more than 3 decimals.");
            order.Lines = order.Lines.GroupBy(x => x.ProductId)
                .Select(group => new OrderLineDto { ProductId = group.Key, Quantity = group.Sum(x => x.Quantity) }).ToList();
            if (order.Lines.Any(line => line.Quantity > 9999)) throw new BusinessException("Combined product quantity cannot exceed 9,999.");
            return repository.Save(order);
        }

        public decimal CollectPayment(long orderId, string method, decimal amount, int userId, string reference, decimal? tenderedAmount)
        {
            if (orderId <= 0 || userId <= 0) throw new BusinessException("A valid order and signed-in user are required.");
            if (method != "Cash" && method != "Transfer") throw new BusinessException("Select a supported payment method.");
            if (amount <= 0) throw new BusinessException("Payment amount must be greater than zero.");
            if (amount > 9999999999999999.99m || HasMoreThanDecimals(amount, 2)) throw new BusinessException("Payment amount must fit the invoice precision of 2 decimal places.");
            if (method == "Cash" && (!tenderedAmount.HasValue || tenderedAmount.Value < amount)) throw new BusinessException("Cash tendered must cover the amount due.");
            if (tenderedAmount.HasValue && (tenderedAmount.Value > 9999999999999999.99m || HasMoreThanDecimals(tenderedAmount.Value, 2))) throw new BusinessException("Tendered cash must fit the payment precision of 2 decimal places.");
            if (method == "Transfer" && string.IsNullOrWhiteSpace(reference)) throw new BusinessException("Transfer reference is required.");
            return repository.CollectPayment(orderId, method, amount, userId, reference, tenderedAmount);
        }

        public decimal GetTotal(long orderId) { return repository.GetTotal(orderId); }
        public decimal GetBalance(long orderId) { return repository.GetBalance(orderId); }
        public DataTable GetAll(DateTime start, DateTime end, string status, int? employeeId = null)
        {
            if (start.Date > end.Date) throw new BusinessException("Start date must be on or before end date.");
            if (employeeId.HasValue && employeeId.Value <= 0) throw new BusinessException("Select a valid employee filter.");
            return repository.GetAll(start.Date, end.Date.AddDays(1), status, employeeId);
        }
        public DataTable GetDetails(long orderId) { if (orderId <= 0) throw new BusinessException("Select an order first."); return repository.GetDetails(orderId); }
        public void Cancel(long orderId, UserSessionDto actor)
        {
            if (orderId <= 0 || actor == null || actor.UserId <= 0) throw new BusinessException("Select an order and sign in again.");
            if (actor.RoleName != "Admin" && actor.RoleName != "Manager") throw new BusinessException("You do not have permission to cancel orders.");
            repository.Cancel(orderId, actor.UserId);
        }
        public void TransferTable(long orderId, int? tableId, int userId)
        {
            if (orderId <= 0 || userId <= 0) throw new BusinessException("Select an open order and sign in again.");
            repository.TransferTable(orderId, tableId, userId);
        }
        public void Merge(long sourceId, long targetId, int userId)
        {
            if (sourceId <= 0 || targetId <= 0 || sourceId == targetId || userId <= 0) throw new BusinessException("Select two different open orders.");
            repository.Merge(sourceId, targetId, userId);
        }
        public long Split(long sourceId, int? tableId, int userId, System.Collections.Generic.IList<OrderSplitLineDto> lines)
        {
            if (sourceId <= 0 || userId <= 0 || lines == null || lines.Count == 0) throw new BusinessException("Select an order line to split.");
            if (lines.Any(line => line == null || line.OrderDetailId <= 0 || line.Quantity <= 0 || line.Quantity > 9999 || HasMoreThanDecimals(line.Quantity, 3))) throw new BusinessException("Split quantity must be positive, at most 9,999 and have no more than 3 decimals.");
            DataTable table = new DataTable("OrderSplitLines"); table.Columns.Add("OrderDetailId", typeof(long)); table.Columns.Add("Quantity", typeof(decimal)); table.PrimaryKey = new[] { table.Columns["OrderDetailId"] };
            foreach (OrderSplitLineDto line in lines) table.Rows.Add(line.OrderDetailId, line.Quantity);
            try { return repository.Split(sourceId, tableId, userId, table); }
            catch (DataAccessException exception)
            {
                if (exception.NativeErrorCode == 51051)
                    throw new BusinessException("That split would change the invoice total due to rounding. Adjust the quantities and try again.");
                throw;
            }
        }

        private static bool HasMoreThanDecimals(decimal value, int decimalPlaces)
        {
            return decimal.Round(value, decimalPlaces) != value;
        }
    }
}
