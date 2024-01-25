CREATE TABLE [dbo].[UserGroup]
(
	[Id] nvarchar(450) NOT NULL PRIMARY KEY,
	[UserId] nvarchar(450) NOT NULL FOREIGN KEY REFERENCES [AspNetUsers]([Id]),
	[GroupId] nvarchar(450) NOT NULL FOREIGN KEY REFERENCES [Group]([Id])
)
