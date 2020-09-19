-------------------------------------
--
-- 
--
--
-------------------------------------

CREATE TABLE [dbo].[Room] (
    [RoomId] uniqueidentifier NOT NULL,
    [RoomNumber] nvarchar(10) NULL,



	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](32) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](32) NOT NULL,
    [ExpirationDate] [datetime2](7) NULL,

    CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED ([RoomId])
)

