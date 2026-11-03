CREATE DATABASE mango_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE mango_db;

CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Phone VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(50) NOT NULL,
    Address VARCHAR(255),
    RegDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Role VARCHAR(20) DEFAULT 'User'
);

CREATE TABLE Products (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Category VARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    ImageUrl TEXT,
    Status VARCHAR(20) DEFAULT 'Pending',
    OwnerId INT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_product_owner FOREIGN KEY (OwnerId) REFERENCES Users(Id) ON DELETE SET NULL
);

CREATE TABLE Orders (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    TotalPrice DECIMAL(10,2) NOT NULL,
    OrderDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Status VARCHAR(50) DEFAULT 'Оплачен',
    CONSTRAINT fk_order_user FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE TABLE OrderItems (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT,
    ProductName VARCHAR(255) NOT NULL,
    Quantity INT DEFAULT 1,
    PriceAtPurchase DECIMAL(10,2) NOT NULL,
    CONSTRAINT fk_item_order FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    CONSTRAINT fk_item_product FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE SET NULL
);

INSERT INTO Users (Phone, Password, Address, Role) VALUES 
('admin', 'admin', 'Главный офис', 'Admin'),
('+79001234567', '123', 'ул. Пушкина, д. 1', 'User');

INSERT INTO Products (Name, Category, Price, ImageUrl, Status, OwnerId) VALUES 
('Смартфон', 'Электроника', 25000, 'https://via.placeholder.com/200', 'Approved', 1),
('Ноутбук', 'Электроника', 60000, 'https://via.placeholder.com/200', 'Approved', 1),
('Футболка', 'Одежда', 1500, 'https://via.placeholder.com/200', 'Approved', 1),
('Диван', 'Дом и сад', 45000, 'https://via.placeholder.com/200', 'Approved', 1),
('Кукла', 'Детские товары', 2500, 'https://via.placeholder.com/200', 'Approved', 1),
('Шины', 'Автотовары', 8000, 'https://via.placeholder.com/200', 'Approved', 1);