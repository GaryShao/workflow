DROP INDEX [IX_Customizations_DishId] ON [Dish].[Customizations];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Dish].[Customizations]') AND [c].[name] = N'DishId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Dish].[Customizations] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Dish].[Customizations] DROP COLUMN [DishId];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190129065724_RemoveDishIdFromCustomization', N'2.1.4-rtm-31024');

GO

