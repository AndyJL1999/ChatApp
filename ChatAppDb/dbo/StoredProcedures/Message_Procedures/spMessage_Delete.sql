CREATE PROCEDURE [dbo].[spMessage_Delete]
	@Id nvarchar(450)
AS
begin 
	delete
	from dbo.[Message]
	where Id = @Id;
end
