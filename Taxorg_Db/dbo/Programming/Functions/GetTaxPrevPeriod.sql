create function GetTaxPrevPeriod()
returns int
as
begin
	return isnull(cast(dbo.GetSettings('taxprevperiod') as int), 1)
end