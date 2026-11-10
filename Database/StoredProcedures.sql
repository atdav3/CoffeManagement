USE CoffeeManagementDB;
GO
IF TYPE_ID(N'dbo.OrderLineType') IS NULL
    EXEC(N'CREATE TYPE dbo.OrderLineType AS TABLE (ProductId int NOT NULL PRIMARY KEY, Quantity decimal(12,3) NOT NULL CHECK (Quantity > 0));');
GO
IF TYPE_ID(N'dbo.OrderSplitLineType') IS NULL
    EXEC(N'CREATE TYPE dbo.OrderSplitLineType AS TABLE (OrderDetailId bigint NOT NULL PRIMARY KEY, Quantity decimal(12,3) NOT NULL CHECK (Quantity > 0));');
GO
CREATE OR ALTER PROCEDURE dbo.sp_User_GetByUsername @UserName nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UserId, u.UserName, u.PasswordHash, u.IsActive, u.EmployeeId,
           r.RoleName, e.FullName AS EmployeeName
    FROM dbo.Users u JOIN dbo.Roles r ON r.RoleId = u.RoleId
    LEFT JOIN dbo.Employees e ON e.EmployeeId = u.EmployeeId
    WHERE u.UserName = @UserName;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_User_RecordLogin @UserId int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Users SET LastLoginAt = SYSUTCDATETIME(), UpdatedAt = SYSUTCDATETIME()
    WHERE UserId = @UserId AND IsActive = 1;
    IF @@ROWCOUNT = 0 THROW 51001, 'Account is not active.', 1;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId) VALUES(@UserId,N'SignIn',N'User',CONVERT(nvarchar(80),@UserId));
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_User_CreateFirstAdmin
    @UserName nvarchar(50), @PasswordHash nvarchar(256)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    IF EXISTS (SELECT 1 FROM dbo.Users WITH (UPDLOCK, HOLDLOCK))
        THROW 51002, 'Initial administrator has already been created.', 1;
    DECLARE @RoleId tinyint = (SELECT RoleId FROM dbo.Roles WHERE RoleName = N'Admin');
    IF @RoleId IS NULL THROW 51003, 'Admin role is missing.', 1;
    DECLARE @EmployeeId int;
    INSERT dbo.Employees(EmployeeCode, FullName, JobTitle) VALUES (CONCAT(N'INIT-', LEFT(CONVERT(nvarchar(36), NEWID()), 8)), @UserName, N'Administrator');
    SET @EmployeeId = SCOPE_IDENTITY();
    INSERT dbo.Users(UserName, PasswordHash, RoleId, EmployeeId) VALUES (@UserName, @PasswordHash, @RoleId, @EmployeeId);
    COMMIT TRANSACTION;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_User_Count
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT_BIG(*) AS UserCount FROM dbo.Users;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Role_GetAll
AS BEGIN SET NOCOUNT ON; SELECT RoleId, RoleName FROM dbo.Roles WHERE IsActive=1 ORDER BY RoleId; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_User_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT u.UserId,u.UserName,u.IsActive,u.LastLoginAt,e.FullName AS EmployeeName,r.RoleName
    FROM dbo.Users u JOIN dbo.Roles r ON r.RoleId=u.RoleId LEFT JOIN dbo.Employees e ON e.EmployeeId=u.EmployeeId
    ORDER BY u.UserName;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_User_Create @UserName nvarchar(50), @PasswordHash nvarchar(256), @RoleName nvarchar(30), @EmployeeId int, @ActorUserId int
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ActorRole nvarchar(30)=(SELECT r.RoleName FROM dbo.Users u JOIN dbo.Roles r ON r.RoleId=u.RoleId WHERE u.UserId=@ActorUserId AND u.IsActive=1 AND r.IsActive=1);
    IF @ActorRole IS NULL OR @ActorRole NOT IN (N'Admin',N'Manager') THROW 51045,'Only active administrators and managers may create accounts.',1;
    IF @ActorRole=N'Manager' AND @RoleName=N'Admin' THROW 51046,'Managers cannot create administrator accounts.',1;
    DECLARE @RoleId tinyint=(SELECT RoleId FROM dbo.Roles WHERE RoleName=@RoleName AND IsActive=1);
    IF @RoleId IS NULL THROW 51016, 'Role does not exist.', 1;
    IF NOT EXISTS(SELECT 1 FROM dbo.Employees WHERE EmployeeId=@EmployeeId AND IsActive=1) THROW 51017, 'Employee is inactive or missing.', 1;
    INSERT dbo.Users(UserName,PasswordHash,RoleId,EmployeeId) VALUES(@UserName,@PasswordHash,@RoleId,@EmployeeId);
    DECLARE @NewUserId int=CONVERT(int,SCOPE_IDENTITY());
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details) VALUES(@ActorUserId,N'CreateAccount',N'User',CONVERT(nvarchar(80),@NewUserId),CONCAT(N'Role: ',@RoleName));
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_User_SetActive @UserId int, @IsActive bit, @ActorUserId int
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ActorRole nvarchar(30)=(SELECT r.RoleName FROM dbo.Users u JOIN dbo.Roles r ON r.RoleId=u.RoleId WHERE u.UserId=@ActorUserId AND u.IsActive=1 AND r.IsActive=1);
    IF @ActorRole IS NULL OR @ActorRole NOT IN (N'Admin',N'Manager') THROW 51047,'Only active administrators and managers may change account status.',1;
    IF @ActorUserId=@UserId THROW 51048,'You cannot change your own account status.',1;
    IF @ActorRole=N'Manager' AND EXISTS(SELECT 1 FROM dbo.Users u JOIN dbo.Roles r ON r.RoleId=u.RoleId WHERE u.UserId=@UserId AND r.RoleName=N'Admin') THROW 51049,'Managers cannot change administrator accounts.',1;
    UPDATE dbo.Users SET IsActive=@IsActive,UpdatedAt=SYSUTCDATETIME() WHERE UserId=@UserId;
    IF @@ROWCOUNT=0 THROW 51044,'User does not exist.',1;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details)
    VALUES(@ActorUserId,CASE WHEN @IsActive=1 THEN N'EnableAccount' ELSE N'DisableAccount' END,N'User',CONVERT(nvarchar(80),@UserId),NULL);
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Category_GetAll
AS BEGIN SET NOCOUNT ON; SELECT CategoryId, CategoryName, Description, IsActive FROM dbo.Categories WHERE IsActive=1 ORDER BY CategoryName; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Promotion_GetAll
AS BEGIN SET NOCOUNT ON; SELECT PromotionId,PromotionName,DiscountPercent,CONVERT(datetime2(0),(StartsAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') AS StartsAt,CONVERT(datetime2(0),(EndsAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') AS EndsAt,IsActive FROM dbo.Promotions ORDER BY StartsAt DESC; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Promotion_GetActive
AS BEGIN SET NOCOUNT ON; SELECT PromotionId,PromotionName,DiscountPercent,StartsAt,EndsAt FROM dbo.Promotions WHERE IsActive=1 AND SYSUTCDATETIME() BETWEEN StartsAt AND EndsAt ORDER BY PromotionName; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Promotion_Save @PromotionId int=NULL,@PromotionName nvarchar(100),@DiscountPercent decimal(5,2),@StartsAt datetime2(0),@EndsAt datetime2(0)
AS
BEGIN
    SET NOCOUNT ON;
    IF @PromotionId IS NULL INSERT dbo.Promotions(PromotionName,DiscountPercent,StartsAt,EndsAt) VALUES(@PromotionName,@DiscountPercent,@StartsAt,@EndsAt);
    ELSE UPDATE dbo.Promotions SET PromotionName=@PromotionName,DiscountPercent=@DiscountPercent,StartsAt=@StartsAt,EndsAt=@EndsAt,IsActive=1 WHERE PromotionId=@PromotionId;
    SELECT COALESCE(@PromotionId,CONVERT(int,SCOPE_IDENTITY())) AS PromotionId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Promotion_Disable @PromotionId int
AS BEGIN SET NOCOUNT ON; UPDATE dbo.Promotions SET IsActive=0 WHERE PromotionId=@PromotionId; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Category_Save
    @CategoryId int = NULL, @CategoryName nvarchar(80), @Description nvarchar(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @CategoryId IS NULL
        INSERT dbo.Categories(CategoryName, Description) VALUES (@CategoryName, @Description);
    ELSE
        UPDATE dbo.Categories SET CategoryName = @CategoryName, Description = @Description, UpdatedAt = SYSUTCDATETIME() WHERE CategoryId = @CategoryId AND IsActive = 1;
    SELECT COALESCE(@CategoryId, CONVERT(int, SCOPE_IDENTITY())) AS CategoryId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Category_Delete @CategoryId int
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.Products WHERE CategoryId = @CategoryId)
        THROW 51004, 'Category still has products.', 1;
    UPDATE dbo.Categories SET IsActive = 0, UpdatedAt = SYSUTCDATETIME() WHERE CategoryId = @CategoryId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Product_GetAll @Search nvarchar(120) = NULL, @CategoryId int = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProductId, p.CategoryId, c.CategoryName, p.ProductName, p.Description, p.UnitPrice, p.ImagePath, p.IsAvailable, p.IsActive
    FROM dbo.Products p JOIN dbo.Categories c ON c.CategoryId = p.CategoryId
    WHERE p.IsActive = 1 AND c.IsActive = 1 AND (@CategoryId IS NULL OR p.CategoryId = @CategoryId)
      AND (@Search IS NULL OR p.ProductName LIKE N'%' + @Search + N'%')
    ORDER BY c.CategoryName, p.ProductName;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Product_GetById @ProductId int
AS
BEGIN SET NOCOUNT ON; SELECT ProductId, CategoryId, ProductName, Description, UnitPrice, ImagePath, IsAvailable, IsActive FROM dbo.Products WHERE ProductId = @ProductId; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Product_Save
    @ProductId int = NULL, @CategoryId int, @ProductName nvarchar(120), @Description nvarchar(500) = NULL,
    @UnitPrice decimal(18,2), @ImagePath nvarchar(500) = NULL, @IsAvailable bit = 1
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.Categories WHERE CategoryId=@CategoryId AND IsActive=1) THROW 51040,'Category is inactive or missing.',1;
    IF @ProductId IS NULL
        INSERT dbo.Products(CategoryId, ProductName, Description, UnitPrice, ImagePath, IsAvailable) VALUES (@CategoryId, @ProductName, @Description, @UnitPrice, @ImagePath, @IsAvailable);
    ELSE
        UPDATE dbo.Products SET CategoryId=@CategoryId, ProductName=@ProductName, Description=@Description, UnitPrice=@UnitPrice, ImagePath=@ImagePath, IsAvailable=@IsAvailable, UpdatedAt=SYSUTCDATETIME() WHERE ProductId=@ProductId AND IsActive=1;
    SELECT COALESCE(@ProductId, CONVERT(int, SCOPE_IDENTITY())) AS ProductId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Product_Delete @ProductId int
AS BEGIN SET NOCOUNT ON; UPDATE dbo.Products SET IsActive=0, IsAvailable=0, UpdatedAt=SYSUTCDATETIME() WHERE ProductId=@ProductId; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Table_GetAll
AS BEGIN SET NOCOUNT ON; SELECT TableId, TableName, AreaName, TableStatus, IsActive FROM dbo.CafeTables WHERE IsActive=1 ORDER BY AreaName, TableName; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Table_SetStatus @TableId int, @TableStatus varchar(12)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @TableStatus NOT IN ('Available','Reserved')
        THROW 51015, 'Only Available or Reserved can be set manually.', 1;
    BEGIN TRANSACTION;
    IF NOT EXISTS(SELECT 1 FROM dbo.CafeTables WITH (UPDLOCK,HOLDLOCK) WHERE TableId=@TableId AND IsActive=1)
        THROW 51018, 'Table does not exist or is inactive.', 1;
    IF EXISTS(SELECT 1 FROM dbo.Orders WHERE TableId=@TableId AND OrderStatus='Open')
        THROW 51005, 'Table has an open order.', 1;
    UPDATE dbo.CafeTables SET TableStatus=@TableStatus WHERE TableId=@TableId AND IsActive=1;
    COMMIT TRANSACTION;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Table_Save @TableId int = NULL, @TableName nvarchar(40), @AreaName nvarchar(60)
AS
BEGIN
    SET NOCOUNT ON;
    IF @TableId IS NULL INSERT dbo.CafeTables(TableName, AreaName, TableStatus) VALUES (@TableName, @AreaName, 'Available');
    ELSE
    BEGIN
        UPDATE dbo.CafeTables SET TableName=@TableName, AreaName=@AreaName WHERE TableId=@TableId AND IsActive=1;
        IF @@ROWCOUNT=0 THROW 51018, 'Table does not exist or is inactive.', 1;
    END;
    SELECT COALESCE(@TableId, CONVERT(int,SCOPE_IDENTITY())) AS TableId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Table_Delete @TableId int
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    IF NOT EXISTS(SELECT 1 FROM dbo.CafeTables WITH (UPDLOCK,HOLDLOCK) WHERE TableId=@TableId AND IsActive=1)
        THROW 51018, 'Table does not exist or is inactive.', 1;
    IF EXISTS(SELECT 1 FROM dbo.Orders WHERE TableId=@TableId AND OrderStatus='Open') THROW 51014, 'Table has an open order.', 1;
    UPDATE dbo.CafeTables SET IsActive=0, TableStatus='Disabled' WHERE TableId=@TableId;
    COMMIT TRANSACTION;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Unit_GetAll
AS BEGIN SET NOCOUNT ON; SELECT UnitId,UnitName,Abbreviation FROM dbo.Units ORDER BY UnitName; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Unit_Save @UnitId int=NULL,@UnitName nvarchar(40),@Abbreviation nvarchar(12)
AS
BEGIN
    SET NOCOUNT ON;
    IF @UnitId IS NULL INSERT dbo.Units(UnitName,Abbreviation) VALUES(@UnitName,@Abbreviation);
    ELSE
    BEGIN
        IF NOT EXISTS(SELECT 1 FROM dbo.Units WHERE UnitId=@UnitId) THROW 51019,'Unit does not exist.',1;
        UPDATE dbo.Units SET UnitName=@UnitName,Abbreviation=@Abbreviation WHERE UnitId=@UnitId;
    END;
    SELECT COALESCE(@UnitId,CONVERT(int,SCOPE_IDENTITY())) AS UnitId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Unit_Delete @UnitId int
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS(SELECT 1 FROM dbo.Ingredients WHERE UnitId=@UnitId) THROW 51020,'Unit is assigned to an ingredient and cannot be deleted.',1;
    DELETE dbo.Units WHERE UnitId=@UnitId;
    IF @@ROWCOUNT=0 THROW 51019,'Unit does not exist.',1;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Ingredient_GetAll
AS BEGIN SET NOCOUNT ON; SELECT IngredientId,IngredientName,UnitId,Unit,ReorderLevel,OnHand FROM dbo.vw_InventoryOnHand ORDER BY IngredientName; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Ingredient_Save @IngredientId int=NULL,@IngredientName nvarchar(120),@UnitId int,@ReorderLevel decimal(18,3)
AS
BEGIN
    SET NOCOUNT ON;
    IF @IngredientId IS NULL INSERT dbo.Ingredients(IngredientName,UnitId,ReorderLevel) VALUES(@IngredientName,@UnitId,@ReorderLevel);
    ELSE UPDATE dbo.Ingredients SET IngredientName=@IngredientName,UnitId=@UnitId,ReorderLevel=@ReorderLevel WHERE IngredientId=@IngredientId AND IsActive=1;
    SELECT COALESCE(@IngredientId,CONVERT(int,SCOPE_IDENTITY())) AS IngredientId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Inventory_Post @IngredientId int,@UserId int,@TransactionType varchar(12),@Quantity decimal(18,3),@UnitCost decimal(18,2)=NULL,@Note nvarchar(300)=NULL
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    IF NOT EXISTS(SELECT 1 FROM dbo.Ingredients WITH (UPDLOCK,HOLDLOCK) WHERE IngredientId=@IngredientId AND IsActive=1) THROW 51018,'Ingredient is inactive or missing.',1;
    IF @TransactionType NOT IN ('Receipt','Issue') OR @Quantity<=0 THROW 51019,'Inventory transaction is invalid.',1;
    IF @TransactionType='Issue'
    BEGIN
        DECLARE @OnHand decimal(18,3);
        SELECT @OnHand=COALESCE(SUM(CASE WHEN TransactionType='Receipt' THEN Quantity WHEN TransactionType='Issue' THEN -Quantity ELSE Quantity END),0)
        FROM dbo.InventoryTransactions WHERE IngredientId=@IngredientId;
        IF @OnHand<@Quantity THROW 51020,'Insufficient inventory on hand.',1;
    END;
    INSERT dbo.InventoryTransactions(IngredientId,UserId,TransactionType,Quantity,UnitCost,Note) VALUES(@IngredientId,@UserId,@TransactionType,@Quantity,@UnitCost,@Note);
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details) VALUES(@UserId,@TransactionType,N'Inventory',CONVERT(nvarchar(80),@IngredientId),CONCAT(@Quantity,N' units'));
    COMMIT TRANSACTION;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Inventory_GetHistory @Start datetime2(0),@EndExclusive datetime2(0)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.InventoryTransactionId,i.IngredientName,t.TransactionType,t.Quantity,u.Abbreviation AS Unit,t.UnitCost,
           CONVERT(datetime2(0),(t.CreatedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') AS CreatedAt,
           t.ReferenceCode,t.Note,usr.UserName
    FROM dbo.InventoryTransactions t JOIN dbo.Ingredients i ON i.IngredientId=t.IngredientId
    JOIN dbo.Units u ON u.UnitId=i.UnitId LEFT JOIN dbo.Users usr ON usr.UserId=t.UserId
    WHERE t.CreatedAt>=@Start AND t.CreatedAt<@EndExclusive ORDER BY t.CreatedAt DESC;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Employee_GetAll @Search nvarchar(120)=NULL,@JobTitle nvarchar(50)=NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.EmployeeId, e.EmployeeCode, e.FullName, e.Phone, e.Email, e.JobTitle, e.HiredAt, e.IsActive, u.UserName, r.RoleName
    FROM dbo.Employees e LEFT JOIN dbo.Users u ON u.EmployeeId=e.EmployeeId LEFT JOIN dbo.Roles r ON r.RoleId=u.RoleId
    WHERE (@Search IS NULL OR e.EmployeeCode LIKE N'%'+@Search+N'%' OR e.FullName LIKE N'%'+@Search+N'%' OR e.Phone LIKE N'%'+@Search+N'%' OR e.JobTitle LIKE N'%'+@Search+N'%')
      AND (@JobTitle IS NULL OR e.JobTitle=@JobTitle)
    ORDER BY e.FullName;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Employee_Save
    @EmployeeId int=NULL, @EmployeeCode nvarchar(20), @FullName nvarchar(120), @Phone nvarchar(20)=NULL,
    @Email nvarchar(254)=NULL, @JobTitle nvarchar(50)=NULL, @HiredAt date=NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @EmployeeId IS NULL INSERT dbo.Employees(EmployeeCode,FullName,Phone,Email,JobTitle,HiredAt) VALUES(@EmployeeCode,@FullName,@Phone,@Email,@JobTitle,@HiredAt);
    ELSE UPDATE dbo.Employees SET EmployeeCode=@EmployeeCode,FullName=@FullName,Phone=@Phone,Email=@Email,JobTitle=@JobTitle,HiredAt=@HiredAt,UpdatedAt=SYSUTCDATETIME() WHERE EmployeeId=@EmployeeId AND IsActive=1;
    SELECT COALESCE(@EmployeeId,CONVERT(int,SCOPE_IDENTITY())) AS EmployeeId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Employee_Deactivate @EmployeeId int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Employees SET IsActive=0,UpdatedAt=SYSUTCDATETIME() WHERE EmployeeId=@EmployeeId;
    UPDATE dbo.Users SET IsActive=0,UpdatedAt=SYSUTCDATETIME() WHERE EmployeeId=@EmployeeId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_AuditLog_Create @UserId int=NULL, @ActionName nvarchar(80), @EntityName nvarchar(80), @EntityId nvarchar(80)=NULL, @Details nvarchar(1000)=NULL
AS
BEGIN SET NOCOUNT ON; INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details) VALUES(@UserId,@ActionName,@EntityName,@EntityId,@Details); END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_AuditLog_Get @Start datetime2(0),@EndExclusive datetime2(0)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.AuditLogId,l.CreatedAt AS CreatedAtUtc,l.ActionName,l.EntityName,l.EntityId,
           COALESCE(e.FullName,u.UserName,l.Details) AS ActorOrDetails,l.Details
    FROM dbo.AuditLogs l LEFT JOIN dbo.Users u ON u.UserId=l.UserId
    LEFT JOIN dbo.Employees e ON e.EmployeeId=u.EmployeeId
    WHERE l.CreatedAt>=@Start AND l.CreatedAt<@EndExclusive
    ORDER BY l.CreatedAt DESC,l.AuditLogId DESC;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Order_Save
    @OrderId bigint = NULL, @TableId int = NULL, @EmployeeId int, @CustomerId int = NULL,
    @PromotionId int = NULL, @Note nvarchar(500) = NULL, @Lines dbo.OrderLineType READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF NOT EXISTS(SELECT 1 FROM @Lines) THROW 51006, 'An order must contain at least one item.', 1;
    BEGIN TRANSACTION;
    DECLARE @OldTableId int;
    DECLARE @OrderCode nvarchar(24);
    IF @OrderId IS NULL
    BEGIN
        IF NOT EXISTS(SELECT 1 FROM dbo.Employees WHERE EmployeeId=@EmployeeId AND IsActive=1) THROW 51036,'Employee is inactive or missing.',1;
        IF @TableId IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.CafeTables WITH (UPDLOCK,HOLDLOCK) WHERE TableId=@TableId AND IsActive=1 AND TableStatus='Available')
            THROW 51037,'Selected table is no longer available.',1;
        SET @OrderCode = CONCAT('CM', LEFT(REPLACE(CONVERT(varchar(36), NEWID()), '-', ''), 22));
        INSERT dbo.Orders(OrderCode, TableId, EmployeeId, CustomerId, PromotionId, Note)
        VALUES (@OrderCode, @TableId, @EmployeeId, @CustomerId, @PromotionId, @Note);
        SET @OrderId = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        SELECT @OldTableId=TableId FROM dbo.Orders WITH (UPDLOCK, HOLDLOCK) WHERE OrderId=@OrderId AND OrderStatus='Open';
        IF @@ROWCOUNT=0
            THROW 51007, 'Order was not found or is already closed.', 1;
        IF EXISTS(SELECT 1 FROM dbo.Payments WHERE OrderId=@OrderId)
            THROW 51041, 'An order with payments cannot be edited.', 1;
        IF @TableId IS NOT NULL AND @TableId<>@OldTableId
           AND NOT EXISTS(SELECT 1 FROM dbo.CafeTables WITH (UPDLOCK,HOLDLOCK) WHERE TableId=@TableId AND IsActive=1 AND TableStatus='Available')
            THROW 51037, 'Selected table is no longer available.', 1;
        UPDATE dbo.Orders SET TableId=@TableId, CustomerId=@CustomerId, PromotionId=@PromotionId, Note=@Note WHERE OrderId=@OrderId;
        DELETE dbo.OrderDetails WHERE OrderId=@OrderId;
    END;
    INSERT dbo.OrderDetails(OrderId, ProductId, ProductNameSnapshot, UnitPrice, Quantity)
    SELECT @OrderId, p.ProductId, p.ProductName, p.UnitPrice, l.Quantity
    FROM @Lines l JOIN dbo.Products p ON p.ProductId=l.ProductId
    WHERE p.IsActive=1 AND p.IsAvailable=1;
    IF (SELECT COUNT(*) FROM dbo.OrderDetails WHERE OrderId=@OrderId) <> (SELECT COUNT(*) FROM @Lines)
        THROW 51008, 'One or more products are unavailable.', 1;
    DECLARE @Subtotal decimal(18,2)=(SELECT SUM(LineTotal) FROM dbo.OrderDetails WHERE OrderId=@OrderId);
    DECLARE @Discount decimal(18,2)=0;
    IF @PromotionId IS NOT NULL
    BEGIN
        SELECT @Discount=ROUND(@Subtotal*DiscountPercent/100.0,2) FROM dbo.Promotions
        WHERE PromotionId=@PromotionId AND IsActive=1 AND SYSUTCDATETIME() BETWEEN StartsAt AND EndsAt;
        IF @@ROWCOUNT=0 THROW 51009, 'Promotion is invalid or expired.', 1;
    END;
    IF @Subtotal-@Discount<=0 THROW 51043,'Order total must be greater than zero.',1;
    UPDATE dbo.Orders SET Subtotal=@Subtotal, DiscountAmount=@Discount, TotalAmount=@Subtotal-@Discount WHERE OrderId=@OrderId;
    IF @TableId IS NOT NULL UPDATE dbo.CafeTables SET TableStatus='Occupied' WHERE TableId=@TableId;
    IF @OldTableId IS NOT NULL AND @OldTableId<>@TableId
       AND NOT EXISTS(SELECT 1 FROM dbo.Orders WHERE TableId=@OldTableId AND OrderStatus='Open' AND OrderId<>@OrderId)
        UPDATE dbo.CafeTables SET TableStatus='Available' WHERE TableId=@OldTableId;
    DECLARE @AuditUserId int=(SELECT TOP (1) UserId FROM dbo.Users WHERE EmployeeId=@EmployeeId AND IsActive=1);
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId) VALUES(@AuditUserId,N'SaveOrder',N'Order',CONVERT(nvarchar(80),@OrderId));
    COMMIT TRANSACTION;
    SELECT @OrderId AS OrderId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Order_GetOpen
AS
BEGIN
    SET NOCOUNT ON;
    SELECT o.OrderId, o.OrderCode, o.TableId, t.TableName, o.EmployeeId, o.CustomerId, o.Subtotal, o.DiscountAmount, o.TotalAmount, o.OpenedAt
    FROM dbo.Orders o LEFT JOIN dbo.CafeTables t ON t.TableId=o.TableId WHERE o.OrderStatus='Open' ORDER BY o.OpenedAt;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Order_TransferTable @OrderId bigint,@TargetTableId int=NULL,@UserId int=NULL
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    DECLARE @OldTableId int;
    SELECT @OldTableId=TableId FROM dbo.Orders WITH (UPDLOCK,HOLDLOCK) WHERE OrderId=@OrderId AND OrderStatus='Open';
    IF @@ROWCOUNT=0 THROW 51021,'Only an open order can be moved.',1;
    IF @TargetTableId IS NOT NULL
    BEGIN
        IF @TargetTableId=@OldTableId THROW 51022,'Order already belongs to this table.',1;
        IF NOT EXISTS(SELECT 1 FROM dbo.CafeTables WITH (UPDLOCK,HOLDLOCK) WHERE TableId=@TargetTableId AND IsActive=1 AND TableStatus='Available') THROW 51023,'Target table is not available.',1;
        UPDATE dbo.CafeTables SET TableStatus='Occupied' WHERE TableId=@TargetTableId;
    END;
    UPDATE dbo.Orders SET TableId=@TargetTableId WHERE OrderId=@OrderId;
    IF @OldTableId IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.Orders WHERE TableId=@OldTableId AND OrderStatus='Open')
        UPDATE dbo.CafeTables SET TableStatus='Available' WHERE TableId=@OldTableId;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details) VALUES(@UserId,N'TransferTable',N'Order',CONVERT(nvarchar(80),@OrderId),CONCAT(N'TableId: ',COALESCE(CONVERT(nvarchar(20),@TargetTableId),N'Take-away')));
    COMMIT TRANSACTION;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Order_Merge @SourceOrderId bigint,@TargetOrderId bigint,@UserId int=NULL
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF @SourceOrderId=@TargetOrderId THROW 51024,'Choose two different orders.',1;
    BEGIN TRANSACTION;
    DECLARE @SourceTableId int,@TargetPromotionId int,@TargetSubtotal decimal(18,2),@TargetDiscount decimal(18,2),@TargetTotal decimal(18,2);
    IF EXISTS(SELECT 1 FROM dbo.Orders WITH (UPDLOCK,HOLDLOCK) WHERE OrderId IN(@SourceOrderId,@TargetOrderId) AND OrderStatus<>'Open') THROW 51025,'Both orders must be open.',1;
    IF (SELECT COUNT(*) FROM dbo.Orders WHERE OrderId IN(@SourceOrderId,@TargetOrderId))<>2 THROW 51026,'Order was not found.',1;
    IF EXISTS(SELECT 1 FROM dbo.Payments WHERE OrderId IN(@SourceOrderId,@TargetOrderId)) THROW 51027,'Orders with payments cannot be merged.',1;
    IF EXISTS(SELECT 1 FROM dbo.Orders WHERE OrderId=@SourceOrderId AND PromotionId IS NOT NULL) THROW 51038,'Remove the source promotion before merging this order.',1;
    SELECT @SourceTableId=TableId FROM dbo.Orders WHERE OrderId=@SourceOrderId;
    SELECT @TargetPromotionId=PromotionId FROM dbo.Orders WHERE OrderId=@TargetOrderId;
    UPDATE dbo.OrderDetails SET OrderId=@TargetOrderId WHERE OrderId=@SourceOrderId;
    SELECT @TargetSubtotal=SUM(LineTotal) FROM dbo.OrderDetails WHERE OrderId=@TargetOrderId;
    SET @TargetDiscount=0;
    IF @TargetPromotionId IS NOT NULL
    BEGIN
        SET @TargetDiscount=NULL;
        SELECT @TargetDiscount=ROUND(@TargetSubtotal*DiscountPercent/100.0,2) FROM dbo.Promotions WHERE PromotionId=@TargetPromotionId AND IsActive=1 AND SYSUTCDATETIME() BETWEEN StartsAt AND EndsAt;
    END;
    IF @TargetDiscount IS NULL THROW 51028,'Target promotion is invalid.',1;
    SET @TargetTotal=@TargetSubtotal-@TargetDiscount;
    UPDATE dbo.Orders SET Subtotal=@TargetSubtotal,DiscountAmount=@TargetDiscount,TotalAmount=@TargetTotal WHERE OrderId=@TargetOrderId;
    UPDATE dbo.Orders SET OrderStatus='Cancelled',ClosedAt=SYSUTCDATETIME(),Subtotal=0,DiscountAmount=0,TotalAmount=0 WHERE OrderId=@SourceOrderId;
    IF @SourceTableId IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.Orders WHERE TableId=@SourceTableId AND OrderStatus='Open') UPDATE dbo.CafeTables SET TableStatus='Available' WHERE TableId=@SourceTableId;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details) VALUES(@UserId,N'MergeOrder',N'Order',CONVERT(nvarchar(80),@TargetOrderId),CONCAT(N'Merged order ',@SourceOrderId));
    COMMIT TRANSACTION;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Order_Split @SourceOrderId bigint,@TargetTableId int=NULL,@UserId int=NULL,@Lines dbo.OrderSplitLineType READONLY
AS
BEGIN
    SET NOCOUNT ON; SET XACT_ABORT ON;
    IF NOT EXISTS(SELECT 1 FROM @Lines) THROW 51029,'Select at least one order line to split.',1;
    BEGIN TRANSACTION;
    DECLARE @EmployeeId int,@CustomerId int,@PromotionId int,@OldTableId int,@NewOrderId bigint,@NewCode nvarchar(24),@OriginalTotal decimal(18,2);
    SELECT @EmployeeId=EmployeeId,@CustomerId=CustomerId,@PromotionId=PromotionId,@OldTableId=TableId,@OriginalTotal=TotalAmount
    FROM dbo.Orders WITH (UPDLOCK,HOLDLOCK) WHERE OrderId=@SourceOrderId AND OrderStatus='Open';
    IF @EmployeeId IS NULL THROW 51030,'Source order must be open.',1;
    IF EXISTS(SELECT 1 FROM dbo.Payments WHERE OrderId=@SourceOrderId) THROW 51031,'An order with payments cannot be split.',1;
    IF @PromotionId IS NOT NULL THROW 51032,'Remove the promotion before splitting this order.',1;
    IF EXISTS(SELECT 1 FROM @Lines l LEFT JOIN dbo.OrderDetails d ON d.OrderDetailId=l.OrderDetailId AND d.OrderId=@SourceOrderId WHERE d.OrderDetailId IS NULL OR l.Quantity>d.Quantity)
        THROW 51033,'Split quantities do not match the source order.',1;
    IF @TargetTableId IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.CafeTables WITH (UPDLOCK,HOLDLOCK) WHERE TableId=@TargetTableId AND IsActive=1 AND TableStatus='Available')
        THROW 51034,'Target table is not available.',1;
    SET @NewCode=CONCAT('CM',LEFT(REPLACE(CONVERT(varchar(36),NEWID()),'-',''),22));
    INSERT dbo.Orders(OrderCode,TableId,EmployeeId,CustomerId,Note) SELECT @NewCode,@TargetTableId,EmployeeId,CustomerId,Note FROM dbo.Orders WHERE OrderId=@SourceOrderId;
    SET @NewOrderId=SCOPE_IDENTITY();
    INSERT dbo.OrderDetails(OrderId,ProductId,ProductNameSnapshot,UnitPrice,Quantity,LineDiscount)
    SELECT @NewOrderId,d.ProductId,d.ProductNameSnapshot,d.UnitPrice,l.Quantity,
           CASE WHEN l.Quantity=d.Quantity THEN d.LineDiscount ELSE ROUND(d.LineDiscount*l.Quantity/d.Quantity,2) END
    FROM @Lines l JOIN dbo.OrderDetails d ON d.OrderDetailId=l.OrderDetailId AND d.OrderId=@SourceOrderId;
    UPDATE d SET Quantity=d.Quantity-l.Quantity,
        LineDiscount=d.LineDiscount-CASE WHEN l.Quantity=d.Quantity THEN d.LineDiscount ELSE ROUND(d.LineDiscount*l.Quantity/d.Quantity,2) END
    FROM dbo.OrderDetails d JOIN @Lines l ON l.OrderDetailId=d.OrderDetailId;
    DELETE dbo.OrderDetails WHERE OrderId=@SourceOrderId AND Quantity<=0;
    IF NOT EXISTS(SELECT 1 FROM dbo.OrderDetails WHERE OrderId=@SourceOrderId) THROW 51035,'Split must leave at least one item on the source order.',1;
    DECLARE @SourceTotal decimal(18,2),@TargetTotal decimal(18,2);
    SELECT @SourceTotal=SUM(LineTotal) FROM dbo.OrderDetails WHERE OrderId=@SourceOrderId;
    SELECT @TargetTotal=SUM(LineTotal) FROM dbo.OrderDetails WHERE OrderId=@NewOrderId;
    IF @SourceTotal+@TargetTotal<>@OriginalTotal THROW 51051,'Split quantities would change the invoice total because of line rounding. Choose different quantities.',1;
    UPDATE dbo.Orders SET Subtotal=(SELECT SUM(LineTotal) FROM dbo.OrderDetails WHERE OrderId=@SourceOrderId),TotalAmount=(SELECT SUM(LineTotal) FROM dbo.OrderDetails WHERE OrderId=@SourceOrderId) WHERE OrderId=@SourceOrderId;
    UPDATE dbo.Orders SET Subtotal=(SELECT SUM(LineTotal) FROM dbo.OrderDetails WHERE OrderId=@NewOrderId),TotalAmount=(SELECT SUM(LineTotal) FROM dbo.OrderDetails WHERE OrderId=@NewOrderId) WHERE OrderId=@NewOrderId;
    IF @TargetTableId IS NOT NULL UPDATE dbo.CafeTables SET TableStatus='Occupied' WHERE TableId=@TargetTableId;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details) VALUES(@UserId,N'SplitOrder',N'Order',CONVERT(nvarchar(80),@SourceOrderId),CONCAT(N'Created order ',@NewOrderId));
    COMMIT TRANSACTION;
    SELECT @NewOrderId AS NewOrderId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Order_GetAll @Start datetime2(0), @EndExclusive datetime2(0), @OrderStatus varchar(12)=NULL, @EmployeeId int=NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT o.OrderId,o.OrderCode,
           CONVERT(datetime2(0),(o.OpenedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') AS OpenedAt,
           CASE WHEN o.ClosedAt IS NULL THEN NULL ELSE CONVERT(datetime2(0),(o.ClosedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') END AS ClosedAt,
           o.OrderStatus,o.TableId,t.TableName,e.FullName AS Employee,
           o.Subtotal,o.DiscountAmount,o.TotalAmount,COALESCE(p.PaidAmount,0) AS PaidAmount
    FROM dbo.Orders o JOIN dbo.Employees e ON e.EmployeeId=o.EmployeeId LEFT JOIN dbo.CafeTables t ON t.TableId=o.TableId
    OUTER APPLY (SELECT SUM(Amount) AS PaidAmount FROM dbo.Payments WHERE OrderId=o.OrderId) p
    WHERE o.OpenedAt>=@Start AND o.OpenedAt<@EndExclusive AND (@OrderStatus IS NULL OR o.OrderStatus=@OrderStatus)
      AND (@EmployeeId IS NULL OR o.EmployeeId=@EmployeeId)
    ORDER BY o.OpenedAt DESC;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Order_GetDetails @OrderId bigint
AS
BEGIN SET NOCOUNT ON; SELECT OrderDetailId, ProductId, ProductNameSnapshot, UnitPrice, Quantity, LineDiscount, LineTotal FROM dbo.OrderDetails WHERE OrderId=@OrderId ORDER BY OrderDetailId; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Order_GetHeader @OrderId bigint
AS
BEGIN SET NOCOUNT ON; SELECT o.OrderId,o.OrderCode,o.TotalAmount,o.OrderStatus,COALESCE(p.PaidAmount,0) AS PaidAmount FROM dbo.Orders o OUTER APPLY(SELECT SUM(Amount) AS PaidAmount FROM dbo.Payments WHERE OrderId=o.OrderId) p WHERE o.OrderId=@OrderId; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Payment_Collect @OrderId bigint, @PaymentMethod varchar(16), @Amount decimal(18,2), @ReceivedByUserId int = NULL, @ReferenceCode nvarchar(100) = NULL, @TenderedAmount decimal(18,2)=NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;
    DECLARE @Total decimal(18,2), @Paid decimal(18,2), @TableId int;
    SELECT @Total=TotalAmount, @TableId=TableId FROM dbo.Orders WITH (UPDLOCK, HOLDLOCK) WHERE OrderId=@OrderId AND OrderStatus='Open';
    IF @Total IS NULL THROW 51010, 'Order is not open or does not exist.', 1;
    SELECT @Paid=COALESCE(SUM(Amount),0) FROM dbo.Payments WITH (UPDLOCK, HOLDLOCK) WHERE OrderId=@OrderId;
    IF @Amount <= 0 OR @Paid + @Amount > @Total THROW 51011, 'Payment amount is invalid.', 1;
    IF @PaymentMethod='Cash' AND (@TenderedAmount IS NULL OR @TenderedAmount < @Amount) THROW 51015, 'Cash tendered does not cover the payment.', 1;
    IF @PaymentMethod='Transfer'
    BEGIN
        IF NULLIF(LTRIM(RTRIM(@ReferenceCode)),N'') IS NULL THROW 51042,'Transfer reference is required.',1;
        SET @TenderedAmount=NULL;
    END;
    INSERT dbo.Payments(OrderId, PaymentMethod, Amount, TenderedAmount, ChangeAmount, ReceivedByUserId, ReferenceCode)
    VALUES(@OrderId, @PaymentMethod, @Amount, @TenderedAmount, CASE WHEN @PaymentMethod='Cash' THEN @TenderedAmount-@Amount ELSE 0 END, @ReceivedByUserId, @ReferenceCode);
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId,Details) VALUES(@ReceivedByUserId,N'CollectPayment',N'Order',CONVERT(nvarchar(80),@OrderId),CONCAT(@PaymentMethod,N': ',CONVERT(nvarchar(40),@Amount)));
    IF @Paid + @Amount = @Total
    BEGIN
        DECLARE @OrderCode nvarchar(24);
        SELECT @OrderCode=OrderCode FROM dbo.Orders WHERE OrderId=@OrderId;
        -- Lock recipe ingredients in a stable order before checking and posting stock issues.
        DECLARE @LockedIngredientCount int;
        SELECT @LockedIngredientCount=COUNT(DISTINCT i.IngredientId)
        FROM dbo.Ingredients i WITH (UPDLOCK,HOLDLOCK)
        JOIN dbo.Recipes r ON r.IngredientId=i.IngredientId
        JOIN dbo.OrderDetails d ON d.ProductId=r.ProductId
        WHERE d.OrderId=@OrderId;
        IF EXISTS
        (
            SELECT 1
            FROM dbo.Recipes r
            JOIN dbo.OrderDetails d ON d.ProductId=r.ProductId
            JOIN dbo.Ingredients i ON i.IngredientId=r.IngredientId
            OUTER APPLY
            (
                SELECT COALESCE(SUM(CASE WHEN t.TransactionType='Receipt' THEN t.Quantity WHEN t.TransactionType='Issue' THEN -t.Quantity ELSE t.Quantity END),0) AS OnHand
                FROM dbo.InventoryTransactions t WHERE t.IngredientId=i.IngredientId
            ) stock
            WHERE d.OrderId=@OrderId
            GROUP BY i.IngredientId,stock.OnHand
            HAVING SUM(d.Quantity*r.QuantityRequired)>stock.OnHand
        ) THROW 51039,'Insufficient inventory for this order.',1;
        INSERT dbo.InventoryTransactions(IngredientId,UserId,TransactionType,Quantity,ReferenceCode,Note)
        SELECT r.IngredientId,@ReceivedByUserId,'Issue',SUM(d.Quantity*r.QuantityRequired),@OrderCode,N'POS recipe consumption'
        FROM dbo.OrderDetails d JOIN dbo.Recipes r ON r.ProductId=d.ProductId
        WHERE d.OrderId=@OrderId
        GROUP BY r.IngredientId;
        UPDATE dbo.Orders SET OrderStatus='Paid', ClosedAt=SYSUTCDATETIME() WHERE OrderId=@OrderId;
        IF @TableId IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.Orders WHERE TableId=@TableId AND OrderStatus='Open' AND OrderId<>@OrderId)
            UPDATE dbo.CafeTables SET TableStatus='Available' WHERE TableId=@TableId;
    END;
    COMMIT TRANSACTION;
    SELECT @Total - (@Paid + @Amount) AS BalanceDue;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Order_Cancel @OrderId bigint,@UserId int=NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF NOT EXISTS(SELECT 1 FROM dbo.Users u JOIN dbo.Roles r ON r.RoleId=u.RoleId WHERE u.UserId=@UserId AND u.IsActive=1 AND r.IsActive=1 AND r.RoleName IN(N'Admin',N'Manager'))
        THROW 51050,'Only active administrators and managers may cancel orders.',1;
    BEGIN TRANSACTION;
    DECLARE @TableId int;
    SELECT @TableId=TableId FROM dbo.Orders WITH (UPDLOCK, HOLDLOCK) WHERE OrderId=@OrderId AND OrderStatus='Open';
    IF @@ROWCOUNT=0 THROW 51012, 'Order is not open or does not exist.', 1;
    IF EXISTS(SELECT 1 FROM dbo.Payments WHERE OrderId=@OrderId) THROW 51013, 'An order with payments cannot be cancelled.', 1;
    UPDATE dbo.Orders SET OrderStatus='Cancelled', ClosedAt=SYSUTCDATETIME() WHERE OrderId=@OrderId;
    IF @TableId IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.Orders WHERE TableId=@TableId AND OrderStatus='Open' AND OrderId<>@OrderId)
        UPDATE dbo.CafeTables SET TableStatus='Available' WHERE TableId=@TableId;
    INSERT dbo.AuditLogs(UserId,ActionName,EntityName,EntityId) VALUES(@UserId,N'CancelOrder',N'Order',CONVERT(nvarchar(80),@OrderId));
    COMMIT TRANSACTION;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Dashboard_Summary @Date date
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COALESCE(SUM(TotalAmount),0) AS Revenue,
           COUNT_BIG(*) AS InvoiceCount,
           COALESCE((SELECT SUM(d.Quantity) FROM dbo.OrderDetails d JOIN dbo.Orders x ON x.OrderId=d.OrderId WHERE x.OrderStatus='Paid' AND CONVERT(date,(x.ClosedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time')=@Date),0) AS ItemsSold
    FROM dbo.Orders WHERE OrderStatus='Paid' AND CONVERT(date,(ClosedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time')=@Date;
    SELECT SUM(CASE WHEN TableStatus='Occupied' THEN 1 ELSE 0 END) AS OccupiedTables,
           SUM(CASE WHEN TableStatus='Available' THEN 1 ELSE 0 END) AS AvailableTables
    FROM dbo.CafeTables WHERE IsActive=1;
    SELECT CONVERT(date,cal.SalesDate) AS SalesDate, COALESCE(SUM(o.TotalAmount),0) AS Revenue
    FROM (SELECT DATEADD(day,-n.Number,@Date) AS SalesDate FROM (VALUES(0),(1),(2),(3),(4),(5),(6)) n(Number)) cal
    LEFT JOIN dbo.Orders o ON o.OrderStatus='Paid' AND CONVERT(date,(o.ClosedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time')=CONVERT(date,cal.SalesDate)
    GROUP BY CONVERT(date,cal.SalesDate) ORDER BY SalesDate;
    SELECT TOP (5) d.ProductNameSnapshot AS ProductName, SUM(d.Quantity) AS QuantitySold
    FROM dbo.OrderDetails d JOIN dbo.Orders o ON o.OrderId=d.OrderId
    WHERE o.OrderStatus='Paid' AND o.ClosedAt>=DATEADD(day,-30,SYSUTCDATETIME())
    GROUP BY d.ProductNameSnapshot ORDER BY QuantitySold DESC;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Report_SalesByPeriod @Start datetime2(0), @EndExclusive datetime2(0)
AS
BEGIN SET NOCOUNT ON; SELECT CONVERT(date,(ClosedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') AS SalesDate, COUNT_BIG(*) AS InvoiceCount, SUM(TotalAmount) AS Revenue FROM dbo.Orders WHERE OrderStatus='Paid' AND ClosedAt>=@Start AND ClosedAt<@EndExclusive GROUP BY CONVERT(date,(ClosedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') ORDER BY SalesDate; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Report_BestSellers @Start datetime2(0), @EndExclusive datetime2(0)
AS
BEGIN SET NOCOUNT ON; SELECT d.ProductNameSnapshot AS ProductName, SUM(d.Quantity) AS QuantitySold, SUM(d.LineTotal) AS Revenue FROM dbo.OrderDetails d JOIN dbo.Orders o ON o.OrderId=d.OrderId WHERE o.OrderStatus='Paid' AND o.ClosedAt>=@Start AND o.ClosedAt<@EndExclusive GROUP BY d.ProductNameSnapshot ORDER BY QuantitySold DESC; END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Report_SalesByEmployee @Start datetime2(0), @EndExclusive datetime2(0)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT e.EmployeeCode, e.FullName, COUNT_BIG(*) AS InvoiceCount, SUM(o.TotalAmount) AS Revenue
    FROM dbo.Orders o JOIN dbo.Employees e ON e.EmployeeId=o.EmployeeId
    WHERE o.OrderStatus='Paid' AND o.ClosedAt>=@Start AND o.ClosedAt<@EndExclusive
    GROUP BY e.EmployeeCode,e.FullName ORDER BY Revenue DESC;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Report_SalesByMonth @Year int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT YEAR(LocalClosedAt) AS SalesYear, MONTH(LocalClosedAt) AS SalesMonth, COUNT_BIG(*) AS InvoiceCount, SUM(TotalAmount) AS Revenue
    FROM (SELECT TotalAmount, CONVERT(datetime2(0),(ClosedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') AS LocalClosedAt FROM dbo.Orders WHERE OrderStatus='Paid') s
    WHERE YEAR(LocalClosedAt)=@Year
    GROUP BY YEAR(LocalClosedAt),MONTH(LocalClosedAt) ORDER BY SalesMonth;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Report_SalesByMonthPeriod @Start datetime2(0), @EndExclusive datetime2(0)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT YEAR(LocalClosedAt) AS SalesYear, MONTH(LocalClosedAt) AS SalesMonth, COUNT_BIG(*) AS InvoiceCount, SUM(TotalAmount) AS Revenue
    FROM (SELECT TotalAmount, CONVERT(datetime2(0),(ClosedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') AS LocalClosedAt FROM dbo.Orders WHERE OrderStatus='Paid' AND ClosedAt>=@Start AND ClosedAt<@EndExclusive) s
    GROUP BY YEAR(LocalClosedAt),MONTH(LocalClosedAt) ORDER BY SalesYear,SalesMonth;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Report_SalesByYear @Start datetime2(0), @EndExclusive datetime2(0)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT YEAR(LocalClosedAt) AS SalesYear, COUNT_BIG(*) AS InvoiceCount, SUM(TotalAmount) AS Revenue
    FROM (SELECT TotalAmount, CONVERT(datetime2(0),(ClosedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') AS LocalClosedAt FROM dbo.Orders WHERE OrderStatus='Paid' AND ClosedAt>=@Start AND ClosedAt<@EndExclusive) s
    GROUP BY YEAR(LocalClosedAt) ORDER BY SalesYear;
END;
GO
CREATE OR ALTER PROCEDURE dbo.sp_Report_OrderInvoices @Start datetime2(0), @EndExclusive datetime2(0)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT o.OrderCode,
           CONVERT(datetime2(0),(o.OpenedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') AS OpenedAt,
           CASE WHEN o.ClosedAt IS NULL THEN NULL ELSE CONVERT(datetime2(0),(o.ClosedAt AT TIME ZONE 'UTC') AT TIME ZONE 'SE Asia Standard Time') END AS ClosedAt,
           e.FullName AS Employee, t.TableName, o.OrderStatus, o.Subtotal, o.DiscountAmount, o.TotalAmount
    FROM dbo.Orders o JOIN dbo.Employees e ON e.EmployeeId=o.EmployeeId LEFT JOIN dbo.CafeTables t ON t.TableId=o.TableId
    WHERE o.OpenedAt>=@Start AND o.OpenedAt<@EndExclusive ORDER BY o.OpenedAt DESC;
END;
GO
