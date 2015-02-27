CREATE PROCEDURE [dbo].[SetModifyEnd]	
	@tableName varchar(100)
AS
if not exists(select 1 from ModifyProcedureWork where tableName = @tableName)
	raiserror('This table does not exists', 16, 10)

update ModifyProcedureWork set isModified = 0 where tableName = @tableName