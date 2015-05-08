use Taxorg
go

update Settings set value = '2.1.0' where name = 'appversion'
go

begin try
begin transaction
/*SessionTaxType*/
ALTER TABLE [dbo].[SessionTaxType] DROP CONSTRAINT [FK_SessionTaxType_TaxType]

ALTER TABLE [dbo].[SessionTaxType] DROP CONSTRAINT [FK_SessionTaxType_Sessions]

/****** Object:  Table [dbo].[SessionTaxType]    Script Date: 08.05.2015 10:32:13 ******/
DROP TABLE [dbo].[SessionTaxType]

/****** Object:  Table [dbo].[SessionTaxType]    Script Date: 08.05.2015 10:32:13 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[SessionTaxType](
	[idSessionTaxType] [int] IDENTITY(1,1) NOT NULL,
	[sessionId] [varchar](24) NOT NULL,
	[idTaxType] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idSessionTaxType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING ON

/*SessionTaxType*/

/*Sessions*/

DROP TABLE [dbo].[Sessions]

CREATE TABLE [dbo].[Sessions] (
  [SessionId] varchar(24) NOT NULL,
  [Created] smalldatetime NOT NULL,
  [Expires] smalldatetime NOT NULL,
  [LockDate] smalldatetime NOT NULL,
  [LockId] int NOT NULL,
  [Locked] bit CONSTRAINT [DF_Sessions_Locked] DEFAULT 0 NOT NULL,
  [ItemContent] varbinary(max) NULL,
  [UserId] int NOT NULL,
  CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED ([SessionId])
)
ON [PRIMARY]

/*Sessions*/

ALTER TABLE [dbo].[SessionTaxType]  WITH CHECK ADD  CONSTRAINT [FK_SessionTaxType_Sessions] FOREIGN KEY([sessionId])
REFERENCES [dbo].[Sessions] ([sessionId])
ON DELETE CASCADE

ALTER TABLE [dbo].[SessionTaxType] CHECK CONSTRAINT [FK_SessionTaxType_Sessions]

ALTER TABLE [dbo].[SessionTaxType]  WITH CHECK ADD  CONSTRAINT [FK_SessionTaxType_TaxType] FOREIGN KEY([idTaxType])
REFERENCES [dbo].[TaxType] ([idTaxType])
ON DELETE CASCADE

ALTER TABLE [dbo].[SessionTaxType] CHECK CONSTRAINT [FK_SessionTaxType_TaxType]

commit
end try
begin catch

rollback
select ERROR_MESSAGE()

end catch