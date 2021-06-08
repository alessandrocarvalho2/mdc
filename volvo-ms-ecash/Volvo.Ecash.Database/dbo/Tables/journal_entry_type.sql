CREATE TABLE [dbo].[journal_entry_type] (
    [id]          INT          IDENTITY (1, 1) NOT NULL,
    [description] VARCHAR (50) NULL,
    CONSTRAINT [PK_journal_entry_type] PRIMARY KEY CLUSTERED ([id] ASC)
);

