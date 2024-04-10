CREATE PROCEDURE [dbo].[spChat_GetAllChatsForUser]
	@UserId nvarchar(450)
AS
begin
	select [Chat].Id, [Chat].Name
	from dbo.[Chat]
	inner join dbo.[UserChat] on [Chat].Id = [UserChat].ChatId
	where [UserChat].UserId = @UserId
end

