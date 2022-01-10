CREATE OR ALTER FUNCTION dbo.GetItemsTotalValue(@IsActive BIT)

RETURNS TABLE
AS 
RETURN 
(
	SELECT Id, [Name], [Description], Quantity, PurchasedPrice, Quantity * PurchasedPrice AS TotalValue
	FROM dbo.[Items]
	WHERE IsActive = @IsActive
)
