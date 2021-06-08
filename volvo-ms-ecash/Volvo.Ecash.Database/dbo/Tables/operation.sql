CREATE TABLE [dbo].[operation] (
    [id]          INT          IDENTITY (1, 1) NOT NULL,
    [code]        VARCHAR (10) NULL,
    [description] VARCHAR (50) NULL,
    CONSTRAINT [PK_operation] PRIMARY KEY CLUSTERED ([id] ASC)
);

