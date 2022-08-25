USE master
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SparkRoseDigital_TemplateDb')
BEGIN
  CREATE DATABASE SparkRoseDigital_TemplateDb;
END;
GO

USE SparkRoseDigital_TemplateDb;
GO

IF NOT EXISTS (SELECT 1
                 FROM sys.server_principals
                WHERE [name] = N'SparkRoseDigital_TemplateDb_Login' 
                  AND [type] IN ('C','E', 'G', 'K', 'S', 'U'))
BEGIN
    CREATE LOGIN SparkRoseDigital_TemplateDb_Login
        WITH PASSWORD = '<DB_PASSWORD>';
END;
GO  

IF NOT EXISTS (select * from sys.database_principals where name = 'SparkRoseDigital_TemplateDb_User')
BEGIN
    CREATE USER SparkRoseDigital_TemplateDb_User FOR LOGIN SparkRoseDigital_TemplateDb_Login;
END;
GO  


EXEC sp_addrolemember N'db_datareader', N'SparkRoseDigital_TemplateDb_User';
GO

EXEC sp_addrolemember N'db_datawriter', N'SparkRoseDigital_TemplateDb_User';
GO

EXEC sp_addrolemember N'db_ddladmin', N'SparkRoseDigital_TemplateDb_User';
GO
