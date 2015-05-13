use Taxorg
go

--Отключение тригера на изменение записей в таблице Organization
if exists(select 1 from sys.triggers where name = 'OnAddOrganization' and parent_id = OBJECT_ID(N'Organization'))
	disable trigger OnAddOrganization on Organization
go

---Функции начало---

ALTER FUNCTION [dbo].[GetCurrentPeriod]
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
GO

ALTER FUNCTION [dbo].[GetSettings]
(
	@name varchar(30)
)
RETURNS varchar(max)
AS
BEGIN
	return (select value from Settings where name = @name)
END
GO

ALTER FUNCTION [dbo].[GetSimpleTax]
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
GO

alter function GetTax(@idOrganization int, @period YearMonth)
returns money
as
begin
	declare @sum money

	select @sum = tax from AllTaxSummary where idOrganization = @idOrganization and period = @period

	return @sum
end
GO

alter function GetTaxPrevPeriod()
returns int
as
begin
	return isnull(cast(dbo.GetSettings('taxprevperiod') as int), 1)
end
GO

ALTER FUNCTION [dbo].[TableIsModified]
(
	@tableName varchar(100)
)
RETURNS BIT
AS
BEGIN
	if Exists(select 1 from ModifyProcedureWork where tableName = @tableName)
		return 1

	return 0
END
GO

alter function ValidateYearMonth(@year int, @month int)
returns bit
as
begin
if @month < 1 or @month > 12
	return 0

if @year < 2000 or @year > 2100
	return 0

return 1
end
GO

---Функции конец---

---Процедуры начало---

ALTER PROCEDURE [dbo].[AddOrganization]
	@name varchar(900),
	@shortName varchar(200),
	@addr varchar(900),
	@inn varchar(30)
as

exec SetModifyBegin 'Organization'
Insert into Organization(name, shortName, addr, inn) values(@name, @shortName, @addr, @inn)
exec SetModifyEnd 'Organization'

select SCOPE_IDENTITY() as generated_blog_identity
GO

ALTER PROCEDURE [dbo].[DeleteOrganization]
	@idOrganization int
as

exec SetModifyBegin 'Organization'
delete from Organization where idOrganization = @idOrganization
exec SetModifyEnd 'Organization'
GO

ALTER PROCEDURE [dbo].[GrantToPublic]
    @toPrint BIT = 1
AS
SET NOCOUNT ON
BEGIN

DECLARE @str VARCHAR(4000)
DECLARE @stmt NVARCHAR(MAX)
DECLARE @crlf VARCHAR(50)

SET @stmt = ''
SET @crlf = /*CHAR(13) + 'GO' +*/ CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10)

DECLARE procursor CURSOR
FOR SELECT 'GRANT EXEC ON ' + schm.name + '.' + p.name + ' TO PUBLIC;' FROM sys.procedures p inner join sys.schemas schm on p.schema_id = schm.schema_id WHERE LOWER(p.name) <> 'granttopublic'

DECLARE tcursor CURSOR
FOR SELECT 'GRANT SELECT, UPDATE, INSERT, DELETE ON ' + schm.name + '.' + p.name + ' TO PUBLIC;' FROM sys.tables p inner join sys.schemas schm on p.schema_id = schm.schema_id

DECLARE vcursor CURSOR
FOR SELECT 'GRANT SELECT ON ' + schm.name + '.' + p.name + ' TO PUBLIC;' FROM sys.views p inner join sys.schemas schm on p.schema_id = schm.schema_id


    OPEN procursor
    FETCH NEXT FROM procursor INTO @str
    WHILE @@FETCH_STATUS = 0
    BEGIN
		IF @toPrint = 1
		BEGIN
			IF LEN(@stmt + @str) > 4000
			BEGIN
				PRINT @stmt
				SET @stmt = ''
			END
		END

        SET @stmt += @str
        IF @toPrint = 1
			SET @stmt += @crlf
        FETCH NEXT FROM procursor INTO @str
    END

    CLOSE procursor
    DEALLOCATE PROCURSOR

    

    OPEN tcursor
    FETCH NEXT FROM tcursor INTO @str
    WHILE @@FETCH_STATUS = 0
    BEGIN
		if @toPrint = 1
		BEGIN
			IF LEN(@stmt + @str) > 4000
			BEGIN
				PRINT @stmt
				SET @stmt = ''
			END
		END

        SET @stmt += @str
        IF @toPrint = 1
            SET @stmt += @crlf
        FETCH NEXT FROM tcursor INTO @str
    END

    CLOSE tcursor
    DEALLOCATE tcursor



    OPEN vcursor
    FETCH NEXT FROM vcursor INTO @str
    WHILE @@FETCH_STATUS = 0
    BEGIN
		IF @toPrint = 1
		BEGIN
			IF LEN(@stmt + @str) > 4000
			BEGIN
				PRINT @stmt
				Set @stmt = ''
			END
		END

        SET @stmt += @str
        IF @toPrint = 1
            SET @stmt += @crlf
        FETCH NEXT FROM vcursor INTO @str
    END

    CLOSE vcursor
    DEALLOCATE vcursor



IF @toPrint = 0 
    EXEC sp_executesql @stmt
ELSE
    PRINT @stmt

END
GO

ALTER PROCEDURE [dbo].[ModifyOrganization]
	@idOrganization int,
	@name varchar(900),
	@shortName varchar(200),
	@addr varchar(900),
	@inn varchar(30)
AS

exec SetModifyBegin 'Organization'
update Organization set name = @name, shortName = @shortName, addr = @addr, inn = @inn where idOrganization = @idOrganization
exec SetModifyEnd 'Organization'
GO

/***************SaveTax***************/
--Производит загрузку в данных, присланных с налоговой
ALTER procedure [dbo].[SaveTax]
	@inn varchar(30),
	@taxCode  varchar(100),
	@taxName varchar(max),
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
	insert into Organization (inn) values(@inn)
	select @idOrganization = scope_identity()
end

if @idTaxType is null
begin
	insert into TaxType (code, name) values(@taxCode, @taxName)
	select @idTaxType = SCOPE_IDENTITY()
end
else
begin
	update TaxType set name = @taxName where idTaxType = @idTaxType
end

if exists(select 1 from Tax where idOrganization = @idOrganization and idTaxType = @idTaxType and period = @period)
	update Tax set tax = tax + @taxSum where idOrganization = @idOrganization and idTaxType = @idTaxType and period = @period
else
	insert into Tax(idOrganization, idTaxType, tax, period) values(@idOrganization, @idTaxType, @taxSum, @period)

GO
/***************SaveTax***************/

ALTER PROCEDURE [dbo].[SetModifyBegin]
	@tableName varchar(100)
AS
if not exists(select 1 from ModifyProcedureWork where tableName = @tableName)
	insert into ModifyProcedureWork(tableName, isModified) values(@tableName, 1)
else
	update ModifyProcedureWork set isModified = 1 where tableName = @tableName
GO

ALTER PROCEDURE [dbo].[SetModifyEnd]	
	@tableName varchar(100)
AS
if not exists(select 1 from ModifyProcedureWork where tableName = @tableName)
	raiserror('This table does not exists', 16, 10)

update ModifyProcedureWork set isModified = 0 where tableName = @tableName
GO

alter procedure SetTaxPrevPeriodCount
	@value int
as
if not exists(select 1 from Settings where name = 'taxprevperiod')
	insert into Settings(name, value, description) values('taxprevperiod', CAST(@value as varchar(max)), 'Значение для вычисления предыдущего периода')
else
	update Settings Set value = @value where name = 'taxprevperiod'
GO

alter procedure SetMaintenance
	@value varchar(10)
as
if not exists(select 1 from Settings where name = 'ismaintenance')
	insert into Settings(name, value, description) values('ismaintenance', CAST(@value as varchar(max)), 'База данных на техническом обслуживании')
else
	update Settings Set value = @value where name = 'ismaintenance'
GO
	

---Процедуры конец---


---View начало---

ALTER view [dbo].[AllTaxSummary]
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
			when t1.tax > 0 then '+ ' + replace(convert(varchar(100), t1.tax, 1), ',', ' ')
			else replace(convert(varchar(30), ABS(t1.tax), 1), ',', ' ')
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
GO

ALTER VIEW [dbo].[SliceTax]
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
GO

alter view TaxSummary
as
select
	*,
	prevTax = isnull(dbo.GetTax(idOrganization, period.Minus(dbo.GetTaxPrevPeriod())), 0)
from
	AllTaxSummary
where
	period = dbo.GetCurrentPeriod()
GO

---View конец---


--Post Deployment
--Заполнение таблицы Settings значениями по умолчанию

if not exists(select 1 from Settings where name = 'taxperiodyear')
	insert into Settings(name, value, description) values('taxperiodyear', 'current', 'Установка года для выбора периода, если значение не установлено или отсутствует, то значение устанавливается в current, что означает текущий год')

if not exists(select 1 from Settings where name = 'taxperiodmonth')
	insert into Settings(name, value, description) values('taxperiodmonth', 'current', 'Установка месяца для выбора периода, если значение не установлено или отсутствует, то значение устанавливается в current, что означает текущий месяц')

if not exists(select 1 from Settings where name = 'taxprevperiod')
	insert into Settings(name, value, description) values('taxprevperiod', '1', 'Значение для вычисления предыдущего периода')

if not exists(select 1 from Settings where name = 'isnotsametaxload')
	insert into Settings(name, value, description) values('isnotsametaxload', '0', 'Запрещает/Разрешает загрузку данных с одинаковыми значениями сумм долга за текущий и предыдущий периоды')

if not exists(select 1 from Settings where name = 'appversion')
	insert into Settings(name, value, description, visible) values('appversion', '1.1.1.0', 'Версия приложения', 0)

if not exists(select 1 from Settings where name = 'ismaintenance')
	insert into Settings(name, value, description) values('ismaintenance', 'False', 'База данных на техническом обслуживании')


--Инструкция всегда должна быть последней
--Предоставление прав для Public-а
exec GrantToPublic 0