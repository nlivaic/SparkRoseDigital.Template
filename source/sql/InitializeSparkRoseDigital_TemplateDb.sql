CREATE DATABASE SparkRoseDigital_TemplateDb;
GO  

USE SparkRoseDigital_TemplateDb;
GO  

CREATE LOGIN SparkRoseDigital_TemplateDb_Login
    WITH PASSWORD = 'Pa$$w0rd_1337';
GO  

CREATE USER SparkRoseDigital_TemplateDb_User FOR LOGIN SparkRoseDigital_TemplateDb_Login;
GO

EXEC sp_addrolemember N'db_datareader', N'SparkRoseDigital_TemplateDb_User';
GO

EXEC sp_addrolemember N'db_datawriter', N'SparkRoseDigital_TemplateDb_User';
GO

EXEC sp_addrolemember N'db_ddladmin', N'SparkRoseDigital_TemplateDb_User';
GO
