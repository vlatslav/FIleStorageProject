using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL;
using BAL.Entity.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace PL.Extentions
{
    public static class ServiceExtentions
    {
        public static void ConfigureIdentity(this IServiceCollection service)
        {
            var builder = service.AddIdentity<User, IdentityRole>(x =>
            {
                x.Password.RequireDigit = true;
                x.Password.RequireLowercase = true;
                x.Password.RequireUppercase = true;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequiredLength = 8;
                x.User.RequireUniqueEmail = true;
            });
            builder = new IdentityBuilder(builder.UserType,typeof(IdentityRole),builder.Services);
            builder.AddEntityFrameworkStores<FileDBContext>()
                .AddDefaultTokenProviders();

        }

        public static void ConfigureSwagger(this IServiceCollection service)
        {
            service.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo {Title = "Code Maze API", Version = "v1"});
            });

        }
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration
            configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("SECRET");
            services.AddAuthentication(opt => {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                        ValidAudience = jwtSettings.GetSection("validAudience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });
        }


    }
}
