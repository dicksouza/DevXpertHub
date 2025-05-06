using DevXpertHub.Core.Entities;
using DevXpertHub.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevXpertHub.Infrastructure.Repositories;

/// <summary>
/// Repositório para gerenciar os comentários.
/// </summary>
public class ComentarioRepository : IComentarioRepository
{
    private readonly AppDbContext _context;

    public ComentarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Comentario> AdicionarAsync(Comentario comentario)
    {
        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();
        return comentario;
    }

    public async Task<Comentario?> ObterPorIdAsync(string id)
    {
        return await _context.Comentarios.FindAsync(id);
    }

    public async Task<List<Comentario>> ObterPorPostIdAsync(string postId)
    {
        return await _context.Comentarios
            .Where(c => c.PostId == postId)
            .ToListAsync();
    }

    public async Task<Comentario> AtualizarAsync(Comentario comentario)
    {
        _context.Comentarios.Update(comentario);
        await _context.SaveChangesAsync();
        return comentario;
    }

    public async Task ExcluirAsync(string id)
    {
        var comentario = await ObterPorIdAsync(id);
        if (comentario != null)
        {
            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();
        }
    }
}