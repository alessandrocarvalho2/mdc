CREATE TABLE [dbo].[domain] (
    [id]                INT          IDENTITY (1, 1) NOT NULL,
    [operation_id]      INT          NOT NULL,
    [category_id]       INT          NOT NULL,
    [bank_account_id]   INT          NOT NULL,
    [description]       VARCHAR (50) NOT NULL,
    [aprovation_needed] BIT          NOT NULL,
    [visible]           BIT          NOT NULL,
    [in_out]            CHAR (3)     NOT NULL,
    CONSTRAINT [PK_domain] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [CK_in_out] CHECK ([in_out]='IN' OR [in_out]='OUT'),
    CONSTRAINT [FK_domain_bank_account] FOREIGN KEY ([bank_account_id]) REFERENCES [dbo].[bank_account] ([id]),
    CONSTRAINT [FK_domain_category] FOREIGN KEY ([category_id]) REFERENCES [dbo].[category] ([id]),
    CONSTRAINT [FK_domain_operation] FOREIGN KEY ([operation_id]) REFERENCES [dbo].[operation] ([id]),
    CONSTRAINT [UQ_description] UNIQUE NONCLUSTERED ([description] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Only accept IN or OUT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'domain', @level2type = N'CONSTRAINT', @level2name = N'CK_in_out';

