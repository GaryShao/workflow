CREATE TABLE [Common].[CountryCodes] (
    [Id] nvarchar(32) NOT NULL,
    [Code] nvarchar(20) NULL,
    [Name] nvarchar(100) NULL,
    [EnglishName] nvarchar(100) NULL,
    [FlagUrl] nvarchar(500) NULL,
    CONSTRAINT [PK_CountryCodes] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190115034734_CountryCode', N'2.1.4-rtm-31024');

GO

