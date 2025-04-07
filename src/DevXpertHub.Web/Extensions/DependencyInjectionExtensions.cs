using DevXpertHub.Core.Interfaces;
using DevXpertHub.Domain.Interfaces;
using DevXpertHub.Infrastructure.Repositories;
using DevXpertHub.Services;

namespace DevXpertHub.Web.Extensions;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adiciona a injeção de dependência para os repositórios e serviços da aplicação.
    /// </summary>
    /// <param name="services">A interface IServiceCollection para adicionar os serviços.</param>
    /// <returns>A interface IServiceCollection para encadeamento.</returns>
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IProdutoService, ProdutoService>();

        return services;
    }
}