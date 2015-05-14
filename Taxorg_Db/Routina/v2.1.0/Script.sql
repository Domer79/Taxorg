use Taxorg
go

update Settings set value = '2.1.0' where name = 'appversion'
go

alter table Sessions
add userId int not null

alter table Sessions
add timeLabel datetime not null default(getdate())
go

alter table TaxType
alter column name varchar(max)