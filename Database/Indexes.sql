USE CoffeeManagementDB;
GO
CREATE INDEX IX_Products_Category_Active ON dbo.Products(CategoryId, IsActive, IsAvailable) INCLUDE (ProductName, UnitPrice);
CREATE INDEX IX_Orders_OpenedAt_Status ON dbo.Orders(OpenedAt, OrderStatus) INCLUDE (EmployeeId, TotalAmount, TableId);
CREATE INDEX IX_Orders_Employee_OpenedAt ON dbo.Orders(EmployeeId, OpenedAt) INCLUDE (OrderStatus, TotalAmount);
CREATE INDEX IX_OrderDetails_OrderId ON dbo.OrderDetails(OrderId) INCLUDE (ProductId, Quantity, UnitPrice, LineTotal);
CREATE INDEX IX_Payments_OrderId_PaidAt ON dbo.Payments(OrderId, PaidAt) INCLUDE (Amount, PaymentMethod);
CREATE INDEX IX_InventoryTransactions_Ingredient_Created ON dbo.InventoryTransactions(IngredientId, CreatedAt) INCLUDE (TransactionType, Quantity);
CREATE INDEX IX_AuditLogs_CreatedAt ON dbo.AuditLogs(CreatedAt DESC);
GO
