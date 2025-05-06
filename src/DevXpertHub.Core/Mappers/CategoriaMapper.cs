using DevXpertHub.Core.Dtos.Categorias;
using DevXpertHub.Core.Entities;

namespace DevXpertHub.Core.Mappers;

/// <summary>
/// Classe estática responsável por realizar o mapeamento entre a entidade de domínio
/// <see cref="Categoria"/> e o modelo de aplicação <see cref="CategoriaDto"/>.
/// </summary>
public static class CategoriaMapper
{
    /// <summary>
    /// Mapeia um modelo de aplicação <see cref="CategoriaCreateDto"/> para uma entidade de domínio <see cref="Categoria"/>.
    /// </summary>
    /// <param name="model">O modelo de aplicação a ser mapeado.</param>
    /// <returns>Uma nova instância da entidade de domínio <see cref="Categoria"/> com os dados mapeados.</returns>
    public static Categoria MapToDomain(CategoriaCreateDto model)
    {
        return new Categoria
        (
            model.Nome,
            model.Descricao
        );
    }

    /// <summary>
    /// Mapeia um modelo de aplicação <see cref="CategoriaDto"/> para uma entidade de domínio <see cref="Categoria"/>.
    /// </summary>
    /// <param name="model">O modelo de aplicação a ser mapeado.</param>
    /// <returns>Uma nova instância da entidade de domínio <see cref="Categoria"/> com os dados mapeados.</returns>
    public static Categoria MapToDomain(CategoriaUpdateDto model)
    {
        return new Categoria (model.Nome, model.Descricao)
        {
            Id = model.Id
        };
    }

    /// <summary>
    /// Mapeia uma entidade de domínio <see cref="Categoria"/> para um modelo de aplicação <see cref="CategoriaDto"/>.
    /// </summary>
    /// <param name="entidade">A entidade de domínio a ser mapeada.</param>
    /// <returns>Uma nova instância do modelo de aplicação <see cref="CategoriaDto"/> com os dados mapeados.</returns>
    public static CategoriaDto MapToDto(Categoria entidade)
    {
        return new CategoriaDto(entidade.Id, entidade.Nome, entidade.Descricao);
    }
}