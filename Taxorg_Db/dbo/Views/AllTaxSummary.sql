CREATE view [dbo].[AllTaxSummary]
as
with taxWithYearMonth(idOrganization, inn, name, shortName, addr, tax, taxDebitKredit, period, periodName)
as
(
	select
		t1.idOrganization,
		org.inn,
		org.name,
		org.shortName,
		org.addr,
		tax = tax * -1,
		case
			when t1.tax > 0 then '+ ' + cast(t1.tax as varchar(100))
			else cast(ABS(t1.tax) as varchar(100))
		end as taxDebitKredit,
		t1.period,
		t1.periodName
	from
	(
		select
			idOrganization,
			SUM(tax) tax,
			period,
			periodName
		from
		(
			select
				idTax,
				idOrganization,
				idTaxType,
				tax,
				period,
				periodName
			from
				Tax
		)t
		group by
			idOrganization, period, periodName
	)t1 inner join Organization org
	on
		t1.idOrganization = org.idOrganization
)

select 
	idOrganization,
	inn,
	name,
	shortName,
	addr,
	tax,
	taxDebitKredit,
	--prevTax = dbo.GetTax(idOrganization, yearMonth.Minus(cast(dbo.GetSettings('taxprevperiod') as int)).Year, yearMonth.Minus(1).Month),
	period,
	periodName
from 
	taxWithYearMonth