CREATE TABLE [dbo].[FileSystem]
(
	idFileSystem INT NOT NULL PRIMARY KEY IDENTITY,
	[fileName] VARCHAR(max) NOT NULL,
	remoteHostName varchar(50) null,
	remoteHostFileName varchar(1000) null,
	remoteHostIpv4 varchar(20) null,
	remoteHostIpv6 varchar(50) null,
	remoteHostMac varchar(50) null,
	contentType varchar(100) null,
	isCompressed bit null
)
