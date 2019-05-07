ALTER TABLE [Dish].[CustomizationCategories] ADD [RestaurantCategoryId] nvarchar(max) NULL;

GO

ALTER TABLE [Dish].[CustomizationCategories] ADD [RestaurantId] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190122102940_AddRestaurantIdAndCategoryIdToCustomizationCategory', N'2.1.4-rtm-31024');

GO

