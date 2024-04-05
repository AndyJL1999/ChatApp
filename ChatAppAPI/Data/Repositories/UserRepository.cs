using ChatApp.API.DTOs;
using ChatApp.API.Interfaces;
using ChatApp.API.Models;
using ChatApp.DataAccess.Interfaces;
using ChatApp.DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public UserDTO GetUserByPhone(string number)
        {
            var user = _context.AppUsers.FirstOrDefault(u => u.PhoneNumber == number);

            if(user != null)
            {
                return new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
            }

            return null;
        }
    }
}
