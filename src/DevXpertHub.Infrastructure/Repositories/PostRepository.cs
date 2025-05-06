using DevXpertHub.Core.Entities;
using DevXpertHub.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevXpertHub.Infrastructure.Repositories;

/// <summary>
/// Repositório para gerenciar os posts.
/// </summary>
public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Post?> ObterPorIdAsync(string id)
    {
        return await _context.Posts
            .Include(p => p.Comentarios)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Post>> ObterTodosAsync()
    {
        return await _context.Posts
            .Include(p => p.Comentarios)
            .ToListAsync();
    }

    public async Task<Post> AdicionarAsync(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<Post> AtualizarAsync(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task ExcluirAsync(string id)
    {
        var post = await ObterPorIdAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}