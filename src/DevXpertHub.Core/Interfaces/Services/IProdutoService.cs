using DevXpertHub.Core.Dtos.Categorias;
using DevXpertHub.Core.Dtos.Produtos;
using Microsoft.EntityFrameworkCore.Storage;

namespace DevXpertHub.Core.Interfaces.Services;

/// <summary>
/// Define a interface para um serviço de gerenciamento de produtos.
/// Esta interface declara os métodos que a camada de serviço de produto deve implementar,
/// expondo funcionalidades para obter, adicionar, atualizar e excluir produtos,
/// além de listar produtos por fornecedor e categoria.
/// </summary>
public interface IProdutoService
{
    /// <summary>
    /// Inicia uma transação para operações atômicas.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task<IDbContextTransaction> BeginTransactionAsync();

    /// <summary>
    /// Adiciona informações de uma imagem associada a um produto.
    /// </summary>
    /// <param name="produtoImagemDto">O DTO contendo os dados da imagem.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    //Task AdicionarImagemAsync(ProdutoImagemDto produtoImagemDto);

    /// <summary>
    /// Obtém um produto pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único do produto a ser obtido.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO do produto encontrado ou null se nenhum produto com o ID especificado for encontrado.</returns>
    Task<ProdutoDto?> ObterPorIdAsync(string id);

    /// <summary>
    /// Obtém todos os produtos do sistema de forma assíncrona.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de DTOs representando todos os produtos.</returns>
    Task<List<ProdutoDto>> ObterTodosAsync();

    /// <summary>
    /// Obtém todos os produtos associados a um determinado fornecedor de forma assíncrona.
    /// </summary>
    /// <param name="fornecedorId">O identificador único do fornecedor.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de DTOs representando os produtos do fornecedor especificado.</returns>
    Task<List<ProdutoDto>> ObterTodosPorFornecedorAsync(string fornecedorId);

    /// <summary>
    /// Obtém todos os produtos associados a um determinada categoria de forma assíncrona.
    /// </summary>
    /// <param name="categoriaId">O identificador único de uma categoria. </param>
    /// <returns> Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de DTOs representando os produtos da categoria especificada.</returns>
    /// </returns>
    Task<List<ProdutoDto>> ObterProdutosPorCategoriaAsync(string categoriaId);

    /// <summary>
    /// Adiciona um novo produto ao sistema de forma assíncrona.
    /// </summary>
    /// <param name="produtoDto">O DTO contendo os dados do produto a ser adicionado.</param>
    /// <param name="fornecedorIdLogado">O identificador único do fornecedor que está adicionando o produto.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO do produto recém-adicionado.</returns>
    /// <exception cref="ArgumentException">Lançada se houver algum problema com os dados do produto.</exception>
    Task<ProdutoDto> AdicionarAsync(ProdutoCreateDto produtoDto, string fornecedorIdLogado);

    /// <summary>
    /// Atualiza um produto existente no sistema de forma assíncrona.
    /// </summary>
    /// <param name="produtoDto">O DTO contendo os dados atualizados do produto. O ID do produto deve estar presente.</param>
    /// <param name="fornecedorIdLogado">O identificador único do fornecedor que está atualizando o produto.
    /// Usado para verificar se o fornecedor tem permissão para atualizar este produto.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO do produto atualizado.</returns>
    /// <exception cref="ArgumentException">Lançada se houver algum problema com os dados do produto.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se o produto com o ID especificado não for encontrado.</exception>
    /// <exception cref="UnauthorizedAccessException">Lançada se o fornecedor logado não tiver permissão para atualizar este produto.</exception>
    Task<ProdutoDto> AtualizarAsync(ProdutoDto produtoDto, string fornecedorIdLogado);

    /// <summary>
    /// Exclui um produto do sistema pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único do produto a ser excluído.</param>
    /// <param name="fornecedorId">O identificador único do fornecedor que está tentando excluir o produto.
    /// Usado para verificar se o fornecedor tem permissão para excluir este produto.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se o produto com o ID especificado não for encontrado.</exception>
    /// <exception cref="UnauthorizedAccessException">Lançada se o fornecedor não tiver permissão para excluir este produto.</exception>
    Task ExcluirAsync(string id, string fornecedorId);

    /// <summary>
    /// Obtém todas as categorias disponíveis no sistema de forma assíncrona.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa 
    /// contém uma lista de DTOs representando categorias.</returns>
    Task<List<CategoriaDto>> ObterCategoriasAsync();
}