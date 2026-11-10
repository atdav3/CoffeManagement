USE master;
GO
DECLARE @BackupPath nvarchar(4000) = N'C:\SQLBackups\CoffeeManagementDB.bak'; -- Change to a folder writable by the SQL Server service account.
IF NULLIF(LTRIM(RTRIM(@BackupPath)),N'') IS NULL THROW 51101,'Backup path is required.',1;
BACKUP DATABASE CoffeeManagementDB TO DISK=@BackupPath
WITH COPY_ONLY, CHECKSUM, INIT, STATS=10;
RESTORE VERIFYONLY FROM DISK=@BackupPath WITH CHECKSUM;
GO
