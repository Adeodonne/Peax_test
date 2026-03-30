USE
EmployeeDB;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Employee)
BEGIN

INSERT INTO dbo.Employee (ID, Name, ManagerID, Enable)
VALUES (1, 'Andrey', NULL, 1),
       (2, 'Alexey', 1, 1),
       (3, 'Roman', 1, 1),
       (3, 'Oleh', 2, 1);

END
GO