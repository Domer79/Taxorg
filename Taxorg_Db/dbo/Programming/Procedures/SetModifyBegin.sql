CREATE PROCEDURE [dbo].[SetModifyBegin]
	@tableName varchar(100)
AS
if not exists(select 1 from ModifyProcedureWork where tableName = @tableName)
	insert into ModifyProcedureWork(tableName, isModified) values(@tableName, 1)
else
	update ModifyProcedureWork set isModified = 1 where tableName = @tableName