实体层使用模板自动生成

Codesmith 在描述中识别自定义类型: 例 Enum<CountryType> or CacheList<Infor> or Infor

1.增加枚举字段类型
EXECUTE sp_addextendedproperty N'MS_Description', '类型[Enum<CountryType>]', N'SCHEMA', N'dbo', N'table', N'表名', N'column', N'字段名' 

2.增加对象字段类型
EXECUTE sp_addextendedproperty N'MS_Description', '名称[CacheList<T>]', N'SCHEMA', N'dbo', N'table', N'表名', N'column', N'字段名' 

3.增加表注释
EXECUTE sp_addextendedproperty N'MS_Description', '玩家信息[:BaseUser]', N'user', N'dbo', N'table', N'GameUser', NULL, NULL

