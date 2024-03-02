using Microsoft.AspNetCore.Identity;

namespace ChatApp.API.Interfaces
{
    public interface IAccountRepository
    {
        Task<SignInResult> SignIn(string email, string password);
        Task<IdentityResult> Register(string name, string email, string password);
        Task SignOut();
    }
}
