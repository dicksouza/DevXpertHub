using DevXpertHub.Core.Entities;

namespace DevXpertHub.Core.Interfaces.Repositories;

/// <summary>
/// Interface para o repositório de comentários.
/// </summary>
public interface IComentarioRepository
{
    Task<Comentario> AdicionarAsync(Comentario comentario);
    Task<Comentario?> ObterPorIdAsync(string id);
    Task<List<Comentario>> ObterPorPostIdAsync(string postId);
    Task<Comentario> AtualizarAsync(Comentario comentario);
    Task ExcluirAsync(string id);
}