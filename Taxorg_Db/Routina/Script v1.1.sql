use Taxorg
go

if exists(select 1 from sys.columns where name = 'period' and object_id = object_id(N'Tax'))
	and exists(select 1 from sys.columns where name = 'periodName' and object_id = object_id(N'Tax'))
raiserror('Остановка скрипта', 20, 1) with log


begin transaction

alter table tax
add 
	period YearMonth,
	periodName as period.ToString()
go

update Tax set period = dbo.YearMonthCreateByDateTime(dateLoad) where idTax = idTax

drop index UQ_Tax_idOrganization_idTaxType_dateLoad on Tax

alter table Tax
drop column dateLoad

CREATE UNIQUE NONCLUSTERED INDEX [UQ_Tax_idOrganization_idTaxType_dateLoad] ON [dbo].[Tax]
(
	[idOrganization] ASC,
	[idTaxType] ASC,
	[period] ASC
)

select * from Tax

commit transaction


------------------------------------------------02.03.2015---------------------------------------
USE [master]
GO

/****** Object:  Database [Taxorg_Temp]    Script Date: 02.03.2015 9:35:14 ******/
CREATE DATABASE [Taxorg_Temp]
GO

use [Taxorg_Temp]
go

---Organization
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Organization](
	[idOrganization] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](900) NULL,
	[shortName] [varchar](200) NULL,
	[addr] [varchar](900) NULL,
	[inn] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idOrganization] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

---TaxType
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TaxType](
	[idTaxType] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](100) NOT NULL,
	[name] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[idTaxType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

---Tax
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tax](
	[idTax] [int] IDENTITY(1,1) NOT NULL,
	[idOrganization] [int] NOT NULL,
	[idTaxType] [int] NOT NULL,
	[tax] [money] NOT NULL CONSTRAINT [DF_Tax]  DEFAULT ((0)),
	[period] VARCHAR(4000)
PRIMARY KEY CLUSTERED 
(
	[idTax] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Tax]  WITH CHECK ADD  CONSTRAINT [FK_Tax_Organization] FOREIGN KEY([idOrganization])
REFERENCES [dbo].[Organization] ([idOrganization])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Tax] CHECK CONSTRAINT [FK_Tax_Organization]
GO

ALTER TABLE [dbo].[Tax]  WITH CHECK ADD  CONSTRAINT [FK_Tax_TaxType] FOREIGN KEY([idTaxType])
REFERENCES [dbo].[TaxType] ([idTaxType])
GO

ALTER TABLE [dbo].[Tax] CHECK CONSTRAINT [FK_Tax_TaxType]
GO


use Taxorg_Temp
go

begin transaction
set identity_insert Organization on
Insert into Organization(idOrganization, name, shortName, addr, inn) select idOrganization, name, shortName, addr, inn from Taxorg.dbo.Organization
set identity_insert Organization off

set identity_insert TaxType on
Insert into TaxType(idTaxType, code, name) select idTaxType, code, name from Taxorg.dbo.TaxType
set identity_insert TaxType off

set identity_insert Tax on
Insert into Tax(idTax, idOrganization, idTaxType, tax, period) select idTax, idOrganization, idTaxType, Tax, cast(datepart(MM, dateLoad) as varchar(2)) + '.' + cast(DATEPART(YY, dateLoad) as varchar(4)) from Taxorg.dbo.Tax
set identity_insert Tax off

commit
go

------------------------------------------------02.03.2015---------------------------------------