  use [SFood-Beta]
  go

  declare @zhcnId nvarchar(32);

  select @zhcnId = Id
  from Localization.Languages
  where Code = 'zh-CN';

INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'no-such-data', '服务异常', @zhcnId);
INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'invalid-verification-code', '验证码错误', @zhcnId);
INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'bad-data', '服务异常', @zhcnId);
INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'bad-response-from-commsHub', '服务异常', @zhcnId);

INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'wrong-country-code', '国家码错误', @zhcnId);
INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'no-files-found-in-request', '未发现提交的文件', @zhcnId);
INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'refuse-reason-needed', '您需要提交拒单原因', @zhcnId);
INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'new-old-phone-same', '新旧号码不能相同', @zhcnId);
INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'bad-request', '参数异常', @zhcnId);
INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'last-category-constraint', '菜单的最后一个分类无法删除', @zhcnId);
INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'menu-timerange-overlapped', '该菜单的时间区间和其他菜单有重叠', @zhcnId);
INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'order-already-expired', '订单已过期，请刷新列表', @zhcnId);
INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'bad-request-outofdate-data', '数据已过期', @zhcnId);
INSERT INTO [Localization].[Resources] VALUES (REPLACE(NEWID(), '-', ''), 'phone-already-exist', '手机号码已存在', @zhcnId);