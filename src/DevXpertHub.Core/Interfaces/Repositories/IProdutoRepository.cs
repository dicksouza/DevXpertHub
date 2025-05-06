using DevXpertHub.Core.Entities;

namespace DevXpertHub.Core.Interfaces.Repositories;

/// <summary>
/// Define a interface para um repositório de produtos.
/// Esta interface declara os métodos para realizar operações de acesso a dados
/// relacionadas à entidade <see cref="Produto"/>.
/// </summary>
public interface IProdutoRepository
{
    /// <summary>
    /// Obtém um produto pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único do produto a ser obtido.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Produto"/> encontrada ou null se nenhum produto com o ID especificado existir.</returns>
    Task<Produto?> ObterPorIdAsync(string id);

    /// <summary>
    /// Obtém todos os produtos do repositório de forma assíncrona.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de todas as entidades <see cref="Produto"/> no repositório.</returns>
    Task<List<Produto>> ObterTodosAsync();

    /// <summary>
    /// Obtém todos os produtos associados a um determinado fornecedor de forma assíncrona.
    /// </summary>
    /// <param name="fornecedorId">O identificador único do fornecedor.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de todas as entidades <see cref="Produto"/> cadastradas pelo fornecedor especificado.</returns>
    Task<List<Produto>> ObterTodosPorFornecedorAsync(string fornecedorId);

    /// <summary>
    /// Adiciona um novo produto ao repositório de forma assíncrona.
    /// </summary>
    /// <param name="produto">A entidade <see cref="Produto"/> a ser adicionada.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Produto"/> recém-adicionada, incluindo seu ID gerado (se aplicável).</returns>
    Task<Produto> AdicionarAsync(Produto produto);

    /// <summary>
    /// Atualiza um produto existente no repositório de forma assíncrona.
    /// </summary>
    /// <param name="produto">A entidade <see cref="Produto"/> com os dados atualizados.
    /// O ID do produto a ser atualizado deve estar presente.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Produto"/> após a atualização.</returns>
    Task<Produto> AtualizarAsync(Produto produto);

    /// <summary>
    /// Exclui um produto do repositório pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único do produto a ser excluído.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task ExcluirAsync(string id);

    /// <summary>
    /// Obtém todos os produtos pertencentes a uma determinada categoria de forma assíncrona.
    /// </summary>
    /// <param name="categoriaId">O identificador único da categoria.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de todas as entidades <see cref="Produto"/> que possuem o <see cref="Produto.CategoriaId"/> especificado.</returns>
    Task<List<Produto>> ObterProdutosPorCategoriaAsync(string categoriaId);

    /// <summary>
    /// Obtém um produto pelo seu nome e pelo ID do fornecedor de forma assíncrona.
    /// </summary>
    /// <param name="nome">O nome do produto a ser obtido.</param>
    /// <param name="fornecedorIdLogado">O identificador único do fornecedor que cadastrou o produto.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Produto"/> encontrada ou null se nenhum produto com o nome e fornecedor especificados existir.</returns>
    Task<Produto?> ObterPorNomeEFornecedorAsync(string nome, string fornecedorIdLogado);
}