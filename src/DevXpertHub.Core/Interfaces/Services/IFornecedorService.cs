using DevXpertHub.Core.Dtos.Fornecedores;

namespace DevXpertHub.Core.Interfaces.Services;

/// <summary>
/// Interface para o serviço de gerenciamento de fornecedores.
/// Define os métodos necessários para operações CRUD relacionadas a fornecedores.
/// </summary>
public interface IFornecedorService
{
    /// <summary>
    /// Obtém um fornecedor pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único do fornecedor.</param>
    /// <returns>Um objeto <see cref="FornecedorDto"/> representando o fornecedor, ou null se não encontrado.</returns>
    Task<FornecedorDto?> ObterPorIdAsync(string id);

    /// <summary>
    /// Cria um novo fornecedor.
    /// </summary>
    /// <param name="fornecedorDto">Os dados do fornecedor a serem criados.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO do fornecedor recém-adicionado.</returns>
    /// <exception cref="ArgumentException">Lançada se houver algum problema com os dados do fornecedor.</exception>
    Task<FornecedorDto> AdicionarAsync(FornecedorDto fornecedorDto);

    /// <summary>
    /// Atualiza os dados de um fornecedor existente.
    /// </summary>
    /// <param name="fornecedorDto">Os dados atualizados do fornecedor.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO do fornecedor atualizado.</returns>
    /// <exception cref="ArgumentException">Lançada se houver algum problema com os dados do fornecedor.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se o fornecedor com o ID especificado não for encontrado.</exception>
    /// <exception cref="UnauthorizedAccessException">Lançada se o fornecedor logado não tiver permissão para atualizar este fornecedor.</exception>
    Task<FornecedorDto> AtualizarAsync(FornecedorDto fornecedorDto);

    /// <summary>
    /// Remove um fornecedor pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único do fornecedor a ser removido.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se o fornecedor com o ID especificado não for encontrado.</exception>
    /// <exception cref="UnauthorizedAccessException">Lançada se o fornecedor não tiver permissão para excluir este fornecedor.</exception>
    Task ExcluirAsync(string id);
}