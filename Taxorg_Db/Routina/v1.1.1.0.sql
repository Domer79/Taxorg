use Taxorg
go

if not exists(select 1 from sys.columns where name = 'visible' and object_id = object_id(N'Settings'))
	alter table Settings add visible bit not null default(1)