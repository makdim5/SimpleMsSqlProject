--Эти запросы можно запускать в SSMS сразу
use shop;

create table GoodsGroups(
	id INT PRIMARY KEY IDENTITY(1,1),
	name NVARCHAR(50) NOT NULL,
);

create table Goods(
	id INT PRIMARY KEY IDENTITY(1,1),
	name NVARCHAR(50) NOT NULL,
	groupId INT NULL,
	discribtion TEXT NULL,
	imageURL TEXT NULL,
	constraint FK_groupId FOREIGN KEY(groupId) REFERENCES GoodsGroups(id)
	ON DELETE set null, 
);

create table Salesmen(
	id INT PRIMARY KEY IDENTITY(1,1),
	name NVARCHAR(50) NOT NULL,
);


create table Sales(
	id INT PRIMARY KEY IDENTITY(1,1),
	time DATETIME NOT NULL,
	salesmanId INT NULL,
	constraint FK_salesmanId FOREIGN KEY(salesmanId) REFERENCES Salesmen(id)
	ON DELETE set null
);

create table SalesContent(
	id INT PRIMARY KEY IDENTITY(1,1),
	goodId INT NULL,
	saleId INT NOT NULL,
	constraint FK_goodId FOREIGN KEY(goodId) REFERENCES Goods(id) ON DELETE set null, 
	constraint FK_saleId FOREIGN KEY(saleId) REFERENCES Sales(id),
	price SMALLMONEY DEFAULT(1) NOT NULL ,
	constraint CK_emp_price check(price > 0),
	amount TINYINT DEFAULT(1) NOT NULL ,
	constraint CK_emp_amount check(amount > 0)
);
