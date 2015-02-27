CREATE TABLE [dbo].[FsFile]
(
	[idFsFile] INT NOT NULL PRIMARY KEY identity,
	idFileSystem int not null unique,
	data varbinary(max),
	constraint FK_FsFile_FileSystem foreign key (idFileSystem) references FileSystem (idFileSystem) on delete cascade
)