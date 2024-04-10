CREATE PROCEDURE [dbo].[spGroup_Upsert]
	@Id nvarchar(450),
	@Name nvarchar(256)
AS

if((select 1 from dbo.[Group] where Id = @Id) = 1)
begin
	update dbo.[Group]
	set Name = @Name
	where Id = @Id;
end
else
begin
	insert into dbo.[Group](Id, Name)
	values(@Id, @Name)
end
