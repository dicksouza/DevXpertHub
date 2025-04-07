using DevXpertHub.Core.Dtos;

namespace DevXpertHub.Core.Interfaces;

/// <summary>
/// Define a interface para um serviço de gerenciamento de produtos.
/// Esta interface declara os métodos que a camada de serviço de produto deve implementar,
/// expondo funcionalidades para obter, adicionar, atualizar e excluir produtos,
/// além de listar produtos por vendedor e categoria.
/// </summary>
public interface IProdutoService
{
    /// <summary>
    /// Obtém um produto pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único do produto a ser obtido.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO do produto encontrado ou null se nenhum produto com o ID especificado for encontrado.</returns>
    Task<ProdutoApplicationModel?> ObterPorIdAsync(int id);

    /// <summary>
    /// Obtém todos os produtos do sistema de forma assíncrona.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de DTOs representando todos os produtos.</returns>
    Task<List<ProdutoApplicationModel>> ObterTodosAsync();

    /// <summary>
    /// Obtém todos os produtos associados a um determinado vendedor de forma assíncrona.
    /// </summary>
    /// <param name="vendedorId">O identificador único do vendedor.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de DTOs representando os produtos do vendedor especificado.</returns>
    Task<List<ProdutoApplicationModel>> ObterTodosPorVendedorAsync(Guid vendedorId);

    /// <summary>
    /// Adiciona um novo produto ao sistema de forma assíncrona.
    /// </summary>
    /// <param name="produto">O DTO contendo os dados do produto a ser adicionado.</param>
    /// <param name="vendedorIdLogado">O identificador único do vendedor que está adicionando o produto.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO do produto recém-adicionado.</returns>
    /// <exception cref="ArgumentException">Lançada se houver algum problema com os dados do produto.</exception>
    Task<ProdutoApplicationModel> AdicionarAsync(ProdutoApplicationModel produto, Guid vendedorIdLogado);

    /// <summary>
    /// Atualiza um produto existente no sistema de forma assíncrona.
    /// </summary>
    /// <param name="produto">O DTO contendo os dados atualizados do produto. O ID do produto deve estar presente.</param>
    /// <param name="vendedorIdLogado">O identificador único do vendedor que está atualizando o produto.
    /// Usado para verificar se o vendedor tem permissão para atualizar este produto.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO do produto atualizado.</returns>
    /// <exception cref="ArgumentException">Lançada se houver algum problema com os dados do produto.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se o produto com o ID especificado não for encontrado.</exception>
    /// <exception cref="UnauthorizedAccessException">Lançada se o vendedor logado não tiver permissão para atualizar este produto.</exception>
    Task<ProdutoApplicationModel> AtualizarAsync(ProdutoApplicationModel produto, Guid vendedorIdLogado);

    /// <summary>
    /// Exclui um produto do sistema pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único do produto a ser excluído.</param>
    /// <param name="vendedorId">O identificador único do vendedor que está tentando excluir o produto.
    /// Usado para verificar se o vendedor tem permissão para excluir este produto.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se o produto com o ID especificado não for encontrado.</exception>
    /// <exception cref="UnauthorizedAccessException">Lançada se o vendedor não tiver permissão para excluir este produto.</exception>
    Task ExcluirAsync(int id, Guid vendedorId);

    /// <summary>
    /// Obtém todos os produtos pertencentes a uma determinada categoria de forma assíncrona.
    /// </summary>
    /// <param name="categoriaId">O identificador único da categoria.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de DTOs representando os produtos da categoria especificada.</returns>
    Task<List<ProdutoApplicationModel>> ObterProdutosPorCategoriaAsync(int categoriaId);
}