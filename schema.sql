-- Run this in your TestGPT database if the table doesn't already exist
IF OBJECT_ID('dbo.tblTestCustomer', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblTestCustomer
    (
        CustomerID   INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        CustomerName NVARCHAR(200)     NULL
    );
END;