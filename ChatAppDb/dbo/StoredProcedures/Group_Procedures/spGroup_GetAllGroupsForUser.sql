CREATE PROCEDURE [dbo].[spGroup_GetAllGroupsForUser]
	@UserId nvarchar(450)
AS
begin 
	select [Group].Id, [Group].Name
	from dbo.[Group]
	inner join dbo.[UserGroup] on [Group].Id = [UserGroup].GroupId
	where [UserGroup].UserId = @UserId
end
