using DevXpertHub.Core.Dtos.Categorias;
using DevXpertHub.Core.Dtos.Produtos;
using DevXpertHub.Core.Entities;
using DevXpertHub.Core.Interfaces.Repositories;
using DevXpertHub.Core.Interfaces.Services;
using DevXpertHub.Core.Mappers;
using Microsoft.EntityFrameworkCore.Storage;

namespace DevXpertHub.Core.Services;

/// <summary>
/// Implementação do serviço para a entidade <see cref="Produto"/>.
/// Fornece a lógica de negócios para operações relacionadas a produtos.
/// </summary>
public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public ProdutoService(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
    }

    #region Create
    public async Task<ProdutoDto> AdicionarAsync(ProdutoCreateDto produtoDto, string fornecedorIdLogado)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(produtoDto.CategoriaId)
            ?? throw new ArgumentException($"Categoria com Id {produtoDto.CategoriaId} não encontrada.");

        var produtoExistente = await _produtoRepository.ObterPorNomeEFornecedorAsync(produtoDto.Nome, fornecedorIdLogado);
        if (produtoExistente != null)
        {
            throw new ArgumentException($"O produto com o nome '{produtoDto.Nome}' já existe para o fornecedor logado.");
        }

        // Criação do produto sem a imagem principal inicialmente
        var novoProduto = new Produto(
            produtoDto.Nome,
            produtoDto.Descricao,
            produtoDto.Preco,
            produtoDto.Estoque,
            produtoDto.CategoriaId,
            fornecedorIdLogado,
            produtoDto.Imagem
        );

        // Após o produto ser adicionado e o ID gerado, crie a imagem principal
        //var imagemPrincipal = new ProdutoImagem(novoProduto.Id, produtoDto.Imagem ?? string.Empty);
        //novoProduto.AdicionarImagem(imagemPrincipal); // Adiciona a imagem principal ao produto

        // Salve o produto com a imagem principal
        var resultado = await _produtoRepository.AdicionarAsync(novoProduto);
        return ProdutoMapper.MapToDto(resultado);
    }

    #endregion

    #region Read

    public async Task<ProdutoDto?> ObterPorIdAsync(string id)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");

        return ProdutoMapper.MapToDto(produto);
    }

    public async Task<List<ProdutoDto>> ObterTodosAsync()
    {
        var produtos = await _produtoRepository.ObterTodosAsync();
        return produtos.Select(ProdutoMapper.MapToDto).ToList();
    }

    public async Task<List<ProdutoDto>> ObterTodosPorFornecedorAsync(string fornecedorId)
    {
        var produtos = await _produtoRepository.ObterTodosPorFornecedorAsync(fornecedorId);
        return produtos.Select(ProdutoMapper.MapToDto).ToList();
    }

    public async Task<List<ProdutoDto>> ObterProdutosPorCategoriaAsync(string categoriaId)
    {
        var produtos = await _produtoRepository.ObterProdutosPorCategoriaAsync(categoriaId);
        return produtos.Select(ProdutoMapper.MapToDto).ToList();
    }

    #endregion

    #region Update

    public async Task<ProdutoDto> AtualizarAsync(ProdutoDto produtoDto, string fornecedorIdLogado)
    {
        var produtoExistente = await _produtoRepository.ObterPorIdAsync(produtoDto.Id)
            ?? throw new KeyNotFoundException($"Produto com Id {produtoDto.Id} não encontrado.");

        if (produtoExistente.FornecedorId != fornecedorIdLogado)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para editar este produto.");
        }

        var categoriaExistente = await _categoriaRepository.ObterPorIdAsync(produtoDto.CategoriaId)
            ?? throw new KeyNotFoundException($"Categoria com Id {produtoDto.CategoriaId} não encontrada.");

        produtoExistente.Atualizar(
            produtoDto.Nome,
            produtoDto.Descricao,
            produtoDto.Preco,
            produtoDto.Estoque,
            produtoDto.CategoriaId,
            fornecedorIdLogado,
            produtoDto.ImagemPrincipal
        );

        var resultado = await _produtoRepository.AtualizarAsync(produtoExistente);
        return ProdutoMapper.MapToDto(resultado);
    }

    #endregion

    #region Delete

    public async Task ExcluirAsync(string id, string fornecedorId)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");

        if (produto.FornecedorId != fornecedorId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para excluir este produto.");
        }

        await _produtoRepository.ExcluirAsync(id);
    }

    #endregion

    #region Transactions and Images

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_produtoRepository is ITransactionSupport transactionSupport)
        {
            return await transactionSupport.BeginTransactionAsync();
        }
        throw new NotSupportedException("O repositório não suporta transações.");
    }

    //public async Task AdicionarImagemAsync(ProdutoImagemDto produtoImagemDto)
    //{
    //    if (produtoImagemDto == null)
    //    {
    //        throw new ArgumentNullException(nameof(produtoImagemDto));
    //    }

    //    var imagem = new ProdutoImagem(produtoImagemDto.ProdutoId, produtoImagemDto.Caminho);
    //    await _produtoRepository.AdicionarImagemAsync(imagem);
    //}

    #endregion

    #region Categorias
    public async Task<List<CategoriaDto>> ObterCategoriasAsync()
    {
        // Obter todas as categorias do repositório
        var categorias = await _categoriaRepository.ObterTodasAsync();
        // Mapear a lista de entidades de domínio para uma lista de DTOs e retornar
        return categorias
            .Where(_ => true) // Filtrar possíveis valores nulos (embora improvável com EF)
            .Select(CategoriaMapper.MapToDto)
            .ToList();
    }
    #endregion
}