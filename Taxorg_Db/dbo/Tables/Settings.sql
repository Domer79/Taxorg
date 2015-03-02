CREATE TABLE [dbo].[Settings]
(
	[idSettings] INT NOT NULL PRIMARY KEY identity,
	name varchar(30),
	value varchar(max),
	description varchar(4000),
	visible bit not null default(1)
)
go

create unique nonclustered index UQ_Settings_Name on Settings(name asc)
go
