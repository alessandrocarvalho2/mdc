CREATE TABLE [dbo].[permission] (
    [id]          INT          IDENTITY (1, 1) NOT NULL,
    [description] VARCHAR (50) NULL,
    [menu]        VARCHAR (50) NULL,
    [text_info]   VARCHAR (50) NULL,
    [title]       VARCHAR (50) NULL,
    [subtitle]    VARCHAR (50) NULL,
    [profile_id]  INT          NULL,
    CONSTRAINT [PK_permission] PRIMARY KEY CLUSTERED ([id] ASC)
);

