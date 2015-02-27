create procedure SaveTax
	@inn varchar(30),
	@taxCode  varchar(100),
	@period varchar(7),
	@taxSum money
as
SET NOCOUNT ON

declare @idOrganization int
declare @idTaxType int

select @idOrganization = idOrganization from Organization where inn = @inn
select @idTaxType = idTaxType from TaxType where code = @taxCode

if @idOrganization is null
begin
	exec AddOrganization null, null, null, @inn
	--insert into Organization (inn) values(@inn)
	select @idOrganization = scope_identity()
end

if @idTaxType is null
begin
	insert into TaxType (code) values(@taxCode)
	select @idTaxType = SCOPE_IDENTITY()
end

if exists(select 1 from Tax where idOrganization = @idOrganization and idTaxType = @idTaxType and period = @period)
	update Tax set tax = @taxSum where idOrganization = @idOrganization and idTaxType = @idTaxType and period = @period
else
	insert into Tax(idOrganization, idTaxType, tax, period) values(@idOrganization, @idTaxType, @taxSum, @period)