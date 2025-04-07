namespace DevXpertHub.Web.Extensions;

public static class RoutingConfigurationExtensions
{
    /// <summary>
    /// Mapeia a rota padrão para os controllers e actions da aplicação.
    /// </summary>
    /// <param name="app">A interface IApplicationBuilder para configurar o pipeline de requisição.</param>
    /// <returns>A interface IEndpointRouteBuilder para encadeamento.</returns>
    public static IEndpointRouteBuilder MapDefaultRoutes(this IEndpointRouteBuilder app)
    {
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        return app;
    }
}