USE CoffeeManagementDB;
GO
CREATE VIEW dbo.vw_InventoryOnHand AS
SELECT i.IngredientId, i.IngredientName, i.UnitId, u.Abbreviation AS Unit,
       i.ReorderLevel,
       COALESCE(SUM(CASE WHEN t.TransactionType = 'Receipt' THEN t.Quantity
                         WHEN t.TransactionType = 'Issue' THEN -t.Quantity
                         ELSE t.Quantity END), 0) AS OnHand
FROM dbo.Ingredients AS i
JOIN dbo.Units AS u ON u.UnitId = i.UnitId
LEFT JOIN dbo.InventoryTransactions AS t ON t.IngredientId = i.IngredientId
GROUP BY i.IngredientId, i.IngredientName, i.UnitId, u.Abbreviation, i.ReorderLevel;
GO
CREATE VIEW dbo.vw_DailySales AS
SELECT CONVERT(date, (ClosedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') AS SalesDate, COUNT_BIG(*) AS InvoiceCount,
       SUM(TotalAmount) AS Revenue
FROM dbo.Orders
WHERE OrderStatus = 'Paid'
GROUP BY CONVERT(date, (ClosedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time');
GO
CREATE VIEW dbo.vw_BestSellingProducts AS
SELECT d.ProductId, d.ProductNameSnapshot AS ProductName,
       SUM(d.Quantity) AS QuantitySold, SUM(d.LineTotal) AS Revenue
FROM dbo.OrderDetails AS d
JOIN dbo.Orders AS o ON o.OrderId = d.OrderId
WHERE o.OrderStatus = 'Paid'
GROUP BY d.ProductId, d.ProductNameSnapshot;
GO
