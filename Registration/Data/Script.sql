
CREATE DATABASE RegistrationDB

CREATE TABLE Registration_DB(
	Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	FirstName [Varchar](100) NOT NULL,
	LastName [Varchar](100) Not Null,
	DOB date NOT NULL,
	Gender [Varchar](100) NOT NULL,
	Email [Varchar](100) NOT NULL,
	Phone  [Varchar](100) NOT NULL,
	Username [Varchar](100) NOT NULL,
	Password [Varchar](100) NOT NULL,
	DepartmentId int Not Null,
	IsActive [Bit] Not Null,
	UserImage [Varchar](100) Null,
)

CREATE TABLE Department(
	DepartmentId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	DepartmentName [Varchar](100) NOT NULL
)