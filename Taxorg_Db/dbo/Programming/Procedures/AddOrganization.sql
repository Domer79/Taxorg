CREATE PROCEDURE [dbo].[AddOrganization]
	@name varchar(900),
	@shortName varchar(200),
	@addr varchar(900),
	@inn varchar(30)
as

exec SetModifyBegin 'Organization'
Insert into Organization(name, shortName, addr, inn) values(@name, @shortName, @addr, @inn)
exec SetModifyEnd 'Organization'

select SCOPE_IDENTITY() as generated_blog_identity