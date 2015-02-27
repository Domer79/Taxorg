create table Error
(
	idError int not null primary key identity,
	[type] varchar(100) not null,
	[message] varchar(max),
	stackTrace nvarchar(max),
	timeLabel DateTime not null default GetDate()
)
