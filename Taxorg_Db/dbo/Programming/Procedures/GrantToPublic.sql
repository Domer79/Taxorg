CREATE PROCEDURE [dbo].[GrantToPublic]
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
FOR SELECT 'GRANT EXEC ON ' + name + ' TO PUBLIC;' FROM sys.procedures WHERE LOWER(name) <> 'granttopublic'

DECLARE tcursor CURSOR
FOR SELECT 'GRANT SELECT, UPDATE, INSERT, DELETE ON ' + name + ' TO PUBLIC;' FROM sys.tables

DECLARE vcursor CURSOR
FOR SELECT 'GRANT SELECT ON ' + name + ' TO PUBLIC;' FROM sys.views


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