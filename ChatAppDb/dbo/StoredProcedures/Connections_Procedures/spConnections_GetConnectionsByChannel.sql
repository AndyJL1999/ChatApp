CREATE PROCEDURE [dbo].[spConnections_GetConnectionsByChannel]
	@ChannelId nvarchar(450)
AS
begin
	select *
	from dbo.[Connections]
	where ChannelId = @ChannelId;
end
