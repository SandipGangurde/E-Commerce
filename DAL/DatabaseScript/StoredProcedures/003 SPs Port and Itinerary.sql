
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_CategoriesByCategoryCode]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_CategoriesByCategoryCode
GO
-- =============================================
-- Author: <Author,,Sandip Patil>
-- Create date: <Create Date,,10-Jan-2024>
-- Schema Name: DBO
-- Description: <Description,, Read timeZone based on TimeZoneId>
-- History :
--
-- =============================================
CREATE PROCEDURE SP_CategoriesByCategoryCode
@CountryCode AS NVARCHAR(30)
AS
BEGIN
SET NOCOUNT ON
SELECT CategoryId,CategoryCode,Category,[Status],ModifiedBy,ModifiedDate
FROM Categories WITH(NOLOCK) WHERE CategoryCode = @CategoryCode
END
------------------------------------------------------------


GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_CountryByCountryId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_CountryByCountryId
GO
-- =============================================
-- Author: <Author,,Sandip Patil>
-- Create date: <Create Date,,10-Jan-2024>
-- Schema Name: DBO
-- Description: <Description,, Read timeZone based on TimeZoneId>
-- History :
-- =============================================

CREATE PROCEDURE SP_CountryByCountryId 
@CountryId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT CountryId,CountryCode,Country,[Status] 
FROM Countries WITH(NOLOCK) WHERE CountryId = @CountryId
END
------------------------------------------------------------

GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_CategoriesByCategoryId]') AND type in (N'P', N'PC'))
DROP PROCEDURE SP_CategoriesByCategoryId
GO
-- =============================================
-- Author: <Author,,Sandip Patil>
-- Create date: <Create Date,,10-Jan-2024>
-- Schema Name: DBO
-- Description: <Description,, Read timeZone based on TimeZoneId>
-- History :
-- =============================================

CREATE PROCEDURE SP_CategoriesByCategoryId 
@CategoryId AS BIGINT
AS
BEGIN
SET NOCOUNT ON
SELECT CategoryID,CategoryName,IsActive 
FROM Categories WITH(NOLOCK) WHERE CategoryID = @CategoryId
END
------------------------------------------------------------