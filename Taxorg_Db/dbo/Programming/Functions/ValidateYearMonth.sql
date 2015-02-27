create function ValidateYearMonth(@year int, @month int)
returns bit
as
begin
if @month < 1 or @month > 12
	return 0

if @year < 2000 or @year > 2100
	return 0

return 1
end