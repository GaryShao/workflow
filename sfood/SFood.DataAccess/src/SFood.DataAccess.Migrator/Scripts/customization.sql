EXEC sp_rename N'[Dish].[CustomizationCategories].[DishId]', N'FromId', N'COLUMN';

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Restaurant].[Menus]') AND [c].[name] = N'Name');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Restaurant].[Menus] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Restaurant].[Menus] ALTER COLUMN [Name] nvarchar(100) NULL;

GO

ALTER TABLE [Restaurant].[Menus] ADD [IsDefault] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [HawkerCenter].[Seats] ADD [CreatedTime] datetime2 NOT NULL DEFAULT (getutcdate());

GO

ALTER TABLE [HawkerCenter].[Seats] ADD [CreatedUserName] nvarchar(100) NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HawkerCenter].[SeatAreas]') AND [c].[name] = N'Name');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [HawkerCenter].[SeatAreas] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [HawkerCenter].[SeatAreas] ALTER COLUMN [Name] nvarchar(20) NULL;

GO

ALTER TABLE [HawkerCenter].[SeatAreas] ADD [CreatedUserName] nvarchar(100) NULL;

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HawkerCenter].[Banners]') AND [c].[name] = N'TargetUrl');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [HawkerCenter].[Banners] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [HawkerCenter].[Banners] ALTER COLUMN [TargetUrl] nvarchar(1000) NULL;

GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HawkerCenter].[Banners]') AND [c].[name] = N'CreatedUserName');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [HawkerCenter].[Banners] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [HawkerCenter].[Banners] ALTER COLUMN [CreatedUserName] nvarchar(100) NULL;

GO

DROP INDEX [IX_Banners_CenterId] ON [HawkerCenter].[Banners];
DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[HawkerCenter].[Banners]') AND [c].[name] = N'CenterId');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [HawkerCenter].[Banners] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [HawkerCenter].[Banners] ALTER COLUMN [CenterId] nvarchar(32) NULL;
CREATE INDEX [IX_Banners_CenterId] ON [HawkerCenter].[Banners] ([CenterId]);

GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Dish].[Customizations]') AND [c].[name] = N'Name');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Dish].[Customizations] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Dish].[Customizations] ALTER COLUMN [Name] nvarchar(100) NULL;

GO

ALTER TABLE [Dish].[Customizations] ADD [Index] tinyint NOT NULL DEFAULT CAST(0 AS tinyint);

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Dish].[CustomizationCategories]') AND [c].[name] = N'Name');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Dish].[CustomizationCategories] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Dish].[CustomizationCategories] ALTER COLUMN [Name] nvarchar(100) NULL;

GO

ALTER TABLE [Dish].[CustomizationCategories] ADD [IsMultiple] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Dish].[CustomizationCategories] ADD [IsSelected] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Dish].[CustomizationCategories] ADD [IsSystem] bit NOT NULL DEFAULT 0;

GO

CREATE TABLE [RelationShip].[Dishes&CustomizationCategories] (
    [Id] nvarchar(32) NOT NULL,
    [DishId] nvarchar(32) NULL,
    [CustomizationCategoryId] nvarchar(450) NULL,
    CONSTRAINT [PK_Dishes&CustomizationCategories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Dishes&CustomizationCategories_Dishes_DishId] FOREIGN KEY ([DishId]) REFERENCES [Restaurant].[Dishes] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Seats_SeatAreaId] ON [HawkerCenter].[Seats] ([SeatAreaId]);

GO

CREATE INDEX [IX_Dishes&CustomizationCategories_DishId_CustomizationCategoryId] ON [RelationShip].[Dishes&CustomizationCategories] ([DishId], [CustomizationCategoryId]);

GO

ALTER TABLE [HawkerCenter].[Banners] ADD CONSTRAINT [FK_Banners_BasicInfos_CenterId] FOREIGN KEY ([CenterId]) REFERENCES [HawkerCenter].[BasicInfos] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [HawkerCenter].[SeatAreas] ADD CONSTRAINT [FK_SeatAreas_BasicInfos_CenterId] FOREIGN KEY ([CenterId]) REFERENCES [HawkerCenter].[BasicInfos] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [HawkerCenter].[Seats] ADD CONSTRAINT [FK_Seats_SeatAreas_SeatAreaId] FOREIGN KEY ([SeatAreaId]) REFERENCES [HawkerCenter].[SeatAreas] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190121153034_CustomizationModuleRefactor', N'2.1.4-rtm-31024');

GO

