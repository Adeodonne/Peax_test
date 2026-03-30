IF
NOT EXISTS (
    SELECT name FROM sys.databases WHERE name = N'EmployeeDB'
)
BEGIN
    CREATE DATABASE EmployeeDB;
END
GO

USE EmployeeDB;
GO

IF NOT EXISTS (
    SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'dbo.Employee') 
      AND type = N'U'
)
BEGIN
CREATE TABLE dbo.Employee
(
    ID        INT NOT NULL,
    Name      VARCHAR(255) NOT NULL,
    ManagerID INT NULL,
    Enable    BIT NOT NULL,

    CONSTRAINT PK_Employee PRIMARY KEY (ID),

    CONSTRAINT FK_Employee_Manager
        FOREIGN KEY (ManagerID)
            REFERENCES dbo.Employee (ID)
);
END
GO