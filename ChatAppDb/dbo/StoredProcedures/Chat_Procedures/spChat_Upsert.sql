﻿CREATE PROCEDURE [dbo].[spChat_Upsert]
	@Id nvarchar(450),
	@Name nvarchar(256)
AS

if((select 1 from dbo.[Chat] where Id = @Id) = 1)
begin
	update dbo.[Chat]
	set Name = @Name
	where Id = @Id;
end
else
begin
	insert into dbo.[Chat](Id, Name)
	values(@Id, @Name)
end

