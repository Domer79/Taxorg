CREATE TABLE [dbo].[Tax] (
    [idTax]          INT      IDENTITY (1, 1) NOT NULL,
    [idOrganization] INT      NOT NULL,
    [idTaxType]      INT      NOT NULL,
    [tax]            MONEY    CONSTRAINT [DF_Tax] DEFAULT ((0)) NOT NULL,
    period YearMonth not null,
	periodName as period.ToString(),
    PRIMARY KEY CLUSTERED ([idTax] ASC),
    CONSTRAINT [FK_Tax_Organization] FOREIGN KEY ([idOrganization]) REFERENCES [dbo].[Organization] ([idOrganization]) ON DELETE CASCADE,
    CONSTRAINT [FK_Tax_TaxType] FOREIGN KEY ([idTaxType]) REFERENCES [dbo].[TaxType] ([idTaxType])
);
go

create unique index UQ_Tax_idOrganization_idTaxType_dateLoad on Tax(idOrganization, idTaxType, period)
go
