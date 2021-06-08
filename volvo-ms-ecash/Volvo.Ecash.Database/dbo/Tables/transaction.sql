CREATE TABLE [dbo].[transaction] (
    [id]                    INT             IDENTITY (1, 1) NOT NULL,
    [bank_account_id]       INT             NULL,
    [operation_id]          INT             NULL,
    [journal_entry_type_id] INT             NULL,
    [description]           VARCHAR (50)    NULL,
    [distortion]            BIT             NULL,
    [amount]                NUMERIC (20, 2) NULL,
    [date]                  DATE            NULL,
    [document_upload_id]    INT             NULL,
    CONSTRAINT [PK_transaction] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_transaction_document_upload] FOREIGN KEY ([document_upload_id]) REFERENCES [dbo].[document_upload] ([id])
);







