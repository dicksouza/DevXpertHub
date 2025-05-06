using DevXpertHub.Core.Interfaces.Repositories;
using DevXpertHub.Core.Interfaces.Services;
using DevXpertHub.Core.Services;
using DevXpertHub.Infrastructure.Repositories;

namespace DevXpertHub.Web.Extensions;

/// <summary>
/// Classe de extensão para configurar serviços no contêiner de injeção de dependência.
/// Contém métodos para configurar o banco de dados, Identity, injeção de dependência de repositórios e serviços,
/// e suporte a MVC com Razor Pages.
/// </summary>
public static class DependencyInjectionConfiguration
{
    /// <summary>
    /// Adiciona a injeção de dependência para os repositórios e serviços da aplicação.
    /// </summary>
    /// <param name="services">A interface IServiceCollection para adicionar os serviços.</param>
    /// <returns>A interface IServiceCollection para encadeamento.</returns>
    public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
    {
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IFornecedorRepository, FornecedorRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IFornecedorService, FornecedorService>();
        services.AddScoped<IProdutoService, ProdutoService>();

        return services;
    }
}