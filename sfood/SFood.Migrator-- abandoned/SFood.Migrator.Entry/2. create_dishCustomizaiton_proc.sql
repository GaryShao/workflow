USE [SFood-Beta];
GO

IF NOT EXISTS (
SELECT  schema_name
FROM    information_schema.schemata
WHERE   schema_name = 'Dish' ) 

BEGIN
EXEC sp_executesql N'CREATE SCHEMA Dish'
END

GO

CREATE PROCEDURE Dish.uspGetDishCustomizations  
	@DishIds nvarchar(MAX)  
AS   
	SET NOCOUNT ON; 
		
	DECLARE @Result TABLE (
		CustomizationName nvarchar(20),
		CustomizationUnitPrice money,
		CustomizationId nvarchar(32),
		DishId nvarchar(32)
	)

DECLARE @DishId nvarchar(32);

DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 

SELECT DISTINCT Item 
FROM Common.Split(@DishIds, ',')

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @DishId
WHILE @@FETCH_STATUS = 0
BEGIN 
	INSERT INTO @Result	
	SELECT [Name] AS CustomizationName, 
		   [UnitPrice] AS CustomizationUnitPrice,
		   [Id] AS CustomizationId,
		   [DishId]
	FROM Dish.Customizations
	WHERE DishId = @DishId
	FETCH NEXT FROM MY_CURSOR INTO @DishId
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR

SELECT *
FROM @Result; 

RETURN
GO 

