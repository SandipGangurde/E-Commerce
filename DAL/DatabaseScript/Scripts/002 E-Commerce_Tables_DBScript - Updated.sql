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

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Products')
BEGIN
    CREATE TABLE dbo.Products (
        ProductId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        ProductName VARCHAR(100) NOT NULL,
        ProductDescription VARCHAR(MAX),
        ProductPrice DECIMAL(10,2) NOT NULL,
        StockQuantity INT NOT NULL DEFAULT 0,
        CategoryId BIGINT,
        DiscountId BIGINT NULL, -- New column for DiscountId, allowing NULL values
        IsActive BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId) ON DELETE CASCADE,
        CONSTRAINT FK_Products_Discounts FOREIGN KEY (DiscountId) REFERENCES Discounts(DiscountId) -- Add foreign key constraint to Discounts table
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
        DiscountApplied DECIMAL(10,2) NULL, -- Discount applied to this order detail
        TotalPrice DECIMAL(10,2) NOT NULL, -- Total price for this order detail after discount
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
VALUES ('Sandip', 'Patil', 'admin@admin.com', '9960011028', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 1),
       ('Akanksha', 'Patil', 'akanksha@admin.com', '987654321', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 1);

-- Role Table
INSERT INTO Role (RoleName, RoleDescription, IsRoleActive)
VALUES ('Admin', 'Administrator role with full access', 1),
       ('User', 'Regular user role with limited access', 1);

-- User Role Mapping Table
INSERT INTO UserRole (UserId, RoleId)
VALUES (1, 1), -- John is Admin
       (2, 2); -- Jane is User



---E-Commerce Tables Data ----
-- Adding Records to Categories Table
INSERT INTO Categories (Category, IsActive)
VALUES 
    ('Electronics', 1),
    ('Clothing', 1),
    ('Books', 1),
    ('Home & Kitchen', 1),
    ('Sports & Outdoors', 1);

-- Adding Records to Discounts Table
INSERT INTO Discounts (DiscountCode, DiscountDescription, DiscountType, DiscountValue, StartDate, EndDate, IsActive)
VALUES 
    ('SALE10', '10% off on all items', 'Percentage', 10.00, '2024-04-12', '2024-04-30', 1),
    ('FREESHIP', 'Free shipping on orders above $50', 'Free Shipping', 0.00, '2024-04-12', '2024-05-31', 1),
    ('FLASH20', 'Flash sale! Get 20% off', 'Percentage', 20.00, '2024-04-15', '2024-04-18', 1),
    ('LOYALTY5', 'Exclusive discount for loyal customers', 'Percentage', 5.00, '2024-04-01', '2024-04-30', 1),
    ('WELCOME', 'Welcome discount for new customers', 'Fixed Amount', 10.00, '2024-04-01', '2024-04-30', 1);

-- Adding Records to Products Table
INSERT INTO Products (ProductName, ProductDescription, ProductPrice, StockQuantity, CategoryId, IsActive)
VALUES 
    ('Smartphone', 'Latest smartphone with advanced features', 599.99, 100, 1, 1),
    ('T-shirt', 'Comfortable cotton t-shirt for everyday wear', 19.99, 200, 2, 1),
    ('Book: The Great Gatsby', 'Classic novel by F. Scott Fitzgerald', 9.99, 50, 3, 1),
    ('Kitchen Knife Set', 'High-quality stainless steel knife set', 49.99, 30, 4, 1),
    ('Yoga Mat', 'Eco-friendly yoga mat for workouts', 29.99, 100, 5, 1);

-- Adding Records to Orders Table
INSERT INTO Orders (UserId, TotalAmount, IsCompleted)
VALUES 
    (1, 799.98, 1),
    (2, 59.98, 1),
    (1, 129.95, 1),
    (2, 179.97, 1),
    (1, 99.99, 1);

-- Adding Records to OrderDetails Table
INSERT INTO OrderDetails (OrderId, ProductId, Quantity, UnitPrice, DiscountApplied, TotalPrice)
VALUES 
    (1, 1, 2, 599.99, 0, 1199.98),
    (2, 3, 3, 9.99, 0, 29.97),
    (3, 4, 1, 49.99, 5.00, 44.99),
    (4, 2, 4, 19.99, 0, 79.96),
    (5, 5, 1, 29.99, 10.00, 19.99);

-- Adding Records to Addresses Table
INSERT INTO Addresses (UserId, AddressLine1, City, State, Country, PostalCode, IsDefault)
VALUES 
    (1, '123 Main St', 'New York', 'NY', 'USA', '10001', 1),
    (2, '456 Elm St', 'Los Angeles', 'CA', 'USA', '90001', 1),
    (1, '789 Oak St', 'Chicago', 'IL', 'USA', '60601', 1),
    (2, '101 Pine St', 'San Francisco', 'CA', 'USA', '94101', 1),
    (1, '202 Maple St', 'Boston', 'MA', 'USA', '02101', 1);

-- Adding Records to PaymentMethods Table
INSERT INTO PaymentMethods (UserId, CardNumber, ExpiryMonth, ExpiryYear, IsDefault)
VALUES 
    (1, '1234567812345678', 12, 2026, 1),
    (2, '8765432187654321', 10, 2025, 1),
    (1, '1111222233334444', 8, 2023, 1),
    (2, '5555666677778888', 5, 2024, 1),
    (1, '9999888877776666', 2, 2027, 1);

-- Adding Records to CartItems Table
INSERT INTO CartItems (UserId, ProductId, Quantity)
VALUES 
    (1, 1, 1),
    (2, 3, 2),
    (1, 4, 1),
    (2, 2, 3),
    (1, 5, 2);

-- Adding Records to Reviews Table
INSERT INTO Reviews (ProductId, UserId, Rating, Comment, ReviewDate)
VALUES 
    (1, 1, 5, 'Great smartphone, highly recommended!', '2024-04-12'),
    (2, 2, 4, 'Nice t-shirt, comfortable fit.', '2024-04-14'),
    (3, 1, 5, 'Amazing book, a must-read!', '2024-04-16'),
    (4, 2, 4, 'Good quality knives, very sharp.', '2024-04-18'),
    (5, 1, 5, 'Love this yoga mat, excellent quality!', '2024-04-20');

-- Adding Records to Transactions Table
INSERT INTO Transactions (OrderId, TransactionDate, Amount, TransactionType)
VALUES 
    (1, '2024-04-12', 799.98, 'Payment'),
    (2, '2024-04-14', 59.98, 'Payment'),
    (3, '2024-04-16', 129.95, 'Payment'),
    (4, '2024-04-18', 179.97, 'Payment'),
    (5, '2024-04-20', 99.99, 'Payment');

-- Adding Records to Shipping Table
INSERT INTO Shipping (OrderId, ShippingMethod, ShippingAddress, TrackingNumber, ShippedDate, DeliveryDate)
VALUES 
    (1, 'Standard Shipping', '123 Main St, New York, NY, USA', '123456789', '2024-04-13', '2024-04-17'),
    (2, 'Express Shipping', '456 Elm St, Los Angeles, CA, USA', '987654321', '2024-04-15', '2024-04-18'),
    (3, 'Standard Shipping', '789 Oak St, Chicago, IL, USA', '456789123', '2024-04-17', '2024-04-21'),
    (4, 'Standard Shipping', '101 Pine St, San Francisco, CA, USA', '789123456', '2024-04-19', '2024-04-24'),
    (5, 'Express Shipping', '202 Maple St, Boston, MA, USA', '654321987', '2024-04-21', '2024-04-25');

-- Adding Records to Wishlist Table
INSERT INTO Wishlist (UserId, ProductId)
VALUES 
    (1, 3),
    (2, 5),
    (1, 2),
    (2, 4),
    (1, 1);

GO

