CREATE TABLE [dbo].[bank] (
    [bank_id]   INT          IDENTITY (1, 1) NOT NULL,
    [bank_name] VARCHAR (50) NULL,
    [bank_code] VARCHAR (4)  NULL,
    CONSTRAINT [PK_bank] PRIMARY KEY CLUSTERED ([bank_id] ASC)
);





