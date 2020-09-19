using System;
using System.Text;
using iHRS.Application.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace iHRS.Infrastructure.Auth
{
    public static class Extensions
    {
        private static TModel GetOptions<TModel>(this IServiceCollection services, string sectionName) where TModel : new()
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            var model = new TModel();
            configuration?.GetSection(sectionName).Bind(model);
            return model;
        }

        public static long ToTimestamp(this DateTime dateTime) => new DateTimeOffset(dateTime).ToUnixTimeSeconds();

        public static IServiceCollection AddJwt(this IServiceCollection services)
        {
            var options = services.GetOptions<JwtOptions>("jwt");
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IPasswordHasher<IPasswordService>, PasswordHasher<IPasswordService>>();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.IssuerSigningKey))
            };

            services
                 .AddAuthentication(o =>
                 {
                     o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                     o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                     o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                 })
                     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                 {
                     o.TokenValidationParameters = tokenValidationParameters;
                 });

            services.AddSingleton(options);
            services.AddSingleton(tokenValidationParameters);

            return services;
        }
    }
}
