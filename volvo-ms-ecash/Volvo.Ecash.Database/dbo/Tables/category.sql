CREATE TABLE [dbo].[category] (
    [id]          INT          IDENTITY (1, 1) NOT NULL,
    [description] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_category] PRIMARY KEY CLUSTERED ([id] ASC)
);

