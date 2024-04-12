CREATE PROCEDURE [dbo].[spMessage_GetAllFromChannel]
	@ChannelId nvarchar(450)
AS
begin
	select [Message].Id, [Message].UserId, [Message].Content, [Message].SentAt 
	from dbo.[Message]
	where [Message].ChatId = @ChannelId or [Message].GroupId = @ChannelId
	order by [Message].SentAt
end
