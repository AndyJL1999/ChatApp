using ChatApp.DataAccess.Models;

namespace ChatApp.API.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
