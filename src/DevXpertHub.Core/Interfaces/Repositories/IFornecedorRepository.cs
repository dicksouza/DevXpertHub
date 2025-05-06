using DevXpertHub.Core.Entities;

namespace DevXpertHub.Core.Interfaces.Repositories;

public interface IFornecedorRepository
{
    Task<Fornecedor?> ObterPorIdAsync(string id);
    Task<List<Fornecedor>> ObterTodosAsync();
    Task<Fornecedor> AdicionarAsync(Fornecedor fornecedor);
    Task<Fornecedor> AtualizarAsync(Fornecedor fornecedor);
    Task RemoverAsync(string id);
}