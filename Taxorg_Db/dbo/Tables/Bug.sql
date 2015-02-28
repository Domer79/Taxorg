CREATE TABLE [dbo].[Bug]
(
	[idBug] INT NOT NULL PRIMARY KEY identity,
	[data] nvarchar(max),
	[cause] nvarchar(max),
	[timeLoad] datetime not null default(getdate()),
	[accept] bit not null default 0
)
