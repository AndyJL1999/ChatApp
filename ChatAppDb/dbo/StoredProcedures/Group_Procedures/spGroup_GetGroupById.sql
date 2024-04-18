CREATE PROCEDURE [dbo].[spGroup_GetGroupById]
	@GroupId nvarchar(450)
AS
begin
	select *
	from dbo.[Group]
	where [Group].Id = @GroupId
end
