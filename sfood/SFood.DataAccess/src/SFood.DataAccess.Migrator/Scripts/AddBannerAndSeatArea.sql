ALTER TABLE [Common].[Images] DROP CONSTRAINT [FK_Images_BasicInfos_CenterId];

GO

ALTER TABLE [OrderInfo].[Bills] DROP CONSTRAINT [FK_Bills_Archives_OrderId];

GO

DROP INDEX [IX_Images_CenterId] ON [Common].[Images];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Common].[Images]') AND [c].[name] = N'CenterId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Common].[Images] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Common].[Images] DROP COLUMN [CenterId];

GO

ALTER TABLE [HawkerCenter].[Seats] ADD [SeatAreaId] nvarchar(32) NULL;

GO

CREATE TABLE [HawkerCenter].[Banner] (
    [Id] nvarchar(32) NOT NULL,
    [CenterId] nvarchar(450) NULL,
    [TargetUrl] nvarchar(max) NULL,
    [StartAt] datetime2 NOT NULL,
    [EndAt] datetime2 NOT NULL,
    [Visit] int NOT NULL,
    [CreatedTime] datetime2 NOT NULL DEFAULT (getutcdate()),
    [CreatedUserName] nvarchar(max) NULL,
    CONSTRAINT [PK_Banner] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [HawkerCenter].[SeatAreas] (
    [Id] nvarchar(32) NOT NULL,
    [CenterId] nvarchar(32) NULL,
    [Name] nvarchar(32) NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedTime] datetime2 NOT NULL DEFAULT (getutcdate()),
    CONSTRAINT [PK_SeatAreas] PRIMARY KEY ([Id])
);

GO

CREATE INDEX [IX_Banner_CenterId] ON [HawkerCenter].[Banner] ([CenterId]);

GO

CREATE INDEX [IX_SeatAreas_CenterId] ON [HawkerCenter].[SeatAreas] ([CenterId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190114114141_AddSeatAreaAndCenterBanner', N'2.1.4-rtm-31024');

GO

