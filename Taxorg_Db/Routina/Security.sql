use Taxorg_Temp
go

set transaction isolation level repeatable read
begin transaction
go

create schema sec
go

create table sec._Role
(
	irRole int not null primary key identity,
	name varchar(200) not null
)

create unique index UQ_Role_Name on sec._Role(name)

create table sec.SecMember
(
	idSecMember int not null primary key identity
)

create table sec._Group
(
	idSecMember int not null,
	name varchar(200) not null,
	constraint FK_Group_SecMember foreign key (idSecMember) references sec.SecMember(idSecMember)
)

create unique index UQ_Group_idSecMember on sec._Group(idSecMember)
go
create unique index UQ_Group_Name on sec._Group(name)
go

create table sec._User
(
	idSecMember int not null,
	login varchar(100) not null,
	sid varchar(100) not null,
	displayName varchar(200),
	email varchar(100)
	constraint FK_User_SecMember foreign key (idSecMember) references sec.SecMember(idSecMember)
)

create unique index UQ_User_idSecMember on sec._User(idSecMember)
create unique index UQ_User_Login on sec._User(login)
create unique index UQ_User_Sid on sec._User(sid)
go

create table sec.AccessType
(
	idAccessType int not null primary key,
	description varchar(300) not null
)

create unique index UQ_AccessType_Description on sec.AccessType(description)
go

create table sec.SecObject
(
	idSecObject int not null primary key identity,
	ObjectName varchar(900) not null,
	type1 varchar(max),
	type2 varchar(max),
	type3 varchar(max),
	type4 varchar(max),
	type5 varchar(max),
	type6 varchar(max),
	type7 varchar(max),
	type8 varchar(max),
	type9 varchar(max),
)

create unique index UQ_SecObject_ObjectName on sec.SecObject(ObjectName)
go

create table sec._Grant
(
	idGrants int not null primary key identity,
	idSecObjects int not null,
	idRoles int not null,
	idAccessType int not null
)

create unique index UQ_Grant
--Связь



raiserror('Тестовая ошибка', 20, 10) with log
commit