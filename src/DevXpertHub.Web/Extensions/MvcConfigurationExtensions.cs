namespace DevXpertHub.Web.Extensions;

public static class MvcConfigurationExtensions
{
    /// <summary>
    /// Adiciona a configuração do MVC com suporte a views.
    /// </summary>
    /// <param name="services">A interface IServiceCollection para adicionar os serviços.</param>
    /// <returns>A interface IServiceCollection para encadeamento.</returns>
    public static IServiceCollection AddMvcConfiguration(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        return services;
    }
}