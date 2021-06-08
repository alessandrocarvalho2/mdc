CREATE TABLE [dbo].[refresh_token] (
    [token_refresh] VARCHAR (300) NULL,
    [token_jwt]     VARCHAR (500) NULL,
    [expiry_date]   DATE          NULL,
    [invalidated]   BIT           NULL,
    [jwt_id]        VARCHAR (300) NULL,
    [id]            INT           IDENTITY (1, 1) NOT NULL,
    [create_at]     DATE          NULL,
    [update_at]     DATE          NULL,
    CONSTRAINT [PK_refresh_token] PRIMARY KEY CLUSTERED ([id] ASC)
);

