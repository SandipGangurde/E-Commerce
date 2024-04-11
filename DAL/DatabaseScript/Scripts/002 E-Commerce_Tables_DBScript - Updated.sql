-- User Authentication Tables
-- Users Table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
BEGIN
    CREATE TABLE Users (
        UserId INT PRIMARY KEY IDENTITY(1,1),
		FirstName NVARCHAR(50),
        LastName NVARCHAR(50),
        Email NVARCHAR(255) NOT NULL,
        Phone NVARCHAR(15),
		PasswordHash NVARCHAR(255) NOT NULL,
        IsUserActive BIT DEFAULT 1
    );

    -- Add unique constraint to ensure unique email addresses
    ALTER TABLE Users ADD CONSTRAINT UC_Email UNIQUE (Email);
END

-- Role Table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Role')
BEGIN
    CREATE TABLE Role (
        RoleId INT PRIMARY KEY IDENTITY(1,1),
        RoleName NVARCHAR(50) NOT NULL,
        RoleDescription NVARCHAR(MAX), -- Adjust the data type and size as needed
        IsRoleActive BIT DEFAULT 1
    );

    -- Add unique constraint to ensure unique role names
    ALTER TABLE Role ADD CONSTRAINT UC_RoleName UNIQUE (RoleName);
END


-- User Role Mapping Table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserRole')
BEGIN
    CREATE TABLE UserRole (
        UserRoleId INT PRIMARY KEY IDENTITY(1,1),
        UserId INT FOREIGN KEY REFERENCES Users(UserId) ON DELETE CASCADE,
        RoleId INT FOREIGN KEY REFERENCES Role(RoleId) ON DELETE CASCADE
    );
END



-- E-commerce Tables
-- Table to store IMAGES
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Images')
BEGIN
	CREATE TABLE Images (
		ImageId INT PRIMARY KEY IDENTITY(1,1),
		[FileName] NVARCHAR(255),
		TableName NVARCHAR(50) NOT NULL,
		RecordId INT NOT NULL,
		FilePath NVARCHAR(255),
		CONSTRAINT UC_Image UNIQUE (TableName, RecordID)
	);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Categories')
BEGIN
    CREATE TABLE dbo.Categories (
        CategoryId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Category VARCHAR(100) NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Products')
BEGIN
    CREATE TABLE dbo.Products (
        ProductId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        ProductName VARCHAR(100) NOT NULL,
        ProductDescription VARCHAR(MAX),
        ProductPrice DECIMAL(10,2) NOT NULL,
        StockQuantity INT NOT NULL DEFAULT 0,
        CategoryId BIGINT,
        IsActive BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId) ON DELETE CASCADE
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Orders')
BEGIN
    CREATE TABLE dbo.Orders (
        OrderId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        UserId INT NOT NULL,
        OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
        TotalAmount DECIMAL(10,2) NOT NULL,
        IsCompleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE CASCADE
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OrderDetails')
BEGIN
    CREATE TABLE dbo.OrderDetails (
        OrderDetailId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        OrderId BIGINT NOT NULL,
        ProductId BIGINT NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(10,2) NOT NULL,
        CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE,
        CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Addresses')
BEGIN
    CREATE TABLE dbo.Addresses (
        AddressId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        UserId INT NOT NULL,
        AddressLine1 NVARCHAR(255) NOT NULL,
        AddressLine2 NVARCHAR(255),
        City NVARCHAR(100) NOT NULL,
        State NVARCHAR(100) NOT NULL,
        Country NVARCHAR(100) NOT NULL,
        PostalCode NVARCHAR(20) NOT NULL,
        IsDefault BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Addresses_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PaymentMethods')
BEGIN
    CREATE TABLE dbo.PaymentMethods (
        PaymentMethodId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        UserId INT NOT NULL,
        CardNumber NVARCHAR(20) NOT NULL,
        ExpiryMonth INT NOT NULL,
        ExpiryYear INT NOT NULL,
        IsDefault BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_PaymentMethods_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CartItems')
BEGIN
    CREATE TABLE dbo.CartItems (
        CartItemId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        UserId INT NOT NULL,
        ProductId BIGINT NOT NULL,
        Quantity INT NOT NULL,
        CONSTRAINT FK_CartItems_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
        CONSTRAINT FK_CartItems_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Reviews')
BEGIN
    CREATE TABLE dbo.Reviews (
        ReviewId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        ProductId BIGINT NOT NULL,
        UserId INT NOT NULL,
        Rating INT NOT NULL,
        Comment NVARCHAR(MAX),
        ReviewDate DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Reviews_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE,
        CONSTRAINT FK_Reviews_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Discounts')
BEGIN
    CREATE TABLE dbo.Discounts (
        DiscountId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        DiscountCode NVARCHAR(20) NOT NULL,
        DiscountDescription NVARCHAR(MAX),
        DiscountType NVARCHAR(20) NOT NULL,
        DiscountValue DECIMAL(10,2) NOT NULL,
        StartDate DATETIME NOT NULL,
        EndDate DATETIME NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Transactions')
BEGIN
    CREATE TABLE dbo.Transactions (
        TransactionId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        OrderId BIGINT,
        TransactionDate DATETIME NOT NULL DEFAULT GETDATE(),
        Amount DECIMAL(10,2) NOT NULL,
        TransactionType NVARCHAR(20) NOT NULL,
        CONSTRAINT FK_Transactions_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Shipping')
BEGIN
    CREATE TABLE dbo.Shipping (
        ShippingId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        OrderId BIGINT NOT NULL,
        ShippingMethod NVARCHAR(100) NOT NULL,
        ShippingAddress NVARCHAR(MAX) NOT NULL,
        TrackingNumber NVARCHAR(50),
        ShippedDate DATETIME,
        DeliveryDate DATETIME,
        CONSTRAINT FK_Shipping_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE
    );
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Wishlist')
BEGIN
    CREATE TABLE dbo.Wishlist (
        WishlistId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        UserId INT NOT NULL,
        ProductId BIGINT NOT NULL,
        CONSTRAINT FK_Wishlist_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
        CONSTRAINT FK_Wishlist_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
    );
END


--Adding Records
-- Users Table
INSERT INTO Users (FirstName, LastName, Email, Phone, PasswordHash, IsUserActive)
VALUES ('Sandip', 'Patil', 'admin@admin.com', '9960011028', 'password_hash_1', 1),
       ('Akanksha', 'Akanksha', 'akanksha@admin.com', '987654321', 'password_hash_2', 1);

-- Role Table
INSERT INTO Role (RoleName, RoleDescription, IsRoleActive)
VALUES ('Admin', 'Administrator role with full access', 1),
       ('User', 'Regular user role with limited access', 1);

-- User Role Mapping Table
INSERT INTO UserRole (UserId, RoleId)
VALUES (1, 1), -- John is Admin
       (2, 2); -- Jane is User

-- Insert records into Categories table
INSERT INTO Categories (Category, IsActive)
VALUES ('Electronics', 1),
       ('Clothing', 1);

-- Insert records into Products table
INSERT INTO Products (ProductName, ProductDescription, ProductPrice, StockQuantity, CategoryId, IsActive)
VALUES ('Smartphone', 'High-end smartphone with advanced features', 999.99, 100, 1, 1),
       ('T-Shirt', 'Comfortable cotton T-shirt for everyday wear', 19.99, 500, 2, 1);

-- Insert records into Orders table
INSERT INTO Orders (UserId, TotalAmount, IsCompleted)
VALUES (1, 599.98, 1), -- John placed an order
       (2, 39.98, 1);  -- Jane placed an order

-- Insert records into OrderDetails table
INSERT INTO OrderDetails (OrderId, ProductId, Quantity, UnitPrice)
VALUES (1, 1, 2, 999.99), -- John's order details
       (2, 2, 2, 19.99);  -- Jane's order details

-- Insert records into Addresses table
INSERT INTO Addresses (UserId, AddressLine1, City, State, Country, PostalCode, IsDefault)
VALUES (1, '123 Main St', 'New York', 'NY', 'USA', '10001', 1),
       (2, '456 Elm St', 'Los Angeles', 'CA', 'USA', '90001', 1);

-- Insert records into PaymentMethods table
INSERT INTO PaymentMethods (UserId, CardNumber, ExpiryMonth, ExpiryYear, IsDefault)
VALUES (1, '1234567890123456', 12, 2025, 1),
       (2, '9876543210987654', 10, 2024, 1);

-- Insert records into CartItems table
INSERT INTO CartItems (UserId, ProductId, Quantity)
VALUES (1, 1, 1), -- John's cart
       (2, 2, 3); -- Jane's cart

-- Insert records into Reviews table
INSERT INTO Reviews (ProductId, UserId, Rating, Comment)
VALUES (1, 1, 5, 'Great smartphone!'), -- John's review
       (2, 2, 4, 'Nice T-shirt, fits well.'); -- Jane's review

-- Insert records into Discounts table
INSERT INTO Discounts (DiscountCode, DiscountDescription, DiscountType, DiscountValue, StartDate, EndDate, IsActive)
VALUES ('SUMMER20', 'Summer discount 20% off', 'Percentage', 20.00, '2024-06-01', '2024-08-31', 1),
       ('FREESHIP', 'Free shipping on all orders', 'FixedAmount', 0.00, '2024-01-01', '2024-12-31', 1);

-- Insert records into Transactions table
INSERT INTO Transactions (OrderId, Amount, TransactionType)
VALUES (1, 599.98, 'Payment'), -- John's payment
       (2, 39.98, 'Payment');  -- Jane's payment

-- Insert records into Shipping table
INSERT INTO Shipping (OrderId, ShippingMethod, ShippingAddress, TrackingNumber, ShippedDate, DeliveryDate)
VALUES (1, 'Standard', '123 Main St, New York, NY, USA', '123456789', '2024-04-05', '2024-04-10'), -- John's shipping
       (2, 'Express', '456 Elm St, Los Angeles, CA, USA', '987654321', '2024-04-06', '2024-04-08');  -- Jane's shipping

-- Insert records into Wishlist table
INSERT INTO Wishlist (UserId, ProductId)
VALUES (1, 2), -- John's wishlist
       (2, 1); -- Jane's wishlist


