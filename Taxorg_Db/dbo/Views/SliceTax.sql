CREATE VIEW [dbo].[SliceTax]
AS
with TaxExtended(idTax, idOrganization, organizationInn, organizationName, organizationShortName, idTaxType, taxCode, taxName, taxSum, taxDebitKredit, period, periodName)
as
(
	select
		extax.idTax,
		extax.idOrganization,
		organizationInn,
		organizationName,
		organizationShortName,
		idTaxType,
		taxCode,
		taxName,
		taxSum,
		taxDebitKredit,
		period,
		periodName
	from
	(
		select
			idTax,
			Tax.idOrganization,
			org.inn as organizationInn,
			org.name as organizationName,
			org.shortName as organizationShortName,
			Tax.idTaxType,
			tt.code as taxCode,
			tt.name as taxName,
			taxSum = tax * -1,
			case
				when tax > 0 then '+ ' + cast(tax as varchar(100))
				else cast(ABS(tax) as varchar(100))
			end as taxDebitKredit,
			Tax.period,
			Tax.periodName
		from
			Tax inner join Organization org
		on
			Tax.idOrganization = org.idOrganization
			inner join TaxType tt
		on
			Tax.idTaxType = tt.idTaxType
	) extax
	where
		period = dbo.GetCurrentPeriod()
)

select 
	idTax, 
	idOrganization, 
	organizationInn, 
	organizationName, 
	organizationShortName, 
	idTaxType, 
	taxCode, 
	taxName, 
	taxSum,
	prevTaxSum = isnull(dbo.GetSimpleTax(idOrganization, idTaxType, period.Minus(dbo.GetTaxPrevPeriod())), 0) * -1,
	taxDebitKredit,
	period,
	periodName
from 
	TaxExtended 