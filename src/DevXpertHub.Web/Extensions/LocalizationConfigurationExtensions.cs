using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace DevXpertHub.Web.Extensions;

public static class LocalizationConfigurationExtensions
{
    /// <summary>
    /// Configura as opções de localização para a aplicação.
    /// </summary>
    /// <param name="app">A interface IApplicationBuilder para configurar o pipeline de requisição.</param>
    /// <returns>A interface IApplicationBuilder para encadeamento.</returns>
    public static IApplicationBuilder UseLocalizationConfiguration(this IApplicationBuilder app)
    {
        var culturasSuportadas = new[]
        {
            new CultureInfo("pt-BR"),
            new CultureInfo("en-US")
        };

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("pt-BR"),
            SupportedCultures = culturasSuportadas,
            SupportedUICultures = culturasSuportadas
        });

        return app;
    }
}