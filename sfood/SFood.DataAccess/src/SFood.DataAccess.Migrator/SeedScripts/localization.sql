use [SFood-Beta]
go

insert into [Localization].[Languages]
values(replace(newid(), '-', ''), 'zh-CN');

insert into [Localization].[Languages]
values(replace(newid(), '-', ''), 'en-US');

