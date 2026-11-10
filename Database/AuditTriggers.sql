USE CoffeeManagementDB;
GO
CREATE OR ALTER TRIGGER dbo.trg_Audit_Categories ON dbo.Categories AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details)
    SELECT NULL, CASE WHEN i.CategoryId IS NULL THEN N'Delete' WHEN d.CategoryId IS NULL THEN N'Create' ELSE N'Update' END,
           N'Category', CONVERT(nvarchar(80),COALESCE(i.CategoryId,d.CategoryId)), CONCAT(N'Actor=',ORIGINAL_LOGIN())
    FROM inserted i FULL OUTER JOIN deleted d ON i.CategoryId=d.CategoryId;
END;
GO
CREATE OR ALTER TRIGGER dbo.trg_Audit_Products ON dbo.Products AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details)
    SELECT NULL, CASE WHEN i.ProductId IS NULL THEN N'Delete' WHEN d.ProductId IS NULL THEN N'Create' ELSE N'Update' END,
           N'Product', CONVERT(nvarchar(80),COALESCE(i.ProductId,d.ProductId)), CONCAT(N'Actor=',ORIGINAL_LOGIN())
    FROM inserted i FULL OUTER JOIN deleted d ON i.ProductId=d.ProductId;
END;
GO
CREATE OR ALTER TRIGGER dbo.trg_Audit_Employees ON dbo.Employees AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details)
    SELECT NULL, CASE WHEN i.EmployeeId IS NULL THEN N'Delete' WHEN d.EmployeeId IS NULL THEN N'Create' ELSE N'Update' END,
           N'Employee', CONVERT(nvarchar(80),COALESCE(i.EmployeeId,d.EmployeeId)), CONCAT(N'Actor=',ORIGINAL_LOGIN())
    FROM inserted i FULL OUTER JOIN deleted d ON i.EmployeeId=d.EmployeeId;
END;
GO
CREATE OR ALTER TRIGGER dbo.trg_Audit_Tables ON dbo.[Tables] AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details)
    SELECT NULL, CASE WHEN i.TableId IS NULL THEN N'Delete' WHEN d.TableId IS NULL THEN N'Create' ELSE N'Update' END,
           N'Table', CONVERT(nvarchar(80),COALESCE(i.TableId,d.TableId)), CONCAT(N'Actor=',ORIGINAL_LOGIN())
    FROM inserted i FULL OUTER JOIN deleted d ON i.TableId=d.TableId;
END;
GO
CREATE OR ALTER TRIGGER dbo.trg_Audit_Promotions ON dbo.Promotions AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details)
    SELECT NULL, CASE WHEN i.PromotionId IS NULL THEN N'Delete' WHEN d.PromotionId IS NULL THEN N'Create' ELSE N'Update' END,
           N'Promotion', CONVERT(nvarchar(80),COALESCE(i.PromotionId,d.PromotionId)), CONCAT(N'Actor=',ORIGINAL_LOGIN())
    FROM inserted i FULL OUTER JOIN deleted d ON i.PromotionId=d.PromotionId;
END;
GO
CREATE OR ALTER TRIGGER dbo.trg_Audit_Ingredients ON dbo.Ingredients AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details)
    SELECT NULL, CASE WHEN i.IngredientId IS NULL THEN N'Delete' WHEN d.IngredientId IS NULL THEN N'Create' ELSE N'Update' END,
           N'Ingredient', CONVERT(nvarchar(80),COALESCE(i.IngredientId,d.IngredientId)), CONCAT(N'Actor=',ORIGINAL_LOGIN())
    FROM inserted i FULL OUTER JOIN deleted d ON i.IngredientId=d.IngredientId;
END;
GO
CREATE OR ALTER TRIGGER dbo.trg_Audit_Units ON dbo.Units AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details)
    SELECT NULL, CASE WHEN i.UnitId IS NULL THEN N'Delete' WHEN d.UnitId IS NULL THEN N'Create' ELSE N'Update' END,
           N'Unit', CONVERT(nvarchar(80),COALESCE(i.UnitId,d.UnitId)), CONCAT(N'Actor=',ORIGINAL_LOGIN())
    FROM inserted i FULL OUTER JOIN deleted d ON i.UnitId=d.UnitId;
END;
GO
