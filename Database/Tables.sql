USE CoffeeManagementDB;
GO
CREATE TABLE dbo.Roles (
    RoleId tinyint IDENTITY(1,1) NOT NULL CONSTRAINT PK_Roles PRIMARY KEY,
    RoleName nvarchar(30) NOT NULL CONSTRAINT UQ_Roles_RoleName UNIQUE,
    Description nvarchar(200) NULL,
    IsActive bit NOT NULL CONSTRAINT DF_Roles_IsActive DEFAULT (1)
);
CREATE TABLE dbo.Employees (
    EmployeeId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Employees PRIMARY KEY,
    EmployeeCode nvarchar(20) NOT NULL CONSTRAINT UQ_Employees_Code UNIQUE,
    FullName nvarchar(120) NOT NULL,
    Phone nvarchar(20) NULL,
    Email nvarchar(254) NULL,
    JobTitle nvarchar(50) NULL,
    HiredAt date NULL,
    IsActive bit NOT NULL CONSTRAINT DF_Employees_IsActive DEFAULT (1),
    CreatedAt datetime2(0) NOT NULL CONSTRAINT DF_Employees_Created DEFAULT (SYSUTCDATETIME()),
    UpdatedAt datetime2(0) NOT NULL CONSTRAINT DF_Employees_Updated DEFAULT (SYSUTCDATETIME())
);
CREATE TABLE dbo.Users (
    UserId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
    UserName nvarchar(50) NOT NULL CONSTRAINT UQ_Users_UserName UNIQUE,
    PasswordHash nvarchar(256) NOT NULL,
    RoleId tinyint NOT NULL,
    EmployeeId int NULL,
    IsActive bit NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT (1),
    LastLoginAt datetime2(0) NULL,
    CreatedAt datetime2(0) NOT NULL CONSTRAINT DF_Users_Created DEFAULT (SYSUTCDATETIME()),
    UpdatedAt datetime2(0) NOT NULL CONSTRAINT DF_Users_Updated DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES dbo.Roles(RoleId),
    CONSTRAINT FK_Users_Employees FOREIGN KEY (EmployeeId) REFERENCES dbo.Employees(EmployeeId),
    CONSTRAINT UQ_Users_Employee UNIQUE (EmployeeId)
);
CREATE TABLE dbo.Customers (
    CustomerId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Customers PRIMARY KEY,
    FullName nvarchar(120) NOT NULL,
    Phone nvarchar(20) NULL,
    Email nvarchar(254) NULL,
    Points int NOT NULL CONSTRAINT DF_Customers_Points DEFAULT (0),
    IsActive bit NOT NULL CONSTRAINT DF_Customers_IsActive DEFAULT (1),
    CreatedAt datetime2(0) NOT NULL CONSTRAINT DF_Customers_Created DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT CK_Customers_Points CHECK (Points >= 0)
);
CREATE TABLE dbo.Categories (
    CategoryId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categories PRIMARY KEY,
    CategoryName nvarchar(80) NOT NULL CONSTRAINT UQ_Categories_Name UNIQUE,
    Description nvarchar(250) NULL,
    IsActive bit NOT NULL CONSTRAINT DF_Categories_IsActive DEFAULT (1),
    CreatedAt datetime2(0) NOT NULL CONSTRAINT DF_Categories_Created DEFAULT (SYSUTCDATETIME()),
    UpdatedAt datetime2(0) NOT NULL CONSTRAINT DF_Categories_Updated DEFAULT (SYSUTCDATETIME())
);
CREATE TABLE dbo.Products (
    ProductId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
    CategoryId int NOT NULL,
    ProductName nvarchar(120) NOT NULL,
    Description nvarchar(500) NULL,
    UnitPrice decimal(18,2) NOT NULL,
    ImagePath nvarchar(500) NULL,
    IsAvailable bit NOT NULL CONSTRAINT DF_Products_Available DEFAULT (1),
    IsActive bit NOT NULL CONSTRAINT DF_Products_Active DEFAULT (1),
    CreatedAt datetime2(0) NOT NULL CONSTRAINT DF_Products_Created DEFAULT (SYSUTCDATETIME()),
    UpdatedAt datetime2(0) NOT NULL CONSTRAINT DF_Products_Updated DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES dbo.Categories(CategoryId),
    CONSTRAINT CK_Products_UnitPrice CHECK (UnitPrice >= 0)
);
CREATE TABLE dbo.CafeTables (
    TableId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_CafeTables PRIMARY KEY,
    TableName nvarchar(40) NOT NULL CONSTRAINT UQ_CafeTables_TableName UNIQUE,
    AreaName nvarchar(60) NOT NULL CONSTRAINT DF_CafeTables_Area DEFAULT (N'Tầng 1'),
    TableStatus varchar(12) NOT NULL CONSTRAINT DF_CafeTables_Status DEFAULT ('Available'),
    IsActive bit NOT NULL CONSTRAINT DF_CafeTables_Active DEFAULT (1),
    CONSTRAINT CK_CafeTables_Status CHECK (TableStatus IN ('Available','Occupied','Reserved','Disabled'))
);
CREATE TABLE dbo.Promotions (
    PromotionId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Promotions PRIMARY KEY,
    PromotionName nvarchar(100) NOT NULL,
    DiscountPercent decimal(5,2) NOT NULL,
    StartsAt datetime2(0) NOT NULL,
    EndsAt datetime2(0) NOT NULL,
    IsActive bit NOT NULL CONSTRAINT DF_Promotions_Active DEFAULT (1),
    CONSTRAINT CK_Promotions_Percent CHECK (DiscountPercent BETWEEN 0 AND 100),
    CONSTRAINT CK_Promotions_Dates CHECK (EndsAt > StartsAt)
);
CREATE TABLE dbo.Orders (
    OrderId bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_Orders PRIMARY KEY,
    OrderCode nvarchar(24) NOT NULL CONSTRAINT UQ_Orders_Code UNIQUE,
    TableId int NULL,
    EmployeeId int NOT NULL,
    CustomerId int NULL,
    PromotionId int NULL,
    OrderStatus varchar(12) NOT NULL CONSTRAINT DF_Orders_Status DEFAULT ('Open'),
    Subtotal decimal(18,2) NOT NULL CONSTRAINT DF_Orders_Subtotal DEFAULT (0),
    DiscountAmount decimal(18,2) NOT NULL CONSTRAINT DF_Orders_Discount DEFAULT (0),
    TotalAmount decimal(18,2) NOT NULL CONSTRAINT DF_Orders_Total DEFAULT (0),
    Note nvarchar(500) NULL,
    OpenedAt datetime2(0) NOT NULL CONSTRAINT DF_Orders_Opened DEFAULT (SYSUTCDATETIME()),
    ClosedAt datetime2(0) NULL,
    CONSTRAINT FK_Orders_Tables FOREIGN KEY (TableId) REFERENCES dbo.CafeTables(TableId),
    CONSTRAINT FK_Orders_Employees FOREIGN KEY (EmployeeId) REFERENCES dbo.Employees(EmployeeId),
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId) REFERENCES dbo.Customers(CustomerId),
    CONSTRAINT FK_Orders_Promotions FOREIGN KEY (PromotionId) REFERENCES dbo.Promotions(PromotionId),
    CONSTRAINT CK_Orders_Status CHECK (OrderStatus IN ('Open','Paid','Cancelled')),
    CONSTRAINT CK_Orders_Amounts CHECK (Subtotal >= 0 AND DiscountAmount >= 0 AND TotalAmount >= 0 AND DiscountAmount <= Subtotal AND TotalAmount = Subtotal - DiscountAmount)
);
CREATE TABLE dbo.OrderDetails (
    OrderDetailId bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_OrderDetails PRIMARY KEY,
    OrderId bigint NOT NULL,
    ProductId int NOT NULL,
    ProductNameSnapshot nvarchar(120) NOT NULL,
    UnitPrice decimal(18,2) NOT NULL,
    Quantity decimal(12,3) NOT NULL,
    LineDiscount decimal(18,2) NOT NULL CONSTRAINT DF_OrderDetails_Discount DEFAULT (0),
    LineTotal AS CONVERT(decimal(18,2), UnitPrice * Quantity - LineDiscount) PERSISTED,
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderId) REFERENCES dbo.Orders(OrderId),
    CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductId) REFERENCES dbo.Products(ProductId),
    CONSTRAINT CK_OrderDetails_Values CHECK (UnitPrice >= 0 AND Quantity > 0 AND LineDiscount >= 0 AND LineDiscount <= UnitPrice * Quantity)
);
CREATE TABLE dbo.Payments (
    PaymentId bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_Payments PRIMARY KEY,
    OrderId bigint NOT NULL,
    PaymentMethod varchar(16) NOT NULL,
    Amount decimal(18,2) NOT NULL,
    TenderedAmount decimal(18,2) NULL,
    ChangeAmount decimal(18,2) NOT NULL CONSTRAINT DF_Payments_Change DEFAULT (0),
    ReferenceCode nvarchar(100) NULL,
    PaidAt datetime2(0) NOT NULL CONSTRAINT DF_Payments_PaidAt DEFAULT (SYSUTCDATETIME()),
    ReceivedByUserId int NULL,
    CONSTRAINT FK_Payments_Orders FOREIGN KEY (OrderId) REFERENCES dbo.Orders(OrderId),
    CONSTRAINT FK_Payments_Users FOREIGN KEY (ReceivedByUserId) REFERENCES dbo.Users(UserId),
    CONSTRAINT CK_Payments_Method CHECK (PaymentMethod IN ('Cash','Transfer')),
    CONSTRAINT CK_Payments_Amount CHECK (Amount > 0),
    CONSTRAINT CK_Payments_Tender CHECK ((PaymentMethod='Cash' AND TenderedAmount IS NOT NULL AND TenderedAmount >= Amount AND ChangeAmount >= 0 AND ChangeAmount = TenderedAmount-Amount) OR (PaymentMethod='Transfer' AND TenderedAmount IS NULL AND ChangeAmount=0))
);
CREATE TABLE dbo.Units (
    UnitId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Units PRIMARY KEY,
    UnitName nvarchar(40) NOT NULL CONSTRAINT UQ_Units_Name UNIQUE,
    Abbreviation nvarchar(12) NOT NULL CONSTRAINT UQ_Units_Abbreviation UNIQUE
);
CREATE TABLE dbo.Ingredients (
    IngredientId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Ingredients PRIMARY KEY,
    UnitId int NOT NULL,
    IngredientName nvarchar(120) NOT NULL CONSTRAINT UQ_Ingredients_Name UNIQUE,
    ReorderLevel decimal(18,3) NOT NULL CONSTRAINT DF_Ingredients_Reorder DEFAULT (0),
    IsActive bit NOT NULL CONSTRAINT DF_Ingredients_Active DEFAULT (1),
    CreatedAt datetime2(0) NOT NULL CONSTRAINT DF_Ingredients_Created DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_Ingredients_Units FOREIGN KEY (UnitId) REFERENCES dbo.Units(UnitId),
    CONSTRAINT CK_Ingredients_Reorder CHECK (ReorderLevel >= 0)
);
CREATE TABLE dbo.Recipes (
    ProductId int NOT NULL,
    IngredientId int NOT NULL,
    QuantityRequired decimal(18,3) NOT NULL,
    CONSTRAINT PK_Recipes PRIMARY KEY (ProductId, IngredientId),
    CONSTRAINT FK_Recipes_Products FOREIGN KEY (ProductId) REFERENCES dbo.Products(ProductId),
    CONSTRAINT FK_Recipes_Ingredients FOREIGN KEY (IngredientId) REFERENCES dbo.Ingredients(IngredientId),
    CONSTRAINT CK_Recipes_Quantity CHECK (QuantityRequired > 0)
);
CREATE TABLE dbo.InventoryTransactions (
    InventoryTransactionId bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_InventoryTransactions PRIMARY KEY,
    IngredientId int NOT NULL,
    UserId int NULL,
    TransactionType varchar(12) NOT NULL,
    Quantity decimal(18,3) NOT NULL,
    UnitCost decimal(18,2) NULL,
    ReferenceCode nvarchar(50) NULL,
    Note nvarchar(300) NULL,
    CreatedAt datetime2(0) NOT NULL CONSTRAINT DF_InventoryTransactions_Created DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_InventoryTransactions_Ingredients FOREIGN KEY (IngredientId) REFERENCES dbo.Ingredients(IngredientId),
    CONSTRAINT FK_InventoryTransactions_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(UserId),
    CONSTRAINT CK_InventoryTransactions_Type CHECK (TransactionType IN ('Receipt','Issue','Adjustment')),
    CONSTRAINT CK_InventoryTransactions_Quantity CHECK (Quantity > 0),
    CONSTRAINT CK_InventoryTransactions_Cost CHECK (UnitCost IS NULL OR UnitCost >= 0)
);
CREATE TABLE dbo.AuditLogs (
    AuditLogId bigint IDENTITY(1,1) NOT NULL CONSTRAINT PK_AuditLogs PRIMARY KEY,
    UserId int NULL,
    ActionName nvarchar(80) NOT NULL,
    EntityName nvarchar(80) NOT NULL,
    EntityId nvarchar(80) NULL,
    Details nvarchar(1000) NULL,
    CreatedAt datetime2(0) NOT NULL CONSTRAINT DF_AuditLogs_Created DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_AuditLogs_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(UserId)
);
GO
CREATE TYPE dbo.OrderLineType AS TABLE (
    ProductId int NOT NULL PRIMARY KEY,
    Quantity decimal(12,3) NOT NULL CHECK (Quantity > 0)
);
GO
CREATE TYPE dbo.OrderSplitLineType AS TABLE (
    OrderDetailId bigint NOT NULL PRIMARY KEY,
    Quantity decimal(12,3) NOT NULL CHECK (Quantity > 0)
);
GO
