using DevXpertHub.Core.Entities;
using DevXpertHub.Core.Interfaces.Repositories;
using DevXpertHub.Core.Interfaces.Services;

namespace DevXpertHub.Core.Services;

/// <summary>
/// Implementação do serviço para a entidade <see cref="Post"/>.
/// Fornece a lógica de negócios para operações relacionadas a posts.
/// </summary>
public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<Post> AdicionarAsync(Post post)
    {
        return await _postRepository.AdicionarAsync(post);
    }

    public async Task<Post?> ObterPorIdAsync(string id)
    {
        return await _postRepository.ObterPorIdAsync(id);
    }

    public async Task<List<Post>> ObterTodosAsync()
    {
        return await _postRepository.ObterTodosAsync();
    }

    public async Task<Post> AtualizarAsync(Post post)
    {
        return await _postRepository.AtualizarAsync(post);
    }

    public async Task ExcluirAsync(string id)
    {
        await _postRepository.ExcluirAsync(id);
    }
}