using ChatApp.UI_Library.Models;

namespace ChatApp.UI_Library.API.Interfaces
{
    public interface IApiHelper
    {
        internal HttpClient ApiClient { get; }
    }
}