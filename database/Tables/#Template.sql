-------------------------------------
--
-- 
--
--
-------------------------------------

CREATE TABLE [dbo].[{{TABLE_NAME}}] (

    [{TABLE_NAME}Id] uniqueidentifier NOT NULL,

	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](32) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](32) NOT NULL,
    [ExpirationDate] [datetime2](7) NULL,

     CONSTRAINT [PK_{TABLE_NAME}] PRIMARY KEY CLUSTERED ([{TABLE_NAME}Id])
)

