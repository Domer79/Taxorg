CREATE FUNCTION [dbo].[GetSimpleTax]
(
	@idOrganization int,
	@idTaxType int,
	@period YearMonth
)
RETURNS MONEY
AS
BEGIN
	declare @taxSum money
	select @taxSum = tax from Tax where idOrganization = @idOrganization and idTaxType = @idTaxType and period = @period

	return @taxSum
END
