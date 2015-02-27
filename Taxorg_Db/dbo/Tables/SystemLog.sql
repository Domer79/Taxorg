CREATE TABLE [dbo].[SystemLog]
(
	[idSystemLogs] INT NOT NULL PRIMARY KEY identity,
	[log] nvarchar(max) not null,
	[timeLabel] datetime not null default(getdate())
)
