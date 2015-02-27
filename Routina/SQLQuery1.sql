use master
go

Create database Taxorg
go

use Taxorg
go

create table Organization
(
	idOrganization int not null primary key identity,
	name varchar(900) not null,
	shortName varchar(200) not null,
	addr varchar(900) not null,
	inn varchar(30) not null
)

Create unique index UQ_Organization_Name on Organization (name asc)
Create unique index UQ_Organization_shortName on Organization (shortName asc)
Create unique index UQ_Organization_inn on Organization (inn asc)
Create unique index UQ_Organization_addr on Organization (addr asc)

create table TaxType
(
	idTaxType int not null primary key identity,
	name varchar(100)
)

create unique index UQ_TaxType_name on TaxType (name asc)

create table Tax
(
	idTax int not null primary key identity,
	idOrganization int null,
	idTaxType int not null,
	tax money not null,
	dateLoad datetime not null
	constraint FK_Tax_Organization foreign key (idOrganization) references Organization(idOrganization) on delete cascade,
	constraint FK_Tax_TaxType foreign key (idTaxType) references TaxType (idTaxType)
)

alter table Tax
add constraint DF_Tax default(0) for tax