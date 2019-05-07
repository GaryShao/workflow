ALTER TABLE [Restaurant].[Dishes] ADD [IsDeleted] bit NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190109063304_AddSoftDeleteToDish', N'2.1.4-rtm-31024');

GO

