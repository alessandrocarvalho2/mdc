CREATE TABLE [dbo].[user] (
    [user_id]          INT           IDENTITY (1, 1) NOT NULL,
    [login]            VARCHAR (50)  NULL,
    [password]         VARCHAR (300) NULL,
    [update_at]        DATE          NULL,
    [create_at]        DATE          NULL,
    [active]           BIT           NULL,
    [refresh_token_id] INT           NULL,
    CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED ([user_id] ASC)
);





