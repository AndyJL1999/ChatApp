namespace ChatApp.API.DTOs
{
    public class CreateGroupDTO
    {
        public string GroupName { get; set; }
        public List<string> PhoneNumbers { get; set; }
    }
}
