invigilator
__________
username=sandra
password =sandra123

for student
________________
the username is rollno itself
and password is name+123
eg:for Ann
its username=10003
password=ann123








database code 





create database studentRecord
use studentRecord


CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL, -- Plain text password
    Role NVARCHAR(20) CHECK (Role IN ('Invigilator', 'Student'))
);
ALTER PROCEDURE [dbo].[sp_AddStudentRecord]
    @Name NVARCHAR(30),
    @Maths INT,
    @Physics INT,
    @Chemistry INT,
    @English INT,
    @Programming INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NextRoll INT = (SELECT ISNULL(MAX(RollNumber), 9999) + 1 FROM StudentRecords);

    -- Insert into StudentRecords
    INSERT INTO StudentRecords (RollNumber, Name, Maths, Physics, Chemistry, English, Programming)
    VALUES (@NextRoll, @Name, @Maths, @Physics, @Chemistry, @English, @Programming);

    -- Insert into Users table
    INSERT INTO Users (Username, Password, Role)
    VALUES (CAST(@NextRoll AS NVARCHAR(10)), @Name + '123', 'Student');
END

-- StudentRecords table
CREATE TABLE StudentRecords (
    Id INT PRIMARY KEY IDENTITY,
    RollNumber INT UNIQUE NOT NULL,
    Name NVARCHAR(30) NOT NULL,
    Maths INT CHECK (Maths BETWEEN 1 AND 100),
    Physics INT CHECK (Physics BETWEEN 1 AND 100),
    Chemistry INT CHECK (Chemistry BETWEEN 1 AND 100),
    English INT CHECK (English BETWEEN 1 AND 100),
    Programming INT CHECK (Programming BETWEEN 1 AND 100),
    IsActive BIT DEFAULT 1
);

-- Add student
CREATE PROCEDURE sp_AddStudentRecord
    @Name NVARCHAR(30),
    @Maths INT,
    @Physics INT,
    @Chemistry INT,
    @English INT,
    @Programming INT
AS
BEGIN
    DECLARE @NextRoll INT = (SELECT ISNULL(MAX(RollNumber), 9999) + 1 FROM StudentRecords)
    INSERT INTO StudentRecords (RollNumber, Name, Maths, Physics, Chemistry, English, Programming)
    VALUES (@NextRoll, @Name, @Maths, @Physics, @Chemistry, @English, @Programming)
END

-- Get all active students
CREATE PROCEDURE sp_GetAllStudentRecords
AS
BEGIN
    SELECT * FROM StudentRecords WHERE IsActive = 1
END

-- Get student by roll number
CREATE PROCEDURE sp_GetStudentRecordByRoll
    @RollNumber INT
AS
BEGIN
    SELECT * FROM StudentRecords WHERE RollNumber = @RollNumber AND IsActive = 1
END

-- Update student marks
CREATE PROCEDURE sp_UpdateStudentRecord
    @RollNumber INT,
    @Maths INT,
    @Physics INT,
    @Chemistry INT,
    @English INT,
    @Programming INT
AS
BEGIN
    UPDATE StudentRecords
    SET Maths = @Maths,
        Physics = @Physics,
        Chemistry = @Chemistry,
        English = @English,
        Programming = @Programming
    WHERE RollNumber = @RollNumber AND IsActive = 1
END

-- Delete student (soft delete)
CREATE PROCEDURE sp_DeleteStudentRecord
    @RollNumber INT
AS
BEGIN
    UPDATE StudentRecords SET IsActive = 0 WHERE RollNumber = @RollNumber
END

-- Get student by user ID (if mapped)
CREATE PROCEDURE sp_GetStudentRecordByUserId
    @UserId INT
AS
BEGIN
    DECLARE @Roll INT = (SELECT CAST(Username AS INT) FROM Users WHERE UserId = @UserId)
    SELECT * FROM StudentRecords WHERE RollNumber = @Roll AND IsActive = 1
END

INSERT INTO Users (Username, Password, Role)
VALUES 
('sandra', 'sandra123', 'Invigilator'),
('kavya', 'kavya123', 'Invigilator');

-- Students
INSERT INTO Users (Username, Password, Role)
VALUES 
('10001', 'sam123', 'Student'),
('10002', 'ram123', 'Student'),
('10003', 'ann123', 'Student');

SET IDENTITY_INSERT StudentRecords ON;

INSERT INTO StudentRecords (Id, RollNumber, Name, Maths, Physics, Chemistry, English, Programming, IsActive)
VALUES
(1, 10001, 'Sam', 85, 78, 88, 92, 90, 1),
(2, 10002, 'Ram', 72, 69, 70, 75, 68, 1),
(3, 10003, 'Ann', 90, 85, 80, 88, 95, 1);

SET IDENTITY_INSERT StudentRecords OFF;
