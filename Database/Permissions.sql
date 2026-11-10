-- Edit the Windows group below, then run as a SQL Server administrator in CoffeeManagementDB.
USE CoffeeManagementDB;
GO
DECLARE @WindowsGroup sysname = N'CONTOSO\CoffeeShopUsers'; -- Replace before execution.
IF SUSER_ID(@WindowsGroup) IS NULL THROW 51100, 'Create or select the Windows login before applying permissions.', 1;
IF DATABASE_PRINCIPAL_ID(N'CoffeeManagementAppRole') IS NULL CREATE ROLE CoffeeManagementAppRole;
DECLARE @CreateUser nvarchar(max) = N'IF DATABASE_PRINCIPAL_ID(N' + QUOTENAME(@WindowsGroup,'''') + N') IS NULL CREATE USER ' + QUOTENAME(@WindowsGroup) + N' FOR LOGIN ' + QUOTENAME(@WindowsGroup) + N';';
EXEC sys.sp_executesql @CreateUser;
DECLARE @AddRole nvarchar(max) = N'ALTER ROLE CoffeeManagementAppRole ADD MEMBER ' + QUOTENAME(@WindowsGroup) + N';';
EXEC sys.sp_executesql @AddRole;
GRANT EXECUTE ON SCHEMA::dbo TO CoffeeManagementAppRole;
GRANT EXECUTE, REFERENCES ON TYPE::dbo.OrderLineType TO CoffeeManagementAppRole;
GRANT EXECUTE, REFERENCES ON TYPE::dbo.OrderSplitLineType TO CoffeeManagementAppRole;
GO
