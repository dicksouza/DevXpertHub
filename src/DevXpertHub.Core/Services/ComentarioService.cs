using DevXpertHub.Core.Entities;
using DevXpertHub.Core.Interfaces.Repositories;
using DevXpertHub.Core.Interfaces.Services;

namespace DevXpertHub.Core.Services;

/// <summary>
/// Serviço para gerenciar os comentários.
/// </summary>
public class ComentarioService : IComentarioService
{
    private readonly IComentarioRepository _comentarioRepository;

    public ComentarioService(IComentarioRepository comentarioRepository)
    {
        _comentarioRepository = comentarioRepository;
    }

    public async Task<Comentario> AdicionarAsync(Comentario comentario)
    {
        return await _comentarioRepository.AdicionarAsync(comentario);
    }

    public async Task<Comentario?> ObterPorIdAsync(string id)
    {
        return await _comentarioRepository.ObterPorIdAsync(id);
    }

    public async Task<List<Comentario>> ObterPorPostIdAsync(string postId)
    {
        return await _comentarioRepository.ObterPorPostIdAsync(postId);
    }

    public async Task<Comentario> AtualizarAsync(Comentario comentario)
    {
        return await _comentarioRepository.AtualizarAsync(comentario);
    }

    public async Task ExcluirAsync(string id)
    {
        await _comentarioRepository.ExcluirAsync(id);
    }
}