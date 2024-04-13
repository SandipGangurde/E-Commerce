

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_ProductsByProductId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_ProductsByProductId
GO
CREATE PROCEDURE SP_ProductsByProductId 
@ProductId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT ProductId, ProductName, ProductDescription, ProductPrice, StockQuantity, CategoryId, IsActive 
FROM Products WITH(NOLOCK) WHERE ProductId = @ProductId
END


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_OrdersByOrderId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_OrdersByOrderId
GO
CREATE PROCEDURE SP_OrdersByOrderId 
@OrderId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT OrderId, UserId, OrderDate, TotalAmount, IsCompleted 
FROM Orders WITH(NOLOCK) WHERE OrderId = @OrderId
END


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_OrderDetailsById]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_OrderDetailsById
GO
CREATE PROCEDURE SP_OrderDetailsById 
@OrderDetailId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT OrderDetailId, OrderId, ProductId, Quantity, UnitPrice 
FROM OrderDetails WITH(NOLOCK) WHERE OrderDetailId = @OrderDetailId
END


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_AddressesByAddressId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_AddressesByAddressId
GO
CREATE PROCEDURE SP_AddressesByAddressId 
@AddressId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT AddressId, UserId, AddressLine1, AddressLine2, City, State, Country, PostalCode, IsDefault 
FROM Addresses WITH(NOLOCK) WHERE AddressId = @AddressId
END


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_CartItemsByCartItemId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_CartItemsByCartItemId
GO
CREATE PROCEDURE SP_CartItemsByCartItemId 
@CartItemId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT CartItemId, UserId, ProductId, Quantity 
FROM CartItems WITH(NOLOCK) WHERE CartItemId = @CartItemId
END



IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_PaymentMethodsByPaymentMethodId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_PaymentMethodsByPaymentMethodId
GO
CREATE PROCEDURE SP_PaymentMethodsByPaymentMethodId 
@PaymentMethodId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT PaymentMethodId, UserId, CardNumber, ExpiryMonth, ExpiryYear, IsDefault 
FROM PaymentMethods WITH(NOLOCK) WHERE PaymentMethodId = @PaymentMethodId
END




IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_ReviewsByReviewId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_ReviewsByReviewId
GO
CREATE PROCEDURE SP_ReviewsByReviewId 
@ReviewId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT ReviewId, ProductId, UserId, Rating, Comment, ReviewDate 
FROM Reviews WITH(NOLOCK) WHERE ReviewId = @ReviewId
END



IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_DiscountsByDiscountsId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_DiscountsByDiscountsId
GO
CREATE PROCEDURE SP_DiscountsByDiscountsId 
@DiscountId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT DiscountId, DiscountCode, DiscountDescription, DiscountType, DiscountValue, StartDate, EndDate, IsActive 
FROM Discounts WITH(NOLOCK) WHERE DiscountId = @DiscountId
END



IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_ShippingByShippingId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_ShippingByShippingId
GO
CREATE PROCEDURE SP_ShippingByShippingId 
@ShippingId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT ShippingId, OrderId, ShippingMethod, ShippingAddress, TrackingNumber, ShippedDate, DeliveryDate 
FROM Shipping WITH(NOLOCK) WHERE ShippingId = @ShippingId
END



IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_TransactionsByTransactionId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_TransactionsByTransactionId
GO
CREATE PROCEDURE SP_TransactionsByTransactionId 
@TransactionId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT TransactionId, OrderId, TransactionDate, Amount, TransactionType 
FROM Transactions WITH(NOLOCK) WHERE TransactionId = @TransactionId
END




IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_WishlistByWishlistId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_WishlistByWishlistId
GO
CREATE PROCEDURE SP_WishlistByWishlistId 
@WishlistId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT WishlistId, UserId, ProductId 
FROM Wishlist WITH(NOLOCK) WHERE WishlistId = @WishlistId
END

--================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_UserByUserId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_UserByUserId
GO
CREATE PROCEDURE SP_UserByUserId 
@UserId AS INT
AS
BEGIN
SET NOCOUNT ON
SELECT UserId, FirstName, LastName, Email, Phone, PasswordHash, IsUserActive 
FROM Users WITH(NOLOCK) WHERE UserId = @UserId
END

----------

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_ImageByImageId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_ImageByImageId
GO
CREATE PROCEDURE SP_ImageByImageId 
@ImageId AS INT
AS
BEGIN
SET NOCOUNT ON
SELECT ImageId, FileName, TableName, RecordId, FilePath 
FROM Images WITH(NOLOCK) WHERE ImageId = @ImageId
END


----------------------------
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_DeleteImage]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_DeleteImage
GO
CREATE PROCEDURE SP_DeleteImage 
@ImageId AS INT
AS
BEGIN
SET NOCOUNT ON
DELETE FROM Images WHERE ImageId = @ImageId
END


--===================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_RoleByRoleId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_RoleByRoleId
GO
CREATE PROCEDURE SP_RoleByRoleId 
@RoleId AS INT
AS
BEGIN
SET NOCOUNT ON
SELECT RoleId, RoleName, RoleDescription 
FROM Role WITH(NOLOCK) WHERE RoleId = @RoleId
END


--===================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_DeleteRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_DeleteRole
GO
CREATE PROCEDURE SP_DeleteRole 
@RoleId AS INT
AS
BEGIN
SET NOCOUNT ON
DELETE FROM Role WHERE RoleId = @RoleId
END


----------
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_DeleteUserRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_DeleteUserRole
GO
CREATE PROCEDURE SP_DeleteUserRole 
@UserRoleId AS INT
AS
BEGIN
SET NOCOUNT ON
DELETE FROM UserRole WHERE UserRoleId = @UserRoleId
END



IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_UserDetailByEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_UserDetailByEmail
GO
CREATE PROCEDURE SP_UserDetailByEmail
    @Email NVARCHAR(255)
AS
BEGIN
    SELECT 
       *
    FROM 
        VuUserDetails 
    WHERE 
        Email = @Email;
END;

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_DeleteCartItemByCartItemId]') AND type in (N'P', N'PC'))
    DROP PROCEDURE SP_DeleteCartItemByCartItemId
GO

CREATE PROCEDURE SP_DeleteCartItemByCartItemId 
    @CartItemId AS INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM CartItems WHERE CartItemId = @CartItemId;
END
