using DevXpertHub.Core.Dtos.Categorias;
using DevXpertHub.Core.Dtos.Fornecedores;
using DevXpertHub.Core.Dtos.Produtos;
using DevXpertHub.Core.Entities;

namespace DevXpertHub.Core.Mappers;

/// <summary>
/// Classe estática responsável por realizar o mapeamento entre a entidade de domínio
/// <see cref="Produto"/> e o modelo de aplicação <see cref="ProdutoCreateDto"/>.
/// </summary>
public static class ProdutoMapper
{
    /// <summary>
    /// Converte um objeto ProdutoApplicationModel para um objeto Produto de domínio.
    /// Garante que uma Categoria válida esteja presente no modelo de aplicação.
    /// </summary>
    /// <param name="model">O modelo de aplicação do produto.</param>
    /// <param name="fornecedorId">O ID do fornecedor associado ao produto.</param>
    /// <returns>O objeto Produto de domínio.</returns>
    /// <exception cref="ArgumentNullException">Lançada se a propriedade Categoria do modelo for nula.</exception>
    public static Produto MapToDomain(ProdutoDto model, string fornecedorId)
    {
        if (model.Categoria == null)
        {
            throw new ArgumentNullException(nameof(model.Categoria), "A categoria do produto não pode ser nula.");
        }

        Categoria categoriaDominio = new Categoria(
            model.Categoria.Nome,
            model.Categoria.Descricao)
        {
            Id = model.Categoria.Id
        };

        return new Produto(model.Nome,
                           model.Descricao,
                           model.Preco,
                           model.Estoque,
                           model.CategoriaId,
                           model.FornecedorId,
                           model.ImagemPrincipal)
        {
            Id = model.Id,
        };
    }

    /// <summary>
    /// Mapeia uma entidade de domínio <see cref="Produto"/> para um modelo de aplicação <see cref="ProdutoCreateDto"/>.
    /// Lida com a possibilidade de a entidade de domínio não ter uma Categoria associada.
    /// Define o caminho da imagem para o modelo de aplicação, usando o valor da entidade ou um padrão.
    /// </summary>
    /// <param name="entidade">A entidade de domínio a ser mapeada.</param>
    /// <returns>Uma nova instância do modelo de aplicação <see cref="ProdutoCreateDto"/> com os dados mapeados.</returns>
    public static ProdutoDto MapToDto(Produto entidade)
    {
        CategoriaDto? categoriaModel = entidade.Categoria == null ? null : new CategoriaDto(
            entidade.Categoria.Id,
            entidade.Categoria.Nome,
            entidade.Categoria.Descricao
        );

        FornecedorDto? fornecedorModel = entidade.Fornecedor == null ? null : new FornecedorDto
        (
            entidade.Fornecedor.Id,
            entidade.Fornecedor.Nome,
            entidade.Fornecedor.Email
        );

        return new ProdutoDto
        (
            entidade.Id,
            entidade.Nome,
            entidade.Descricao,
            entidade.Preco,
            entidade.Estoque,
            entidade.CategoriaId,
            categoriaModel,
            entidade.FornecedorId,
            fornecedorModel,
            entidade.ImagemPrincipal
        );
    }

    /// <summary>
    /// Mapeia uma lista de entidades de domínio <see cref="Produto"/> para uma lista de modelos de aplicação <see cref="ProdutoCreateDto"/>.
    /// </summary>
    /// <param name="entidades">A lista de entidades de domínio a serem mapeadas.</param>
    /// <returns>Uma nova lista de modelos de aplicação <see cref="ProdutoCreateDto"/> com os dados mapeados.</returns>
    /// <exception cref="ArgumentNullException">Lançada se a lista de entidades de produto for nula.</exception>
    public static List<ProdutoDto> MapToDto(List<Produto> entidades)
    {
        if (entidades == null)
        {
            throw new ArgumentNullException(nameof(entidades), "A lista de entidades de produto não pode ser nula.");
        }

        return entidades.Select(MapToDto).ToList();
    }
}