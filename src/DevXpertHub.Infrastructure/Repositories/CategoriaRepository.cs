using DevXpertHub.Domain.Entities;
using DevXpertHub.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevXpertHub.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório para a entidade <see cref="Categoria"/>.
/// Fornece métodos para realizar operações de acesso a dados relacionadas a categorias no banco de dados.
/// </summary>
public class CategoriaRepository(AppDbContext context) : ICategoriaRepository
{
    private readonly AppDbContext _context = context;

    /// <summary>
    /// Adiciona uma nova categoria ao banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="categoria">A entidade <see cref="Categoria"/> a ser adicionada.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Categoria"/> recém-adicionada, com seu ID gerado pelo banco de dados.</returns>
    /// <exception cref="DbUpdateException">Ocorre se houver um erro ao salvar as alterações no banco de dados.</exception>
    /// <exception cref="DbUpdateConcurrencyException">Ocorre se ocorrer um erro de concorrência ao salvar as alterações.</exception>
    public async Task<Categoria> AdicionarAsync(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    /// <summary>
    /// Obtém uma categoria pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único da categoria a ser obtida.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Categoria"/> encontrada ou null se nenhuma categoria com o ID especificado existir.</returns>
    /// <exception cref="DbException">Ocorre se houver um erro ao acessar o banco de dados.</exception>
    public async Task<Categoria?> ObterPorIdAsync(int id)
    {
        return await _context.Categorias.FindAsync(id);
    }

    /// <summary>
    /// Atualiza uma categoria existente no banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="categoria">A entidade <see cref="Categoria"/> com os dados atualizados.
    /// O ID da categoria a ser atualizada deve corresponder ao ID da entidade fornecida.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Categoria"/> atualizada.</returns>
    /// <exception cref="KeyNotFoundException">Ocorre se não for encontrada nenhuma categoria com o ID especificado.</exception>
    /// <exception cref="DbUpdateException">Ocorre se houver um erro ao salvar as alterações no banco de dados.</exception>
    /// <exception cref="DbUpdateConcurrencyException">Ocorre se ocorrer um erro de concorrência ao salvar as alterações.</exception>
    public async Task<Categoria> AtualizarAsync(Categoria categoria)
    {
        var categoriaExistente = await _context.Categorias.FindAsync(categoria.Id);
        if (categoriaExistente == null)
        {
            throw new KeyNotFoundException($"Categoria com Id {categoria.Id} não encontrada.");
        }
        _context.Entry(categoriaExistente).CurrentValues.SetValues(categoria);
        await _context.SaveChangesAsync();
        return categoriaExistente;
    }

    /// <summary>
    /// Exclui uma categoria do banco de dados pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único da categoria a ser excluída.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    /// <exception cref="KeyNotFoundException">Ocorre se não for encontrada nenhuma categoria com o ID especificado.</exception>
    /// <exception cref="InvalidOperationException">Ocorre se a categoria possuir produtos associados.</exception>
    /// <exception cref="DbUpdateException">Ocorre se houver um erro ao salvar as alterações no banco de dados.</exception>
    /// <exception cref="DbUpdateConcurrencyException">Ocorre se ocorrer um erro de concorrência ao salvar as alterações.</exception>
    public async Task ExcluirAsync(int id)
    {
        var categoria = await _context.Categorias
            .Include(c => c.Produtos!)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null)
        {
            throw new KeyNotFoundException($"Categoria com Id {id} não encontrada.");
        }

        if (categoria.Produtos!.Any())
        {
            throw new InvalidOperationException("Não é possível excluir a categoria, pois ela possui produtos associados.");
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Obtém todas as categorias do banco de dados de forma assíncrona.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de todas as entidades <see cref="Categoria"/> no banco de dados, sem rastreamento.</returns>
    /// <exception cref="DbException">Ocorre se houver um erro ao acessar o banco de dados.</exception>
    public async Task<List<Categoria>> ObterTodasAsync()
    {
        return await _context.Categorias.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Verifica de forma assíncrona se uma categoria possui produtos associados no banco de dados.
    /// </summary>
    /// <param name="categoriaId">O identificador único da categoria a ser verificada.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// é true se a categoria possuir produtos associados, caso contrário, false.</returns>
    /// <exception cref="DbException">Ocorre se houver um erro ao acessar o banco de dados.</exception>
    public async Task<bool> CategoriaPossuiProdutosAssociadosAsync(int categoriaId)
    {
        return await _context.Produtos.AnyAsync(p => p.CategoriaId == categoriaId);
    }

    /// <summary>
    /// Verifica de forma assíncrona se já existe uma categoria com o nome especificado no banco de dados,
    /// opcionalmente ignorando uma categoria com um determinado ID.
    /// </summary>
    /// <param name="nome">O nome da categoria a ser verificado.</param>
    /// <param name="id">O identificador único da categoria a ser ignorada na verificação (útil durante a atualização).
    /// Se nulo, a verificação será feita em todas as categorias.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// é true se existir uma categoria com o nome especificado (e ID diferente do fornecido, se houver), caso contrário, false.</returns>
    /// <exception cref="DbException">Ocorre se houver um erro ao acessar o banco de dados.</exception>
    public async Task<bool> ExisteCategoriaComNomeAsync(string nome, int? id = null)
    {
        var query = _context.Categorias.Where(c => c.Nome == nome);

        if (id.HasValue && id.Value != 0)
        {
            query = query.Where(c => c.Id != id.Value);
        }

        return await query.AnyAsync();
    }
}