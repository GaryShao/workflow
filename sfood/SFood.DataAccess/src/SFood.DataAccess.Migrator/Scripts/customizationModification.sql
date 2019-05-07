ALTER TABLE [Dish].[Customizations] DROP CONSTRAINT [FK_Customizations_Dishes_DishId];

GO

ALTER TABLE [Dish].[Customizations] ADD [IsDeleted] bit NOT NULL DEFAULT 0;

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Dish].[CustomizationCategories]') AND [c].[name] = N'RestaurantId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Dish].[CustomizationCategories] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Dish].[CustomizationCategories] ALTER COLUMN [RestaurantId] nvarchar(32) NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Dish].[CustomizationCategories]') AND [c].[name] = N'RestaurantCategoryId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Dish].[CustomizationCategories] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Dish].[CustomizationCategories] ALTER COLUMN [RestaurantCategoryId] nvarchar(32) NULL;

GO

ALTER TABLE [Dish].[CustomizationCategories] ADD [Index] tinyint NOT NULL DEFAULT CAST(0 AS tinyint);

GO

ALTER TABLE [Dish].[CustomizationCategories] ADD [IsDeleted] bit NOT NULL DEFAULT 0;

GO

CREATE INDEX [IX_CustomizationCategories_RestaurantId] ON [Dish].[CustomizationCategories] ([RestaurantId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190129060934_CustomizationModification', N'2.1.4-rtm-31024');

GO

