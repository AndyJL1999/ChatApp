CREATE PROCEDURE [dbo].[spMessage_GetLastMessageSent]
	@ChannelId nvarchar(450)
AS

begin
	select top 1 [Message].Content
	from dbo.[Message]
	where [Message].ChatId = @ChannelId or [Message].GroupId = @ChannelId
	order by [Message].SentAt desc
end
