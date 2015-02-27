create function GetTax(@idOrganization int, @period YearMonth)
returns money
as
begin
	declare @sum money

	select @sum = tax from AllTaxSummary where idOrganization = @idOrganization and period = @period

	return @sum
end