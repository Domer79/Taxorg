use Taxorg
go

update Settings set value = '1.1.2.0' where name = 'appversion'
go

create table Sessions
(
	sessionId varchar(100) not null primary key
)

create table SessionTaxType
(
	idSessionTaxType int not null primary key identity,
	sessionId varchar(100) not null,
	idTaxType int not null,
	constraint FK_SessionTaxType_Sessions foreign key(sessionId) references Sessions(sessionId) on delete cascade,
	constraint FK_SessionTaxType_TaxType foreign key(idTaxType) references TaxType(idTaxType) on delete cascade
)

create unique index UQ_SessionTaxType on SessionTaxType(sessionId, idTaxType)