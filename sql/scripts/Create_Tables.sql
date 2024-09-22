CREATE TABLE `taskmanager-systemDB`.CountryCodes (
	ID int NOT NULL PRIMARY KEY AUTO_INCREMENT,
	ISOCode varchar(4) NOT NULL,
	Name varchar(100) NOT NULL
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci
COMMENT='Defines the countries of where the task manager app is availible in';


CREATE TABLE `taskmanager-systemDB`.Customers (
	ID int NOT NULL PRIMARY KEY AUTO_INCREMENT,
	FirstName varchar(100) NOT NULL,
	LastName varchar(100) NOT NULL,
	Email varchar(255) NOT NULL UNIQUE,
	Country varchar(100),
	FOREIGN KEY (CountryCodeId) References CountryCodes(ID)
	SsoProvider varchar(255) NOT NULL,
	SsoId varchar(255) NOT NULL,
	CreatedAt DATETIME DEFAULT GETDATE()
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci
COMMENT='Defines the customers of the task manager app';