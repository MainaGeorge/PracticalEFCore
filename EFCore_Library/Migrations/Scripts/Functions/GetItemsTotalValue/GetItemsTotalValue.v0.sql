CREATE OR ALTER FUNCTION dbo.GetItemsTotalValue(@IsActive BIT)

RETURNS TABLE
AS 
RETURN 
(
	SELECT Id, [Name], [Description], Quantity, PurchasePrice, Quantity * PurchasePrice AS TotalValue
	FROM dbo.[Items]
	WHERE IsActive = @IsActive
)
