use Taxorg
go

if exists(select 1 from sys.objects where object_id = OBJECT_ID(N'SessionTaxType') and type in (N'U'))
	drop table SessionTaxType
go

if exists(select 1 from sys.objects where object_id = OBJECT_ID(N'Sessions') and type in (N'U'))
	drop table Sessions