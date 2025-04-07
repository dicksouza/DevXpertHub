using DevXpertHub.Domain.Interfaces;
using DevXpertHub.Infrastructure.Repositories;

namespace DevXpertHub.Api.Extensions;

public static class RepositoryExtensions
{
    /// <summary>
    /// Adiciona a injeção de dependência para os repositórios.
    /// </summary>
    /// <param name="services">A interface IServiceCollection para adicionar os serviços.</param>
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
    }
}