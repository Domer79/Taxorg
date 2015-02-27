CREATE PROCEDURE [dbo].[DeleteOrganization]
	@idOrganization int
as

exec SetModifyBegin 'Organization'
delete from Organization where idOrganization = @idOrganization
exec SetModifyEnd 'Organization'