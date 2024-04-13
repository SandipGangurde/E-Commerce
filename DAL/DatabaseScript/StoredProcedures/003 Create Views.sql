

GO
IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[VuUserDetails]'))
    DROP VIEW VuUserDetails
GO
GO
CREATE VIEW VuUserDetails
AS
SELECT 
    u.UserId,
    u.FirstName,
    u.LastName,
    u.Email,
    u.Phone,
    u.PasswordHash,
    u.IsUserActive,
    r.RoleId,
    r.RoleName,
    r.RoleDescription
FROM 
    Users u
LEFT JOIN 
    UserRole ur ON u.UserId = ur.UserId
LEFT JOIN 
    Role r ON ur.RoleId = r.RoleId;

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[VuProductDetails]') AND type in (N'P', N'PC'))
    DROP VIEW VuProductDetails;
GO

CREATE VIEW VuProductDetails
AS
SELECT 
    p.ProductId,
    p.ProductName,
    p.ProductDescription,
    p.ProductPrice,
    p.StockQuantity,
    p.CategoryId,
    c.Category,
    p.IsActive AS ProductIsActive,
    c.IsActive AS CategoryIsActive,
    (
        SELECT AVG(Rating) FROM Reviews WHERE ProductId = p.ProductId
    ) AS AverageRating,
    (
        SELECT COUNT(*) FROM Reviews WHERE ProductId = p.ProductId
    ) AS NumberOfReviews,
    d.DiscountId,
    d.DiscountCode,
    d.DiscountDescription,
    d.DiscountType,
    d.DiscountValue,
    d.StartDate AS DiscountStartDate,
    d.EndDate AS DiscountEndDate,
    d.IsActive AS DiscountIsActive,
    -- Calculation for discountPrice
    CAST(
        CASE 
            WHEN d.DiscountId IS NOT NULL AND d.IsActive = 1 AND d.StartDate <= GETDATE() AND d.EndDate >= GETDATE() THEN
                CASE 
                    WHEN d.DiscountType = 'Percentage' THEN CAST((p.ProductPrice * (1 - d.DiscountValue / 100)) AS DECIMAL(18, 2))
                    WHEN d.DiscountType = 'FixedAmount' THEN CAST((p.ProductPrice - d.DiscountValue) AS DECIMAL(18, 2))
                    ELSE CAST(p.ProductPrice AS DECIMAL(18, 2)) -- handle other discount types if necessary
                END
            ELSE CAST(p.ProductPrice AS DECIMAL(18, 2))
        END AS DECIMAL(18, 2)) AS DiscountPrice,
    -- Calculation for discountAmount
    CAST(
        CASE 
            WHEN d.DiscountId IS NOT NULL AND d.IsActive = 1 AND d.StartDate <= GETDATE() AND d.EndDate >= GETDATE() THEN
                CASE 
                    WHEN d.DiscountType = 'Percentage' THEN CAST((p.ProductPrice * (d.DiscountValue / 100)) AS DECIMAL(18, 2))
                    WHEN d.DiscountType = 'FixedAmount' THEN CAST(d.DiscountValue AS DECIMAL(18, 2))
                    ELSE CAST(0 AS DECIMAL(18, 2)) -- handle other discount types if necessary
                END
            ELSE CAST(0 AS DECIMAL(18, 2))
        END AS DECIMAL(18, 2)) AS DiscountAmount
FROM 
    Products p
LEFT JOIN 
    Categories c ON p.CategoryId = c.CategoryId
LEFT JOIN 
    Discounts d ON p.DiscountId = d.DiscountId
WHERE 
    p.IsActive = 1 -- Product is active
    AND c.IsActive = 1; -- Category is active
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[VuCartItemDetails]') AND type in (N'P', N'PC'))
    DROP VIEW VuCartItemDetails;
GO

CREATE VIEW VuCartItemDetails
AS
SELECT 
    ci.CartItemId,
    ci.UserId,
    p.ProductId,
    p.ProductName,
    p.ProductDescription,
    p.CategoryId,
    c.Category,
    CAST(ROUND(p.ProductPrice, 2) AS NUMERIC(10,2)) AS OriginalPrice,
    p.StockQuantity AS StockQuantity,
    ci.Quantity AS CartQuantity,
    CAST(
        CASE 
            WHEN d.DiscountType = 'Percentage' AND d.StartDate <= GETDATE() AND d.EndDate >= GETDATE() THEN 
                CASE 
                    WHEN d.DiscountValue IS NOT NULL THEN
                        ROUND(p.ProductPrice * (1 - (d.DiscountValue / 100)), 2)
                END
            WHEN d.DiscountType = 'FixedAmount' AND d.StartDate <= GETDATE() AND d.EndDate >= GETDATE() THEN 
                CASE 
                    WHEN d.DiscountValue IS NOT NULL THEN
                        ROUND(p.ProductPrice - d.DiscountValue, 2)
                END
            ELSE ROUND(p.ProductPrice, 2) -- No discount
        END AS NUMERIC(10,2)
    ) AS DiscountedPrice,
    CAST(
        CASE 
            WHEN d.DiscountType = 'Percentage' AND d.StartDate <= GETDATE() AND d.EndDate >= GETDATE() THEN 
                CASE 
                    WHEN d.DiscountValue IS NOT NULL THEN
                        ROUND((p.ProductPrice * (1 - (d.DiscountValue / 100))), 2)
                END
            WHEN d.DiscountType = 'FixedAmount' AND d.StartDate <= GETDATE() AND d.EndDate >= GETDATE() THEN 
                CASE 
                    WHEN d.DiscountValue IS NOT NULL THEN
                        ROUND(d.DiscountValue, 2)
                END
            ELSE 0 -- No discount
        END AS NUMERIC(10,2)
    ) AS DiscountPrice,
    CAST(
        CASE 
            WHEN d.DiscountType = 'Percentage' AND d.StartDate <= GETDATE() AND d.EndDate >= GETDATE() THEN 
                CASE 
                    WHEN d.DiscountValue IS NOT NULL THEN
                        ROUND((p.ProductPrice * (1 - (d.DiscountValue / 100))) * ci.Quantity, 2)
                END
            WHEN d.DiscountType = 'FixedAmount' AND d.StartDate <= GETDATE() AND d.EndDate >= GETDATE() THEN 
                CASE 
                    WHEN d.DiscountValue IS NOT NULL THEN
                        ROUND(d.DiscountValue * ci.Quantity, 2)
                END
            ELSE ROUND(p.ProductPrice * ci.Quantity, 2) -- No discount
        END AS NUMERIC(10,2)
    ) AS TotalDiscountedPrice,
    d.DiscountType,
    CASE
        WHEN d.DiscountValue IS NOT NULL THEN
            CAST(ROUND(d.DiscountValue, 2) AS NUMERIC(10,2))
        ELSE NULL
    END AS DiscountAmount,
    CASE
        WHEN c.IsActive = 1 THEN CAST(1 AS BIT)
        ELSE CAST(0 AS BIT)
    END AS CategoryStatus,
    CASE
        WHEN p.IsActive = 1 THEN CAST(1 AS BIT)
        ELSE CAST(0 AS BIT)
    END AS ProductStatus
FROM 
    CartItems ci
INNER JOIN 
    Products p ON ci.ProductId = p.ProductId
LEFT JOIN 
    Discounts d ON p.DiscountId = d.DiscountId
LEFT JOIN
    Categories c ON p.CategoryId = c.CategoryId;
GO