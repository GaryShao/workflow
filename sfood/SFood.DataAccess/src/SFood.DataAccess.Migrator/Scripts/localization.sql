IF SCHEMA_ID(N'Localization') IS NULL EXEC(N'CREATE SCHEMA [Localization];');

GO

CREATE TABLE [Localization].[Languages] (
    [Id] nvarchar(32) NOT NULL,
    [Code] nvarchar(20) NULL,
    CONSTRAINT [PK_Languages] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Localization].[Resources] (
    [Id] nvarchar(32) NOT NULL,
    [Key] nvarchar(100) NULL,
    [Value] nvarchar(500) NULL,
    [LanguageId] nvarchar(32) NULL,
    CONSTRAINT [PK_Resources] PRIMARY KEY ([Id])
);

GO

CREATE UNIQUE INDEX [IX_Languages_Code] ON [Localization].[Languages] ([Code]) WHERE [Code] IS NOT NULL;

GO

CREATE UNIQUE INDEX [IX_Resources_LanguageId_Key] ON [Localization].[Resources] ([LanguageId], [Key]) WHERE [LanguageId] IS NOT NULL AND [Key] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190117054812_AddLocalization', N'2.1.4-rtm-31024');

GO

