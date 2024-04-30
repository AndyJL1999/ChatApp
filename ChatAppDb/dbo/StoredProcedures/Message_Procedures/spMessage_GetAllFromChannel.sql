CREATE PROCEDURE [dbo].[spMessage_GetAllFromChannel]
	@ChannelId nvarchar(450),
	@Limit int,
	@Offset int
AS
begin
	select [Message].Id, [AspNetUsers].Name as 'UserName', [Message].UserId, [Message].Content, [Message].SentAt 
	from dbo.[Message]
	inner join dbo.[AspNetUsers]
	on [AspNetUsers].Id = [Message].UserId
	where [Message].ChatId = @ChannelId or [Message].GroupId = @ChannelId
	order by [Message].SentAt desc
	offset @Offset rows fetch next @Limit rows only;
end
