USE master;
GO
DECLARE @BackupPath nvarchar(4000) = N'C:\SQLBackups\CoffeeManagementDB.bak'; -- Change before execution; restore replaces current database contents.
IF NULLIF(LTRIM(RTRIM(@BackupPath)),N'') IS NULL THROW 51102,'Restore path is required.',1;
ALTER DATABASE CoffeeManagementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
BEGIN TRY
    RESTORE DATABASE CoffeeManagementDB FROM DISK=@BackupPath WITH REPLACE, CHECKSUM, RECOVERY, STATS=10;
    ALTER DATABASE CoffeeManagementDB SET MULTI_USER;
END TRY
BEGIN CATCH
    IF DB_ID(N'CoffeeManagementDB') IS NOT NULL
        ALTER DATABASE CoffeeManagementDB SET MULTI_USER;
    THROW;
END CATCH;
GO
