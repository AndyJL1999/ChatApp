CREATE PROCEDURE [dbo].[spUserChat_Insert]
	@Id nvarchar(450),
	@UserId nvarchar(450),
	@ChatId nvarchar(450)
AS
begin
	insert into dbo.[UserChat](Id, UserId, ChatId)
	values(@Id, @UserId, @ChatId)
end
