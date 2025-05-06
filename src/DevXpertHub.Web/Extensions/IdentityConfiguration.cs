using DevXpertHub.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace DevXpertHub.Web.Extensions;

/// <summary>
/// Classe de extensão para configurar serviços no contêiner de injeção de dependência.
/// Contém métodos para configurar o banco de dados, Identity, injeção de dependência de repositórios e serviços,
/// e suporte a MVC com Razor Pages.
/// </summary>
public static class IdentityConfiguration
{
    /// <summary>
    /// Configura a injeção de dependência para os serviços de Identity na aplicação.
    /// </summary>
    /// <param name="services">O IServiceCollection ao qual os serviços serão adicionados.</param>
    /// <returns>O IServiceCollection para encadeamento.</returns>
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddDefaultIdentity<IdentityUser>(options =>
        options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultUI();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        });
        return services;
    }
}