using DevXpertHub.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DevXpertHub.Api.Extensions;

public static class JwtAuthenticationExtensions
{
    /// <summary>
    /// Adiciona a configuração da autenticação JWT.
    /// </summary>
    /// <param name="services">A interface IServiceCollection para adicionar os serviços.</param>
    /// <param name="configuration">A interface IConfiguration para acessar as configurações.</param>
    /// <exception cref="InvalidOperationException">Lançada se as configurações JWT não estiverem corretas.</exception>
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettingsSection = configuration.GetSection("JwtSettings");
        services.Configure<JwtSettings>(jwtSettingsSection);

        var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
        if (jwtSettings == null
            || string.IsNullOrEmpty(jwtSettings.SecretKey)
            || string.IsNullOrEmpty(jwtSettings.Issuer)
            || string.IsNullOrEmpty(jwtSettings.Audience)
            || jwtSettings.ExpirationTime <= 0)
        {
            throw new InvalidOperationException("JWT settings are not configured properly.");
        }

        var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey)
            };
        });
    }
}