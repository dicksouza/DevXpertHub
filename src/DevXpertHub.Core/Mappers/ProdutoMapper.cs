using DevXpertHub.Core.Dtos;
using DevXpertHub.Domain.Entities;

namespace DevXpertHub.Core.Mappers;

/// <summary>
/// Classe estática responsável por realizar o mapeamento entre a entidade de domínio
/// <see cref="Produto"/> e o modelo de aplicação <see cref="ProdutoApplicationModel"/>.
/// </summary>
public static class ProdutoMapper
{
    /// <summary>
    /// Converte um objeto ProdutoApplicationModel para um objeto Produto de domínio.
    /// Garante que uma Categoria válida esteja presente no modelo de aplicação.
    /// </summary>
    /// <param name="model">O modelo de aplicação do produto.</param>
    /// <param name="vendedorId">O ID do vendedor associado ao produto.</param>
    /// <returns>O objeto Produto de domínio.</returns>
    /// <exception cref="ArgumentNullException">Lançada se a propriedade Categoria do modelo for nula.</exception>
    public static Produto ParaDominio(ProdutoApplicationModel model, Guid vendedorId)
    {
        if (model.Categoria == null)
        {
            throw new ArgumentNullException(nameof(model.Categoria), "A categoria do produto não pode ser nula.");
        }

        Categoria categoriaDominio = new Categoria(
            model.Categoria.Nome,
            model.Categoria.Descricao,
                     model.Categoria.Id
        );

        return new Produto(
            model.Id,
            model.Nome,
            model.Descricao,
            model.Preco,
            model.Estoque,
            model.CategoriaId,
            categoriaDominio,
            vendedorId,
            model.Imagem
        );
    }

    /// <summary>
    /// Mapeia uma entidade de domínio <see cref="Produto"/> para um modelo de aplicação <see cref="ProdutoApplicationModel"/>.
    /// Lida com a possibilidade de a entidade de domínio não ter uma Categoria associada.
    /// Define o caminho da imagem para o modelo de aplicação, usando o valor da entidade ou um padrão.
    /// </summary>
    /// <param name="entidade">A entidade de domínio a ser mapeada.</param>
    /// <returns>Uma nova instância do modelo de aplicação <see cref="ProdutoApplicationModel"/> com os dados mapeados.</returns>
    public static ProdutoApplicationModel ParaAplicacao(Produto entidade)
    {
        CategoriaApplicationModel? categoriaModel = entidade.Categoria == null ? null : new CategoriaApplicationModel(
            entidade.Categoria.Id,
            entidade.Categoria.Nome,
            entidade.Categoria.Descricao
        );

        return new ProdutoApplicationModel
        {
            Id = entidade.Id,
            Nome = entidade.Nome,
            Descricao = entidade.Descricao,
            Preco = entidade.Preco,
            Estoque = entidade.Estoque,
            CategoriaId = entidade.CategoriaId,
            Categoria = categoriaModel,
            Imagem = string.IsNullOrEmpty(entidade.Imagem) ? "default_image_path" : entidade.Imagem
        };
    }

    /// <summary>
    /// Mapeia uma lista de entidades de domínio <see cref="Produto"/> para uma lista de modelos de aplicação <see cref="ProdutoApplicationModel"/>.
    /// </summary>
    /// <param name="entidades">A lista de entidades de domínio a serem mapeadas.</param>
    /// <returns>Uma nova lista de modelos de aplicação <see cref="ProdutoApplicationModel"/> com os dados mapeados.</returns>
    /// <exception cref="ArgumentNullException">Lançada se a lista de entidades de produto for nula.</exception>
    public static List<ProdutoApplicationModel> ParaAplicacao(List<Produto> entidades)
    {
        if (entidades == null)
        {
            throw new ArgumentNullException(nameof(entidades), "A lista de entidades de produto não pode ser nula.");
        }

        return entidades.Select(ParaAplicacao).ToList();
    }
}