using DevXpertHub.Domain.Entities;
using DevXpertHub.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevXpertHub.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório para a entidade <see cref="Produto"/>.
/// Fornece métodos para realizar operações de acesso a dados relacionadas a produtos no banco de dados.
/// </summary>
public class ProdutoRepository(AppDbContext context) : IProdutoRepository
{
    private readonly AppDbContext _context = context;

    /// <summary>
    /// Obtém um produto pelo seu identificador único de forma assíncrona, incluindo a sua categoria.
    /// </summary>
    /// <param name="id">O identificador único do produto a ser obtido.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Produto"/> encontrada com a propriedade de navegação <see cref="Produto.Categoria"/> carregada,
    /// ou null se nenhum produto com o ID especificado existir.</returns>
    /// <exception cref="DbException">Ocorre se houver um erro ao acessar o banco de dados.</exception>
    public async Task<Produto?> ObterPorIdAsync(int id)
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Obtém todos os produtos associados a um determinado vendedor de forma assíncrona, sem rastreamento.
    /// </summary>
    /// <param name="vendedorId">O identificador único do vendedor.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de todas as entidades <see cref="Produto"/> cadastradas pelo vendedor especificado,
    /// sem serem rastreadas pelo Entity Framework.</returns>
    /// <exception cref="DbException">Ocorre se houver um erro ao acessar o banco de dados.</exception>
    public async Task<List<Produto>> ObterTodosPorVendedorAsync(Guid vendedorId)
    {
        return await _context.Produtos
            .AsNoTracking()
            .Where(p => p.VendedorId == vendedorId)
            .ToListAsync();
    }

    /// <summary>
    /// Adiciona um novo produto ao banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="produto">A entidade <see cref="Produto"/> a ser adicionada.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Produto"/> recém-adicionada, com seu ID gerado pelo banco de dados.</returns>
    /// <exception cref="DbUpdateException">Ocorre se houver um erro ao salvar as alterações no banco de dados.</exception>
    /// <exception cref="DbUpdateConcurrencyException">Ocorre se ocorrer um erro de concorrência ao salvar as alterações.</exception>
    public async Task<Produto> AdicionarAsync(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    /// <summary>
    /// Atualiza um produto existente no banco de dados de forma assíncrona.
    /// </summary>
    /// <param name="produto">A entidade <see cref="Produto"/> com os dados atualizados.
    /// O ID do produto a ser atualizado deve corresponder ao ID da entidade fornecida.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Produto"/> atualizada.</returns>
    /// <exception cref="KeyNotFoundException">Ocorre se não for encontrado nenhum produto com o ID especificado.</exception>
    /// <exception cref="DbUpdateException">Ocorre se houver um erro ao salvar as alterações no banco de dados.</exception>
    /// <exception cref="DbUpdateConcurrencyException">Ocorre se ocorrer um erro de concorrência ao salvar as alterações.</exception>
    public async Task<Produto> AtualizarAsync(Produto produto)
    {
        var produtoExistente = await _context.Produtos.FindAsync(produto.Id);
        if (produtoExistente == null)
        {
            throw new KeyNotFoundException($"Produto com Id {produto.Id} não encontrado.");
        }
        _context.Entry(produtoExistente).CurrentValues.SetValues(produto);
        await _context.SaveChangesAsync();
        return produtoExistente;
    }

    /// <summary>
    /// Exclui um produto do banco de dados pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único do produto a ser excluído.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    /// <exception cref="KeyNotFoundException">Ocorre se não for encontrado nenhum produto com o ID especificado.</exception>
    /// <exception cref="DbUpdateException">Ocorre se houver um erro ao salvar as alterações no banco de dados.</exception>
    /// <exception cref="DbUpdateConcurrencyException">Ocorre se ocorrer um erro de concorrência ao salvar as alterações.</exception>
    public async Task ExcluirAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto != null)
        {
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");
        }
    }

    /// <summary>
    /// Obtém todos os produtos pertencentes a uma determinada categoria de forma assíncrona, incluindo a categoria.
    /// </summary>
    /// <param name="categoriaId">O identificador único da categoria.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de todas as entidades <see cref="Produto"/> que possuem o <see cref="Produto.CategoriaId"/> especificado,
    /// com a propriedade de navegação <see cref="Produto.Categoria"/> carregada.</returns>
    /// <exception cref="DbException">Ocorre se houver um erro ao acessar o banco de dados.</exception>
    public async Task<List<Produto>> ObterProdutosPorCategoriaAsync(int categoriaId)
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.CategoriaId == categoriaId)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém todos os produtos com estoque abaixo de um determinado limite de forma assíncrona, sem rastreamento.
    /// </summary>
    /// <param name="estoqueMinimo">O nível mínimo de estoque para filtrar os produtos.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de todas as entidades <see cref="Produto"/> com estoque inferior ao valor especificado,
    /// sem serem rastreadas pelo Entity Framework.</returns>
    /// <exception cref="DbException">Ocorre se houver um erro ao acessar o banco de dados.</exception>
    public async Task<List<Produto>> ObterProdutosComEstoqueBaixoAsync(int estoqueMinimo)
    {
        return await _context.Produtos
            .AsNoTracking()
            .Where(p => p.Estoque < estoqueMinimo)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém todos os produtos do banco de dados de forma assíncrona, incluindo suas categorias e sem rastreamento.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de todas as entidades <see cref="Produto"/> no banco de dados,
    /// com a propriedade de navegação <see cref="Produto.Categoria"/> carregada e sem rastreamento.</returns>
    /// <exception cref="DbException">Ocorre se houver um erro ao acessar o banco de dados.</exception>
    public async Task<List<Produto>> ObterTodosAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Obtém um produto pelo seu nome e identificador do vendedor de forma assíncrona, sem rastreamento.
    /// </summary>
    /// <param name="nome">O nome do produto a ser obtido.</param>
    /// <param name="vendedorIdLogado">O identificador único do vendedor logado.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Produto"/> encontrada ou null se nenhum produto com o nome e vendedor especificados existir.</returns>
    /// <exception cref="DbException">Ocorre se houver um erro ao acessar o banco de dados.</exception>
    public async Task<Produto?> ObterPorNomeEVendedorAsync(string nome, Guid vendedorIdLogado)
    {
        return await _context.Produtos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Nome == nome && p.VendedorId == vendedorIdLogado);
    }
}