use Taxorg
go

/******************   DROP   **********************/

/****** Object:  View [sec].[GroupsGrant]    Script Date: 13.05.2015 16:30:25 ******/
DROP VIEW [sec].[GroupsGrant]
GO
/****** Object:  View [sec].[UsersGrant]    Script Date: 13.05.2015 16:30:25 ******/
DROP VIEW [sec].[UsersGrant]
GO
/****** Object:  View [sec].[RoleOfMember]    Script Date: 13.05.2015 16:30:25 ******/
DROP VIEW [sec].[RoleOfMember]
GO
/****** Object:  View [sec].[Members]    Script Date: 13.05.2015 16:30:25 ******/
DROP VIEW [sec].[Members]
GO
/****** Object:  View [sec].[Grants]    Script Date: 13.05.2015 16:30:25 ******/
DROP VIEW [sec].[Grants]
GO
/****** Object:  View [sec].[UserGroupsDetail]    Script Date: 13.05.2015 16:30:25 ******/
DROP VIEW [sec].[UserGroupsDetail]
GO
/****** Object:  View [sec].[Users]    Script Date: 13.05.2015 16:30:25 ******/
DROP VIEW [sec].[Users]
GO
/****** Object:  View [sec].[Groups]    Script Date: 13.05.2015 16:30:25 ******/
DROP VIEW [sec].[Groups]
GO
/****** Object:  UserDefinedFunction [sec].[IsAllowByName]    Script Date: 13.05.2015 16:30:25 ******/
DROP FUNCTION [sec].[IsAllowByName]
GO
/****** Object:  UserDefinedFunction [sec].[IsAllowById]    Script Date: 13.05.2015 16:30:25 ******/
DROP FUNCTION [sec].[IsAllowById]
GO
/****** Object:  UserDefinedFunction [sec].[GetSettings]    Script Date: 13.05.2015 16:30:25 ******/
DROP FUNCTION [sec].[GetSettings]
GO
/****** Object:  UserDefinedFunction [sec].[GetIdentificationMode]    Script Date: 13.05.2015 16:30:25 ******/
DROP FUNCTION [sec].[GetIdentificationMode]
GO
/****** Object:  StoredProcedure [sec].[UpdateUserGroup]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[UpdateUserGroup]
GO
/****** Object:  StoredProcedure [sec].[UpdateUser]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[UpdateUser]
GO
/****** Object:  StoredProcedure [sec].[UpdateMemberRole]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[UpdateMemberRole]
GO
/****** Object:  StoredProcedure [sec].[UpdateGroup]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[UpdateGroup]
GO
/****** Object:  StoredProcedure [sec].[UpdateGrant]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[UpdateGrant]
GO
/****** Object:  StoredProcedure [sec].[SetIdentificationMode]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[SetIdentificationMode]
GO
/****** Object:  StoredProcedure [sec].[DeleteUserFromGroup]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[DeleteUserFromGroup]
GO
/****** Object:  StoredProcedure [sec].[DeleteUser]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[DeleteUser]
GO
/****** Object:  StoredProcedure [sec].[DeleteMemberRole]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[DeleteMemberRole]
GO
/****** Object:  StoredProcedure [sec].[DeleteGroup]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[DeleteGroup]
GO
/****** Object:  StoredProcedure [sec].[DeleteGrant]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[DeleteGrant]
GO
/****** Object:  StoredProcedure [sec].[AddUserToGroup]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[AddUserToGroup]
GO
/****** Object:  StoredProcedure [sec].[AddUser]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[AddUser]
GO
/****** Object:  StoredProcedure [sec].[AddMemberRole]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[AddMemberRole]
GO
/****** Object:  StoredProcedure [sec].[AddGroup]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[AddGroup]
GO
/****** Object:  StoredProcedure [sec].[AddGrant]    Script Date: 13.05.2015 16:30:25 ******/
DROP PROCEDURE [sec].[AddGrant]
GO

/******************   DROP   **********************/


/******************   Procedures   **********************/

create procedure [sec].[AddGrant]
	@IdSecObject int,
	@IdRole int,
	@IdAccessType int,
	@ObjectName varchar(500) = null,
	@Type1 varchar(500) = NULL,
	@Type2 varchar(500) = NULL,
	@Type3 varchar(500) = NULL,
	@Type4 varchar(500) = NULL,
	@Type5 varchar(500) = NULL,
	@Type6 varchar(500) = NULL,
	@Type7 varchar(500) = NULL,
	@RoleName varchar(500) = NULL,
	@RoleDescription varchar(max)=NULL,
	@AccessName varchar(500) = NULL
as
set nocount on
insert into sec._Grant(idSecObject, idRole, idAccessType) values(@IdSecObject, @IdRole, @IdAccessType)

GO
/****** Object:  StoredProcedure [sec].[AddGroup]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[AddGroup]
	@name varchar(200),
	@description varchar(max)
as
set nocount on
insert into sec.Groups(name, description) values(@name, @description)
select IDENT_CURRENT('sec.Member') as idMember

GO
/****** Object:  StoredProcedure [sec].[AddMemberRole]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [sec].[AddMemberRole]
	@idRole int,
	@idMember int,
	@roleName varchar(200) = null,
	@RoleDescription varchar(max) = NULL,
	@memberName varchar(200) = null,
	@isUser bit = null
as
set nocount on
insert into sec.MemberRole(idMember, idRole) values(@idMember, @idRole)

GO
/****** Object:  StoredProcedure [sec].[AddUser]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [sec].[AddUser]
	@login varchar(200),
	@password varbinary(16),
	@displayName varchar(200),
	@email varchar(300),
	@usersid varchar(300)
as
set nocount on
insert into sec.Users(login, password, displayName, email, usersid) values(@login, @password, @displayName, @email, @usersid)
select IDENT_CURRENT('sec.Member') as idMember
GO
/****** Object:  StoredProcedure [sec].[AddUserToGroup]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[AddUserToGroup]
	@idUser int,
	@idGroup int,
	@login varchar(200) = null,
	@displayName varchar(200) = null,
	@email varchar(100) = null,
	@usersid varchar(100) = null,
	@groupName varchar(200) = null,
	@groupDescription varchar(max) = null
as
set nocount on
insert into sec.UserGroups(idUser, idGroup) values(@idUser, @idGroup)

GO
/****** Object:  StoredProcedure [sec].[DeleteGrant]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [sec].[DeleteGrant]
	@IdSecObject int,
	@IdRole int,
	@IdAccessType int
as
set nocount on
delete from sec._Grant where idSecObject = @IdSecObject and idRole = @IdRole and idAccessType = @IdAccessType
GO
/****** Object:  StoredProcedure [sec].[DeleteGroup]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[DeleteGroup]
	@idGroup int
as
set nocount on
delete from sec.Groups where idGroup = @idGroup
GO
/****** Object:  StoredProcedure [sec].[DeleteMemberRole]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [sec].[DeleteMemberRole]
	@idRole int,
	@idMember int
as
set nocount on

delete from sec.MemberRole where idMember = @idMember and idRole = @idRole

GO
/****** Object:  StoredProcedure [sec].[DeleteUser]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[DeleteUser]
	@idUser int
as
set nocount on
delete from sec.Users where idUser = @idUser
GO
/****** Object:  StoredProcedure [sec].[DeleteUserFromGroup]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[DeleteUserFromGroup]
	@idUser int,
	@idGroup int
as
set nocount on
delete from sec.UserGroups where idUser = @idUser and idGroup = @idGroup

GO
/****** Object:  StoredProcedure [sec].[SetIdentificationMode]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[SetIdentificationMode]
	@mode varchar(100)
as
set nocount on
if not exists(select 1 from sec.Settings where name = 'identification_mode')
	insert into sec.Settings(name, value) values('identification_mode', @mode)
else
	update sec.Settings set value = @mode where name = 'identification_mode'
GO
/****** Object:  StoredProcedure [sec].[UpdateGrant]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[UpdateGrant]
	@IdSecObject int,
	@IdRole int,
	@IdAccessType int,
	@ObjectName varchar(500) = null,
	@Type1 varchar(500) = NULL,
	@Type2 varchar(500) = NULL,
	@Type3 varchar(500) = NULL,
	@Type4 varchar(500) = NULL,
	@Type5 varchar(500) = NULL,
	@Type6 varchar(500) = NULL,
	@Type7 varchar(500) = NULL,
	@RoleName varchar(500) = NULL,
	@RoleDescription varchar(max)=NULL,
	@AccessName varchar(500) = NULL
as
set nocount on
raiserror('not_modified', 16, 1)

GO
/****** Object:  StoredProcedure [sec].[UpdateGroup]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[UpdateGroup]
	@idGroup int,
	@name varchar(200),
	@description varchar(max)
as
set nocount on
update sec.Groups set name = @name, description = @description where idGroup = @idGroup

GO
/****** Object:  StoredProcedure [sec].[UpdateMemberRole]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [sec].[UpdateMemberRole]
	@idRole int,
	@idMember int,
	@roleName varchar(200),
	@RoleDescription varchar(max) = NULL,
	@memberName varchar(200),
	@isUser bit
as
raiserror('is_not_modified', 16, 10)
GO
/****** Object:  StoredProcedure [sec].[UpdateUser]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[UpdateUser]
	@idUser int,
	@login varchar(200),
	@password varbinary(16),
	@displayName varchar(200),
	@email varchar(300),
	@usersid varchar(300)
as
set nocount on
update sec.Users set login = @login, password = @password, displayName = @displayName, email = @email, usersid = @usersid where idUser = @idUser

GO
/****** Object:  StoredProcedure [sec].[UpdateUserGroup]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[UpdateUserGroup]
	@idUser int,
	@idGroup int,
	@login varchar(200) = null,
	@displayName varchar(200) = null,
	@email varchar(100) = null,
	@usersid varchar(100) = null,
	@groupName varchar(200) = null,
	@groupDescription varchar(max) = null
as
set nocount on
raiserror('is_not_modified', 16, 10)
GO
/****** Object:  UserDefinedFunction [sec].[GetIdentificationMode]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/******************   Procedures   **********************/

/******************   Functions   **********************/

create function [sec].[GetIdentificationMode]()
returns varchar(max)
as
begin
	return sec.GetSettings('identification_mode')
end
GO
/****** Object:  UserDefinedFunction [sec].[GetSettings]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [sec].[GetSettings](@name varchar(100))
returns varchar(max)
as
begin
	declare @value varchar(max)

	select @value = value from sec.Settings where name = @name

	return @value
end

GO
/****** Object:  UserDefinedFunction [sec].[IsAllowById]    Script Date: 25.04.2015 2:54:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create function [sec].[IsAllowById](@idSecObject int, @idMember int, @idAccessType int)
returns bit
as
begin
	declare @result bit = 0

	--Сначала проверяем доступ, если участником безопасности является группа
	if not exists(select 1 from sec.Users where idUser  = @idMember)
	begin
		if exists
				(
					select
						1
					from
						sec._Grant gr inner join sec.SecObject so 
					on
						gr.idSecObject = so.idSecObject inner join sec.RoleOfMember rof 
					on
						gr.idRole = rof.idRole 
					where 
						so.idSecObject = @idSecObject
						and rof.idMember = @idMember
						and idAccessType = @idAccessType
				)
		begin
			set @result = 1
		end
		
		return @result
	end

	--Проверяем доступ по списку всех групп, в которых состоит пользователь, не забывая при вклювить самого пользователя в этот список
	if exists
			(
				select
					1
				from
					sec._Grant gr inner join sec.SecObject so 
				on
					gr.idSecObject = so.idSecObject inner join sec.RoleOfMember rof 
				on
					gr.idRole = rof.idRole 
				where 
					so.idSecObject = @idSecObject
					and rof.idMember in (select idGroup from sec.UserGroups where idUser = @idMember union all select @idMember)
					and idAccessType = @idAccessType
			)
	begin
		set @result = 1
	end

	return @result
end

GO
/****** Object:  UserDefinedFunction [sec].[IsAllowByName]    Script Date: 25.04.2015 2:54:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE function [sec].[IsAllowByName](@secObject varchar(200), @member varchar(200), @accessType varchar(300))
returns bit
as
begin
declare @idSecObject int = (select idSecObject from sec.SecObject where ObjectName = @secObject)
declare @idMember int = (select idMember from sec.Member where name = @member)
declare @idAccessType int = (select idAccessType from sec.AccessType where name = @accessType)

return sec.IsAllowById(@idSecObject, @idMember, @idAccessType)
end

GO

/******************   Functions   **********************/

/***************  View  ****************/

create view [sec].[Groups]
as
select
	m.idMember idGroup,
	m.name,
	g.description
from
	sec._Group g inner join sec.Member m
on
	g.idMember = m.idMember

GO
/****** Object:  View [sec].[Users]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE view [sec].[Users]
as
select
	m.idMember idUser,
	m.name login,
	u.password,
	u.displayName,
	u.email,
	u.usersid
from 
	sec.Member m inner join sec._User u 
on
	m.idMember = u.idMember






GO
/****** Object:  View [sec].[UserGroupsDetail]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [sec].[UserGroupsDetail]
as
select
	u.idUser,
	u.login,
	u.displayName,
	u.email,
	u.usersid,
	g.idGroup,
	g.name as groupName,
	g.description as groupDescription
from
	sec.Users u inner join sec.UserGroups ug
on
	u.idUser = ug.idUser inner join sec.Groups g
on
	ug.idGroup = g.idGroup


GO
/****** Object:  View [sec].[Grants]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE view [sec].[Grants]
as
select
	so.idSecObject,
	so.ObjectName,
	so.type1,
	so.type2,
	so.type3,
	so.type4,
	so.type5,
	so.type6,
	so.type7,
	r.idRole,
	r.name roleName,
	r.description roleDescription,
	at.idAccessType,
	at.name accessName
from 
	sec._Grant gr inner join sec.SecObject so
on
	gr.idSecObject = so.idSecObject inner join sec._Role r
on
	gr.idRole = r.idRole inner join sec.AccessType at
on
	gr.idAccessType = at.idAccessType




GO
/****** Object:  View [sec].[Members]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [sec].[Members]
as
select
	*,
	case
		when exists(select 1 from sec._User where idMember = sec.Member.idMember) then cast(1 as bit)
		else cast(0 as bit)
	end as isUser
from
	sec.Member
GO
/****** Object:  View [sec].[RoleOfMember]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE view [sec].[RoleOfMember]
as
select
	r.idRole,
	r.name roleName,
	r.description roleDescription,
	m.idMember,
	m.name memberName,
	case
		when exists(select 1 from sec._User where idMember = m.idMember) then cast(1 as bit)
		else cast(0 as bit)
	end as isUser
from
	sec._Role r inner join sec.MemberRole mr 
on 
	r.idRole = mr.idRole inner join sec.Member m
on
	mr.idMember = m.idMember

GO

create view sec.UsersGrant
as
select 
	m.idMember,
	g.idSecObject,
	rm.idRole,
	g.idAccessType,
	m.name,
	g.ObjectName,
	rm.roleName,
	g.accessName
from 
	sec.Members m inner join sec.RoleOfMember rm 
on 
	m.idMember = rm.idMember inner join sec.Grants g 
on 
	rm.idRole = g.idRole 
where
	m.isUser = 1

go

create view sec.GroupsGrant
as
select 
	m.idMember,
	g.idSecObject,
	rm.idRole,
	g.idAccessType,
	m.name,
	g.ObjectName,
	rm.roleName,
	g.accessName
from 
	sec.Members m inner join sec.RoleOfMember rm 
on 
	m.idMember = rm.idMember inner join sec.Grants g 
on 
	rm.idRole = g.idRole 
where
	m.isUser = 0

go

/***************  View  ****************/