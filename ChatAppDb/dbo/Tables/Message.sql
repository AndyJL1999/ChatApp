CREATE TABLE [dbo].[Message]
(
	[Id] nvarchar(450) NOT NULL PRIMARY KEY,
	[UserId] nvarchar(450) NOT NULL FOREIGN KEY REFERENCES [AspNetUsers]([Id]),
	[GroupId] nvarchar(450) FOREIGN KEY REFERENCES [Group]([Id]),
	[ChatId] nvarchar(450) FOREIGN KEY REFERENCES [Chat]([Id]),
	[Content] nvarchar(1000) NOT NULL,
	[SentAt] datetime2(7) NOT NULL,
	[DeliveredAt] datetime2(7),
	[SeenAt] datetime2(7)
)
