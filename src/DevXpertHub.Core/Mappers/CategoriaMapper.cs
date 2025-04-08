using DevXpertHub.Core.Dtos;
using DevXpertHub.Domain.Entities;

namespace DevXpertHub.Core.Mappers;

/// <summary>
/// Classe estática responsável por realizar o mapeamento entre a entidade de domínio
/// <see cref="Categoria"/> e o modelo de aplicação <see cref="CategoriaApplicationModel"/>.
/// </summary>
public static class CategoriaMapper
{
    /// <summary>
    /// Mapeia um modelo de aplicação <see cref="CategoriaApplicationModel"/> para uma entidade de domínio <see cref="Categoria"/>.
    /// </summary>
    /// <param name="model">O modelo de aplicação a ser mapeado.</param>
    /// <returns>Uma nova instância da entidade de domínio <see cref="Categoria"/> com os dados mapeados.</returns>
    public static Categoria ParaDominio(CategoriaApplicationModel model)
    {
        return new Categoria
        (
            model.Nome,
            model.Descricao,
            model.Id
        );
    }

    /// <summary>
    /// Mapeia uma entidade de domínio <see cref="Categoria"/> para um modelo de aplicação <see cref="CategoriaApplicationModel"/>.
    /// </summary>
    /// <param name="entidade">A entidade de domínio a ser mapeada.</param>
    /// <returns>Uma nova instância do modelo de aplicação <see cref="CategoriaApplicationModel"/> com os dados mapeados.</returns>
    public static CategoriaApplicationModel ParaAplicacao(Categoria entidade)
    {
        return new CategoriaApplicationModel(entidade.Id, entidade.Nome, entidade.Descricao);
    }

    /// <summary>
    /// Mapeia uma lista de entidades de domínio <see cref="Categoria"/> para uma lista de modelos de aplicação <see cref="CategoriaApplicationModel"/>.
    /// </summary>
    /// <param name="entidades">A lista de entidades de domínio a serem mapeadas.</param>
    /// <returns>Uma nova lista de modelos de aplicação <see cref="CategoriaApplicationModel"/> com os dados mapeados.</returns>
    public static List<CategoriaApplicationModel> ParaAplicacao(List<Categoria> entidades)
    {
        var modelos = new List<CategoriaApplicationModel>();
        foreach (var entidade in entidades)
        {
            modelos.Add(ParaAplicacao(entidade));
        }
        return modelos;
    }

    /// <summary>
    /// Mapeia uma entidade de domínio <see cref="Categoria"/> para um modelo de aplicação <see cref="CategoriaApplicationModel"/>
    /// que pode ser utilizado como ViewModel na camada de apresentação.
    /// </summary>
    /// <param name="categoria">A entidade de domínio a ser mapeada.</param>
    /// <returns>Uma nova instância do modelo de aplicação <see cref="CategoriaApplicationModel"/> com os dados mapeados para a View.</returns>
    public static CategoriaApplicationModel ParaViewModel(Categoria categoria)
    {
        return new CategoriaApplicationModel
        (
            categoria.Id,
            categoria.Nome,
            categoria.Descricao
        );
    }

    /// <summary>
    /// Mapeia uma lista de entidades de domínio <see cref="Categoria"/> para uma lista de modelos de aplicação <see cref="CategoriaApplicationModel"/>
    /// que podem ser utilizados como ViewModels na camada de apresentação.
    /// </summary>
    /// <param name="categorias">A lista de entidades de domínio a serem mapeadas.</param>
    /// <returns>Uma nova lista de modelos de aplicação <see cref="CategoriaApplicationModel"/> com os dados mapeados para a View.</returns>
    public static List<CategoriaApplicationModel> ParaViewModel(List<Categoria> categorias)
    {
        return categorias.Select(categoria => new CategoriaApplicationModel
        (
            categoria.Id,
            categoria.Nome,
            categoria.Descricao
        )).ToList();
    }
}