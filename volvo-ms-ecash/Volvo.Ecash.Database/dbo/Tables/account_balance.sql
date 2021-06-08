CREATE TABLE [dbo].[account_balance] (
    [id]                 INT             IDENTITY (1, 1) NOT NULL,
    [bank_account_id]    INT             NULL,
    [date]               DATE            NULL,
    [balance]            NUMERIC (20, 2) NULL,
    [document_upload_id] INT             NULL,
    CONSTRAINT [PK_account_balance] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_account_balance_document_upload] FOREIGN KEY ([document_upload_id]) REFERENCES [dbo].[document_upload] ([id])
);



