namespace Maui_UI_Fiction_Library.API
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; }

        Task<string> Authenticate(string email, string password);
        Task<string> Register(string name, string email, string password, string phoneNumber);
        Task<string> SignOut();
    }
}