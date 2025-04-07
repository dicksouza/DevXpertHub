namespace DevXpertHub.Api.Extensions;

public static class ControllersConfigurationExtensions
{
    /// <summary>
    /// Adiciona a configuração dos controllers e a convenção para rotas em lowercase.
    /// </summary>
    /// <param name="services">A interface IServiceCollection para adicionar os serviços.</param>
    public static void AddControllersConfiguration(this IServiceCollection services)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.AppendTrailingSlash = false;
        });
        //services.AddControllers(options =>
        //{
        //    options.Conventions.Add(new RouteTokenTransformerConvention(new LowerCaseRouteTransformer()));
        //});
    }
}