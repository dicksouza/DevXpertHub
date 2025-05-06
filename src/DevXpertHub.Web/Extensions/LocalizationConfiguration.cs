using DevXpertHub.Web.Adapters;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace DevXpertHub.Web.Extensions;

/// <summary>
/// Classe responsável por configurar a localização (localization) da aplicação.
/// </summary>
public static class LocalizationConfiguration
{
    public static void AddLocalizationConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
        services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();
    }

    /// <summary>
    /// Configura a localização da aplicação, definindo as culturas suportadas e a cultura padrão.
    /// </summary>
    /// <param name="app">Instância do WebApplicationBuilder utilizada para configurar os serviços da aplicação.</param>
    /// <exception cref="ArgumentNullException">Lançada se o parâmetro <paramref name="app"/> for nulo.</exception>
    public static void UseLocalizationConfiguration(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Define as culturas suportadas pela aplicação.
        var culturasSuportadas = new[]
        {
            new CultureInfo("pt-BR")
        };

        // Configura as opções de localização.
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("pt-BR"),
            SupportedCultures = culturasSuportadas,
            SupportedUICultures = culturasSuportadas
        });
    }
}