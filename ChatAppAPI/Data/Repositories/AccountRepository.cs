using ChatApp.API.DTOs;
using ChatApp.API.Interfaces;
using ChatApp.API.Models;
using ChatApp.DataAccess.Interfaces;
using ChatApp.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChatApp.API.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DatabaseContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly ITokenService _tokenService;
        private readonly IUserEmailStore<IdentityUser> _emailStore;

        public AccountRepository(SignInManager<IdentityUser> signInManager, 
            UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore,
            ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _tokenService = tokenService;
            _emailStore = GetEmailStore();
        }

        public async Task<ServiceResponse<AuthUserDTO>> Register(string name, string email, string password, string phoneNumber)
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
            await _userManager.SetPhoneNumberAsync(user, phoneNumber);

            user.Name = name;

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return new ServiceResponse<AuthUserDTO>
                {
                    Data = new AuthUserDTO
                    {
                        Name = user.Name,
                        Token = await _tokenService.CreateToken(user)
                    },
                    Success = true,
                    Message = "Sign in successful"
                };
            }

            return new ServiceResponse<AuthUserDTO>
            {
                Data = null,
                Success = false,
                Message = "Something went wrong"
            };
        }

        public async Task<ServiceResponse<AuthUserDTO>> SignIn(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (result.Succeeded)
            {
                AppUser user = (AppUser)await _userManager.FindByEmailAsync(email);

                return new ServiceResponse<AuthUserDTO>
                {
                    Data = new AuthUserDTO
                    {
                        Name = user.Name,
                        Email = user.NormalizedEmail,
                        PhoneNumber = user.PhoneNumber,
                        Token = await _tokenService.CreateToken(user)
                    },
                    Success = true,
                    Message = "Sign in successful"
                };
            }

            return new ServiceResponse<AuthUserDTO>
            {
                Data = null,
                Success = false,
                Message = "Failed to sign in"
            };
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
