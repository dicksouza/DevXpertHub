using DevXpertHub.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DevXpertHub.Web.Extensions;

/// <summary>
/// Classe de extensão para configurar serviços no contêiner de injeção de dependência.
/// Contém métodos para configurar o banco de dados, Identity, injeção de dependência de repositórios e serviços,
/// e suporte a MVC com Razor Pages.
/// </summary>
public static class DatabaseConfiguration
{
    /// <summary>
    /// Adiciona a configuração do contexto do banco de dados com base no ambiente.
    /// Utiliza Sqlite em desenvolvimento e SqlServer em produção.
    /// </summary>
    /// <param name="services">A interface IServiceCollection para adicionar os serviços.</param>
    /// <param name="configuration">A interface IConfiguration para acessar as configurações.</param>
    /// <param name="isDevelopment">Indica se o ambiente é de desenvolvimento.</param>
    /// <returns>A interface IServiceCollection para encadeamento.</returns>
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada.");

        if (isDevelopment)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString), ServiceLifetime.Scoped);
        }
        else
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
        }

        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }
}