namespace DevXpertHub.Api.Extensions;

public static class DeveloperEnvironmentExtensions
{
    /// <summary>
    /// Configura middlewares e endpoints específicos para o ambiente de desenvolvimento.
    /// </summary>
    /// <param name="app">A interface IApplicationBuilder para configurar o pipeline de requisição.</param>
    /// <returns>A interface IApplicationBuilder para encadeamento.</returns>
    public static IApplicationBuilder UseDeveloperEnvironment(this IApplicationBuilder app)
    {
        // Garante que o Swagger UI esteja disponível
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/openapi/v1.json", "DevXpertHub API v1");
        });
        //DatabaseInitializer.InitializeDatabase(app);
        return app;
    }
}