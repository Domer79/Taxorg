CREATE FUNCTION [dbo].[GetCurrentPeriod]
(
)
RETURNS VARCHAR(7)
AS
BEGIN
	declare @taxperiodyear varchar(100)
	declare @taxperiodmonth varchar(100)

	--Год
	select @taxperiodyear = value from Settings where name = 'taxperiodyear'

	--Месяц
	select @taxperiodmonth = value from Settings where name = 'taxperiodmonth'

	if @taxperiodyear = 'current'
		set @taxperiodyear = cast(datepart(YY, getdate()) as varchar(4))

	if @taxperiodmonth = 'current'
		set @taxperiodmonth = cast(datepart(MM, getdate()) as varchar(2))

	return @taxperiodmonth + '.' + @taxperiodyear
END
