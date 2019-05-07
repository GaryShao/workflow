ALTER TABLE [HawkerCenter].[Banner] DROP CONSTRAINT [PK_Banner];

GO

EXEC sp_rename N'[HawkerCenter].[Banner]', N'Banners';

GO

EXEC sp_rename N'[HawkerCenter].[Banners].[IX_Banner_CenterId]', N'IX_Banners_CenterId', N'INDEX';

GO

ALTER TABLE [HawkerCenter].[Banners] ADD CONSTRAINT [PK_Banners] PRIMARY KEY ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190114114810_RenameBanner', N'2.1.4-rtm-31024');

GO

