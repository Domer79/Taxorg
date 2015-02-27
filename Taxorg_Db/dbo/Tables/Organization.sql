CREATE TABLE [dbo].[Organization] (
    [idOrganization] INT           IDENTITY (1, 1) NOT NULL,
    [name]           VARCHAR (900) NULL,
    [shortName]      VARCHAR (200) NULL,
    [addr]           VARCHAR (900) NULL,
    [inn]            VARCHAR (30)  NOT NULL,
    PRIMARY KEY CLUSTERED ([idOrganization] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Organization_inn]
    ON [dbo].[Organization]([inn] ASC);


GO