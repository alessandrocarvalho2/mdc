CREATE TABLE [dbo].[bank_account] (
    [id]                INT             IDENTITY (1, 1) NOT NULL,
    [account]           VARCHAR (50)    NULL,
    [agency]            VARCHAR (50)    NULL,
    [nickname]          VARCHAR (50)    NULL,
    [bank_id]           INT             NOT NULL,
    [minimum_balance]   NUMERIC (20, 2) NULL,
    [kpi_target]        NUMERIC (20, 2) NULL,
    [main_account]      BIT             NULL,
    [balance_tolerance] NUMERIC (20, 2) NULL,
    CONSTRAINT [PK_bank_account] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_bank_account_bank] FOREIGN KEY ([bank_id]) REFERENCES [dbo].[bank] ([bank_id])
);











