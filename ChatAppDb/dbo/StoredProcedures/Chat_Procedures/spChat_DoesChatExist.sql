CREATE PROCEDURE [dbo].[spChat_DoesChatExist]
	@UserId nvarchar(450),
	@RecipientId nvarchar(450)
AS
begin 
	select case when exists (select [UserChat].ChatId 
	from dbo.[UserChat] 
	where [UserChat].UserId = @UserId or [UserChat].UserId = @RecipientId
	group by [UserChat].ChatId 
	having count(*) > 1)
	then cast(1 as bit) else cast(0 as bit) end as 'DoesExist'
end
