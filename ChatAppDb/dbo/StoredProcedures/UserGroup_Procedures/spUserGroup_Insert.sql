CREATE PROCEDURE [dbo].[spUserGroup_Insert]
	@Id nvarchar(450),
	@UserId nvarchar(450),
	@GroupId nvarchar(450)
AS
begin
	insert into dbo.[UserGroup](Id, UserId, GroupId)
	values(@Id, @UserId, @GroupId)
end
