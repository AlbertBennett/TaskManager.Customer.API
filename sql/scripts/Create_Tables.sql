CREATE TABLE `taskmanager-systemDB`.Customers (
	ID int NOT NULL PRIMARY KEY AUTO_INCREMENT,
	FirstName varchar(100) NOT NULL,
	LastName varchar(100) NOT NULL,
	Email varchar(255) NOT NULL UNIQUE,
	Country varchar(100),
	CreatedAt DATETIME DEFAULT GETDATE()
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_0900_ai_ci
COMMENT='Defines the customers of the task manager app';