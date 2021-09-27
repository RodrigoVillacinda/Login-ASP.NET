CREATE DATABASE Login_Register
GO

CREATE TABLE Users
(
	IDUser INT IDENTITY(1,1) PRIMARY KEY,
	name NVARCHAR(128),
	email NVARCHAR(256),
	password NVARCHAR(256),
	token NVARCHAR(800),
	token_expiration DATETIME,
	last_login DATETIME 
)

SELECT * FROM Users

drop table Users