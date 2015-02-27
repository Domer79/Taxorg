CREATE FUNCTION [dbo].[TableIsModified]
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
