using ChatApp.UI_Library.API;
using ChatApp.UI_Library.API.Interfaces;
using ChatApp.UI_Library.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.UI_Library
{
    public static class SetUp
    {
        public static IServiceCollection AddLibraryServices(this IServiceCollection services)
        {
            return services
                .AddMemoryCache()
                .AddSingleton<IAuthenticatedUser, AuthenticatedUser>()
                .AddSingleton<IApiHelper, ApiHelper>()
                .AddScoped<ICacheService, CacheService>()
                .AddScoped<IAuthHelper, AuthHelper>()
                .AddScoped<IChatHelper, ChatHelper>()
                .AddScoped<IUserHelper, UserHelper>()
                .AddScoped<IMessageHelper, MessageHelper>()
                .AddScoped<IGroupHelper, GroupHelper>();
        }
    }
}
