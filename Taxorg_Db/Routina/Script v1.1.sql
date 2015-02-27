use Taxorg
go

if exists(select 1 from sys.columns where name = 'period' and object_id = object_id(N'Tax'))
	and exists(select 1 from sys.columns where name = 'periodName' and object_id = object_id(N'Tax'))
raiserror('Остановка скрипта', 20, 1) with log


begin transaction

alter table tax
add 
	period YearMonth,
	periodName as period.ToString()
go

update Tax set period = dbo.YearMonthCreateByDateTime(dateLoad) where idTax = idTax

drop index UQ_Tax_idOrganization_idTaxType_dateLoad on Tax

alter table Tax
drop column dateLoad

CREATE UNIQUE NONCLUSTERED INDEX [UQ_Tax_idOrganization_idTaxType_dateLoad] ON [dbo].[Tax]
(
	[idOrganization] ASC,
	[idTaxType] ASC,
	[period] ASC
)

select * from Tax

commit transaction