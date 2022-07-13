create table users(
Id int IDENTITY(1,1) primary key,
FirstName varchar(255),
LastName varchar(255),
Username varchar(255),
Email varchar(255),
Password varchar(255),
CurrencyType varchar(255),
Balance varchar(255),
DateOfBirth varchar(255))