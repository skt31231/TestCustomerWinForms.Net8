-- Run this in your TestGPT database if the table doesn't already exist
IF OBJECT_ID('dbo.tblTestCustomer', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblTestCustomer
    (
        CustomerID   INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        CustomerName NVARCHAR(200)     NULL
    );
END;

-- Run this to create the Categories table if it doesn't exist
IF OBJECT_ID('dbo.Categories', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categories
    (
        CategoryID   INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        CategoryName NVARCHAR(200)     NULL,
        Description  NVARCHAR(MAX)     NULL,
        Picture      VARBINARY(MAX)    NULL
    );
END;