create view TaxSummary
as
select
	*,
	prevTax = isnull(dbo.GetTax(idOrganization, period.Minus(dbo.GetTaxPrevPeriod())), 0)
from
	AllTaxSummary
where
	period = dbo.GetCurrentPeriod()