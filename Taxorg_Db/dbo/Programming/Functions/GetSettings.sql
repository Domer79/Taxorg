CREATE FUNCTION [dbo].[GetSettings]
(
	@name varchar(30)
)
RETURNS varchar(max)
AS
BEGIN
	return (select value from Settings where name = @name)
END
