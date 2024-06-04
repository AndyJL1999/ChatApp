CREATE PROCEDURE [dbo].[spConnections_Insert]
	@ConnectionId nvarchar(450),
	@ChannelId nvarchar(450),
	@Username nvarchar(256)
AS
begin 
	insert into dbo.[Connections](ConnectionId, ChannelId, Username)
	values(@ConnectionId, @ChannelId, @Username)
end
