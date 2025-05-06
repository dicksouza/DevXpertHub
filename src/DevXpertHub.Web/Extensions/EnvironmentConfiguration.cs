namespace DevXpertHub.Web.Extensions;

/// <summary>
/// Fornece métodos de extensão para configurar o comportamento do application builder com base no ambiente de hospedagem.
/// </summary>
public static class EnvironmentConfiguration
{
    /// <summary>
    /// Configura definições específicas do ambiente para a aplicação, como helpers de migração de banco de dados 
    /// em desenvolvimento e tratamento de exceções em produção.
    /// </summary>
    /// <param name="app">A instância de <see cref="IApplicationBuilder"/> a ser configurada.</param>
    /// <param name="env">O <see cref="IWebHostEnvironment"/> que representa o ambiente de hospedagem atual.</param>
    public static void UseEnvironmentConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseDbMigrationHelper();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
    }
}