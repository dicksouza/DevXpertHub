using DevXpertHub.Core.Interfaces;
using DevXpertHub.Services;

namespace DevXpertHub.Api.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Adiciona a injeção de dependência para os serviços de aplicação.
    /// </summary>
    /// <param name="services">A interface IServiceCollection para adicionar os serviços.</param>
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IProdutoService, ProdutoService>();
    }
}