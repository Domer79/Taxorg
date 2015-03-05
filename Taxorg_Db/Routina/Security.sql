use Taxorg_Temp
go

set transaction isolation level repeatable read
begin transaction
go

create schema sec
go

create table sec._Role
(
	idRole int not null primary key identity,
	name varchar(200) not null
)

create unique index UQ_Role_Name on sec._Role(name)

create table sec.Member
(
	idMember int not null primary key identity,
	name varchar(200)
)

create unique index UQ_Member_Name on sec.Member(name)
go

create table sec._Group
(
	idMember int not null,
--	name varchar(200) not null,
	description varchar(max),
	constraint FK_Group_Member foreign key (idMember) references sec.Member(idMember) on delete cascade
)

create unique index UQ_Group_idMember on sec._Group(idMember)
go
--create unique index UQ_Group_Name on sec._Group(name)
--go

create table sec._User
(
	idMember int not null,
--	login varchar(100) not null,
	sid varchar(100) not null,
	displayName varchar(200),
	email varchar(100)
	constraint FK_User_Member foreign key (idMember) references sec.Member(idMember) on delete cascade
)

create unique index UQ_User_idMember on sec._User(idMember)
--create unique index UQ_User_Login on sec._User(login)
create unique index UQ_User_Sid on sec._User(sid)
go

create table sec.AccessType
(
	idAccessType int not null primary key,
	description varchar(300) not null
)

create unique index UQ_AccessType_Description on sec.AccessType(description)
go

--Объект безопасности. Это может быть любой объект, в отношении которого необходимо настроить безопасность
create table sec.SecObject
(
	idSecObject int not null primary key identity,
	ObjectName varchar(200) not null,
	--Дополнительные поля, уникально идентифицирующие объект
	type1 varchar(100),
	type2 varchar(100),
	type3 varchar(100),
	type4 varchar(100),
	type5 varchar(100),
	type6 varchar(100),
	type7 varchar(100)
)

create unique index UQ_SecObject_ObjectName on sec.SecObject(ObjectName, type1, type2, type3, type4, type5, type6, type7)
go

--create trigger sec.OnSecObjectInsertUpdate on sec.SecObject
--after insert, update
--as
--begin
--	if exists
--		(
--			select 1 from 
--				sec.SecObject so inner join inserted ins 
--			on 
--				so.ObjectName = ins.ObjectName and 
--				so.type1 = ins.type1 and 
--				so.type2 = ins.type2 and
--				so.type3 = ins.type3 and
--				so.type4 = ins.type4 and
--				so.type5 = ins.type5 and
--				so.type6 = ins.type6 and
--				so.type7 = ins.type7 and
--				so.type8 = ins.type8 and
--				so.type9 = ins.type9
--		)
--	raiserror('secObject_not_unique', 16, 10)
--end
--go

create table sec._Grant
(
	idGrants int not null primary key identity,
	idSecObject int not null,
	idRole int not null,
	idAccessType int not null,
	constraint FK_Grant_SecObject foreign key(idSecObject) references sec.SecObject(idSecObject) on delete cascade,
	constraint FK_Grant_Role foreign key(idRole) references sec._Role(idRole) on delete cascade,
	constraint FK_Grant_AccessType foreign key(idAccessType) references sec.AccessType(idAccessType)
)

create unique index UQ_Grant on sec._Grant(idSecObject, idRole, idAccessType)
go


--Создание связей

create table sec.MemberRole
(
	idMember int not null,
	idRole int not null,
	constraint PK_MemberRole primary key(idMember, idRole),
	constraint FK_MemberRole_Member foreign key(idMember) references sec.Member(idMember) on delete cascade, --При удалении участника безопасности также удаляем и все его связи с ролями
	constraint FK_MemberRole_Role foreign key(idRole) references sec._Role(idRole) on delete cascade --При удалении роли таккже удаляем все ее связи с участниками безопасности
)

--raiserror('Тестовая ошибка', 20, 10) with log
commit