CREATE PROCEDURE [dbo].[spMessage_Insert]
	@Id nvarchar(450),
	@UserId nvarchar(450),
	@GroupId nvarchar(450) = NULL,
	@ChatId nvarchar(450) = NULL,
	@Content nvarchar(1000),
	@SentAt datetime2(7) = NULL,
	@DeliveredAt datetime2(7) = NULL,
	@SeenAt datetime2(7) = NULL
AS
begin 
	insert into dbo.[Message](Id, UserId, GroupId, ChatId, Content, SentAt, DeliveredAt, SeenAt)
	values(@Id, @UserId, @GroupId, @ChatId, @Content, @SentAt, @DeliveredAt, @SeenAt)
end
