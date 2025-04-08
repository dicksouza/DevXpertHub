using DevXpertHub.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace DevXpertHub.Web.Extensions;

public static class IdentityInjectionExtensions
{
    /// <summary>
    /// Configura a injeção de dependência para os serviços de Identity na aplicação.
    /// </summary>
    /// <param name="services">O IServiceCollection ao qual os serviços serão adicionados.</param>
    /// <returns>O IServiceCollection para encadeamento.</returns>
    public static IServiceCollection AddIdentityInjection(this IServiceCollection services)
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