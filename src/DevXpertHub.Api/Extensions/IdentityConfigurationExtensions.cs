using DevXpertHub.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace DevXpertHub.Api.Extensions;

public static class IdentityConfigurationExtensions
{
    /// <summary>
    /// Adiciona a configuração do Identity para usuários e roles.
    /// </summary>
    /// <param name="services">A interface IServiceCollection para adicionar os serviços.</param>
    public static void AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }
}