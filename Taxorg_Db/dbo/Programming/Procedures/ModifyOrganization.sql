CREATE PROCEDURE [dbo].[ModifyOrganization]
	@idOrganization int,
	@name varchar(900),
	@shortName varchar(200),
	@addr varchar(900),
	@inn varchar(30)
AS

exec SetModifyBegin 'Organization'
update Organization set name = @name, shortName = @shortName, addr = @addr, inn = @inn where idOrganization = @idOrganization
exec SetModifyEnd 'Organization'