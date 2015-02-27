CREATE TABLE [dbo].[TaxType] (
    [idTaxType] INT           IDENTITY (1, 1) NOT NULL,
	[code]		VARCHAR (100) NOT NULL,
    [name]      VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([idTaxType] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_TaxType_Code]
    ON [dbo].[TaxType]([code] ASC);

