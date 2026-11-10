USE CoffeeManagementDB;
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Roles)
    INSERT dbo.Roles(RoleName, Description) VALUES
    (N'Admin', N'Full system access'), (N'Manager', N'Operations and reports'),
    (N'Cashier', N'POS and payments'), (N'Staff', N'Order entry');
IF NOT EXISTS (SELECT 1 FROM dbo.Categories)
    INSERT dbo.Categories(CategoryName, Description) VALUES
    (N'Cà phê', N'Cà phê nóng và lạnh'), (N'Trà', N'Trà và trà trái cây'), (N'Khác', N'Đồ uống khác');
IF NOT EXISTS (SELECT 1 FROM dbo.[Tables])
    INSERT dbo.[Tables](TableName, AreaName) VALUES (N'Bàn 01', N'Tầng 1'), (N'Bàn 02', N'Tầng 1'), (N'Bàn 03', N'Tầng 1');
IF NOT EXISTS (SELECT 1 FROM dbo.Units)
    INSERT dbo.Units(UnitName, Abbreviation) VALUES (N'Gram', N'g'), (N'Milliliter', N'ml'), (N'Piece', N'cái');
IF NOT EXISTS (SELECT 1 FROM dbo.Products)
BEGIN
    INSERT dbo.Products(CategoryId, ProductName, UnitPrice)
    SELECT CategoryId, N'Cà phê đen', 25000 FROM dbo.Categories WHERE CategoryName=N'Cà phê'
    UNION ALL SELECT CategoryId, N'Cà phê sữa', 30000 FROM dbo.Categories WHERE CategoryName=N'Cà phê'
    UNION ALL SELECT CategoryId, N'Trà đào', 35000 FROM dbo.Categories WHERE CategoryName=N'Trà';
END;
IF NOT EXISTS (SELECT 1 FROM dbo.Ingredients)
BEGIN
    INSERT dbo.Ingredients(UnitId,IngredientName,ReorderLevel)
    SELECT UnitId,N'Hạt cà phê',500 FROM dbo.Units WHERE Abbreviation=N'g'
    UNION ALL SELECT UnitId,N'Sữa tươi',1000 FROM dbo.Units WHERE Abbreviation=N'ml'
    UNION ALL SELECT UnitId,N'Siro đào',500 FROM dbo.Units WHERE Abbreviation=N'ml';
    INSERT dbo.InventoryTransactions(IngredientId,TransactionType,Quantity,ReferenceCode,Note)
    SELECT IngredientId,'Receipt',2000,'SEED-OPENING',N'Tồn đầu kỳ mẫu' FROM dbo.Ingredients;
END;
IF NOT EXISTS (SELECT 1 FROM dbo.Recipes)
BEGIN
    INSERT dbo.Recipes(ProductId,IngredientId,QuantityRequired)
    SELECT p.ProductId,i.IngredientId,20 FROM dbo.Products p CROSS JOIN dbo.Ingredients i
    WHERE p.ProductName=N'Cà phê đen' AND i.IngredientName=N'Hạt cà phê'
    UNION ALL SELECT p.ProductId,i.IngredientId,20 FROM dbo.Products p CROSS JOIN dbo.Ingredients i
    WHERE p.ProductName=N'Cà phê sữa' AND i.IngredientName=N'Hạt cà phê'
    UNION ALL SELECT p.ProductId,i.IngredientId,30 FROM dbo.Products p CROSS JOIN dbo.Ingredients i
    WHERE p.ProductName=N'Trà đào' AND i.IngredientName=N'Siro đào';
END;
GO
