namespace DevXpertHub.Web.Extensions;

/// <summary>
/// Classe de extensão para configurar serviços no contêiner de injeção de dependência.
/// Contém métodos para configurar o banco de dados, Identity, injeção de dependência de repositórios e serviços,
/// e suporte a MVC com Razor Pages.
/// </summary>
public static class MvcConfiguration
{
    /// <summary>
    /// Adiciona a configuração do MVC com suporte a views.
    /// </summary>
    /// <param name="services">A interface IServiceCollection para adicionar os serviços.</param>
    /// <returns>A interface IServiceCollection para encadeamento.</returns>
    public static IServiceCollection AddMvcConfiguration(this IServiceCollection services)
    {
        services.AddControllersWithViews().
                 AddSessionStateTempDataProvider();
        services.AddRazorPages().
                 AddSessionStateTempDataProvider();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expiração da sessão
            options.Cookie.HttpOnly = true; // Garante que o cookie só seja acessível via HTTP
            options.Cookie.IsEssential = true; // Necessário para cookies funcionarem sem consentimento explícito
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Garante que o cookie seja enviado apenas via HTTPS
        });

        return services;
    }

    /// <summary>
    /// Mapeia endpoints da aplicação, incluindo arquivos estáticos, rotas de controladores e Razor Pages.
    /// </summary>
    /// <param name="app">A instância de <see cref="WebApplication"/> a ser configurada.</param>
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();
        app.MapRazorPages()
            .WithStaticAssets();
    }
}