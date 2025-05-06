using DevXpertHub.Core.Entities;

namespace DevXpertHub.Core.Interfaces.Repositories;

/// <summary>
/// Define a interface para um repositório de posts.
/// Esta interface declara os métodos para realizar operações de acesso a dados
/// relacionadas à entidade <see cref="Post"/>.
/// </summary>
public interface IPostRepository
{
    Task<Post?> ObterPorIdAsync(string id);
    Task<List<Post>> ObterTodosAsync();
    Task<Post> AdicionarAsync(Post post);
    Task<Post> AtualizarAsync(Post post);
    Task ExcluirAsync(string id);
}