using DevXpertHub.Core.Entities;
using DevXpertHub.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DevXpertHub.Infrastructure.Repositories;

/// <summary>
/// Repositório para a entidade <see cref="Produto"/>.
/// Realiza operações de acesso a dados relacionadas a produtos.
/// </summary>
public class ProdutoRepository : IProdutoRepository, ITransactionSupport
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém um produto pelo ID, incluindo sua categoria.
    /// </summary>
    /// <param name="id">ID do produto.</param>
    /// <returns>O produto encontrado ou null se não existir.</returns>
    public async Task<Produto?> ObterPorIdAsync(string id)
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Include(p => p.Fornecedor)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Obtém todos os produtos de um fornecedor, sem rastreamento.
    /// </summary>
    /// <param name="fornecedorId">ID do fornecedor.</param>
    /// <returns>Lista de produtos do fornecedor.</returns>
    public async Task<List<Produto>> ObterTodosPorFornecedorAsync(string fornecedorId)
    {
        return await _context.Produtos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.Fornecedor)
            .Where(p => p.FornecedorId == fornecedorId)
            .ToListAsync();
    }

    /// <summary>
    /// Adiciona um novo produto ao banco de dados.
    /// </summary>
    /// <param name="produto">Produto a ser adicionado.</param>
    /// <returns>O produto adicionado.</returns>
    public async Task<Produto> AdicionarAsync(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    /// <summary>
    /// Atualiza um produto existente.
    /// </summary>
    /// <param name="produto">Produto com dados atualizados.</param>
    /// <returns>O produto atualizado.</returns>
    /// <exception cref="KeyNotFoundException">Se o produto não for encontrado.</exception>
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
    /// Exclui um produto pelo ID.
    /// </summary>
    /// <param name="id">ID do produto.</param>
    /// <exception cref="KeyNotFoundException">Se o produto não for encontrado.</exception>
    public async Task ExcluirAsync(string id)
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
    /// Obtém produtos de uma categoria específica.
    /// </summary>
    /// <param name="categoriaId">ID da categoria.</param>
    /// <returns>Lista de produtos da categoria.</returns>
    public async Task<List<Produto>> ObterProdutosPorCategoriaAsync(string categoriaId)
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Include(p => p.Fornecedor)
            .Where(p => p.CategoriaId == categoriaId)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém produtos com estoque abaixo de um limite.
    /// </summary>
    /// <param name="estoqueMinimo">Quantidade mínima de estoque.</param>
    /// <returns>Lista de produtos com estoque baixo.</returns>
    public async Task<List<Produto>> ObterProdutosComEstoqueBaixoAsync(int estoqueMinimo)
    {
        return await _context.Produtos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.Fornecedor)
            .Where(p => p.Estoque < estoqueMinimo)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém todos os produtos, incluindo suas categorias.
    /// </summary>
    /// <returns>Lista de todos os produtos.</returns>
    public async Task<List<Produto>> ObterTodosAsync()
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .Include(p => p.Fornecedor)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Obtém um produto pelo nome e fornecedor.
    /// </summary>
    /// <param name="nome">Nome do produto.</param>
    /// <param name="fornecedorIdLogado">ID do fornecedor.</param>
    /// <returns>O produto encontrado ou null.</returns>
    public async Task<Produto?> ObterPorNomeEFornecedorAsync(string nome, string fornecedorIdLogado)
    {
        return await _context.Produtos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.Fornecedor)
            .FirstOrDefaultAsync(p => p.Nome == nome && p.FornecedorId == fornecedorIdLogado);
    }

    ///// <summary>
    ///// Adiciona uma imagem a um produto.
    ///// </summary>
    ///// <param name="imagem">Imagem a ser adicionada.</param>
    //public async Task AdicionarImagemAsync(ProdutoImagem imagem)
    //{
    //    if (imagem == null)
    //    {
    //        throw new ArgumentNullException(nameof(imagem), "A imagem não pode ser nula.");
    //    }

    //    _context.Set<ProdutoImagem>().Add(imagem);
    //    await _context.SaveChangesAsync();
    //}

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}