using ChatApp.UI_Library.Models;

namespace Maui_UI_Fiction_Library.API
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; }

        Task<AuthenticatedUser> Authenticate(string email, string password);
        Task<AuthenticatedUser> Register(string name, string email, string password, string phoneNumber);
        Task SignOut();
    }
}