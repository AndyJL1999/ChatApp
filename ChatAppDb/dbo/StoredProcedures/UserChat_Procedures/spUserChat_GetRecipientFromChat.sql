CREATE PROCEDURE [dbo].[spUserChat_GetRecipientFromChat]
	@CurrentUserId nvarchar(450),
	@ChatId nvarchar(450)
AS
begin
	select [UserChat].UserId
	from dbo.[UserChat]
	where ChatId = @ChatId and UserId != @CurrentUserId
end
