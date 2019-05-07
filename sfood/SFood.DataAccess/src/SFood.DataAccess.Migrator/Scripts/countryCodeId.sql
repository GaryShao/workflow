ALTER TABLE [Restaurant].[Details] ADD [CountryCodeId] nvarchar(32) NULL;

GO

ALTER TABLE [IdentitySchema].[UserExtensions] ADD [CountryCodeId] nvarchar(32) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190116031240_AddCountryCodeId', N'2.1.4-rtm-31024');

GO

