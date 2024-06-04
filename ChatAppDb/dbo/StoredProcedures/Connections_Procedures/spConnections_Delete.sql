CREATE PROCEDURE [dbo].[spConnections_Delete]
	@ConnectionId nvarchar(450)
AS
begin 
	delete
	from dbo.[Connections]
	where ConnectionId = @ConnectionId;
end
