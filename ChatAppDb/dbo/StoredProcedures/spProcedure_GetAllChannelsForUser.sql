CREATE PROCEDURE [dbo].[spProcedure_GetAllChannelsForUser]
	@UserId nvarchar(450)

AS
begin
	select [Chat].Id, [Chat].Name, 'Chat' as 'Type'
	from dbo.[Chat]
	inner join dbo.[UserChat] on [Chat].Id = [UserChat].ChatId
	where [UserChat].UserId = @UserId
	union
	select [Group].Id, [Group].Name, 'Group' as 'Type'
	from dbo.[Group]
	inner join dbo.[UserGroup] on [Group].Id = [UserGroup].GroupId
	where [UserGroup].UserId = @UserId
	order by 'Type' desc
end
