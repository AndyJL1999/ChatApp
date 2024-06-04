CREATE TABLE [dbo].[Connections]
(
	[ConnectionId] NVARCHAR(450) NOT NULL PRIMARY KEY, 
    [ChannelId] NVARCHAR(450) NOT NULL, 
    [Username] NVARCHAR(256) NOT NULL
)
