CREATE TABLE [dbo].[document_upload] (
    [id]               INT          IDENTITY (1, 1) NOT NULL,
    [filename]         VARCHAR (50) NULL,
    [uploaded_at]      DATE         NULL,
    [selected_account] INT          NULL,
    CONSTRAINT [PK_document_upload] PRIMARY KEY CLUSTERED ([id] ASC)
);

