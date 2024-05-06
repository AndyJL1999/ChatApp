using ChatApp.API.Data;
using ChatApp.API.Data.Repositories;
using ChatApp.API.Interfaces;
using ChatApp.API.Services;
using ChatApp.DataAccess.Data;
using ChatApp.DataAccess.DataAccess;
using ChatApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Extensions
{
    public static class ServiceExtentions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(config.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme, e.g. \"bearer {token} \"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
            services.AddSingleton<IChatData, ChatData>();
            services.AddSingleton<IGroupData, GroupData>();
            services.AddSingleton<IMessageData, MessageData>();
            services.AddSingleton<IUserData, UserData>();
            
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    //.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed((hosts) => true));
            });

            services.AddSignalR();

            return services;
        }
    }
}
