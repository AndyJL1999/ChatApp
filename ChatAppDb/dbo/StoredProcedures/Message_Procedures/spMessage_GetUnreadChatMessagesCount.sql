CREATE PROCEDURE [dbo].[spMessage_GetUnreadChatMessagesCount]
	@ChatId nvarchar(450),
	@UserId nvarchar(450)
AS
begin
	select COUNT([Message].Id)
	from dbo.[Message]
	where [Message].ChatId = @ChatId and [Message].UserId != @UserId and [Message].SeenAt is null
end
