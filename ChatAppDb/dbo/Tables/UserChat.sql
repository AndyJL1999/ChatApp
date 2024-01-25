CREATE TABLE [dbo].[UserChat]
(
	[Id] nvarchar(450) NOT NULL PRIMARY KEY,
	[UserId] nvarchar(450) NOT NULL FOREIGN KEY REFERENCES [AspNetUsers]([Id]),
	[ChatId] nvarchar(450) NOT NULL FOREIGN KEY REFERENCES [Chat]([Id])
)
