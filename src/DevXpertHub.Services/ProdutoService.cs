using DevXpertHub.Core.Dtos;
using DevXpertHub.Core.Interfaces;
using DevXpertHub.Domain.Entities;
using DevXpertHub.Domain.Interfaces;

namespace DevXpertHub.Services;

/// <summary>
/// Implementação do serviço para a entidade <see cref="Produto"/>.
/// Fornece a lógica de negócios para operações relacionadas a produtos,
/// utilizando os repositórios <see cref="IProdutoRepository"/> e <see cref="ICategoriaRepository"/>.
/// </summary>
public class ProdutoService(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository) : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;

    #region Create

    /// <summary>
    /// Adiciona um novo produto de forma assíncrona.
    /// Valida a existência da categoria associada e associa o produto ao vendedor logado.
    /// </summary>
    /// <param name="produtoDto">O DTO <see cref="ProdutoApplicationModel"/> contendo os dados do novo produto.</param>
    /// <param name="vendedorIdLogado">O ID do vendedor logado que está criando o produto.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO <see cref="ProdutoApplicationModel"/> do produto adicionado, incluindo os dados da categoria.</returns>
    /// <exception cref="ArgumentException">Ocorre se a categoria especificada não for encontrada.</exception>
    public async Task<ProdutoApplicationModel> AdicionarAsync(ProdutoApplicationModel produtoDto, Guid vendedorIdLogado)
    {
        // Verificar se a categoria existe
        var categoria = await _categoriaRepository.ObterPorIdAsync(produtoDto.CategoriaId)
                            ?? throw new ArgumentException($"Categoria com Id {produtoDto.CategoriaId} não encontrada.");

        // Verificar se o produto a ser adicionado já existe para o vendedor logado
        var produtoExistente = await _produtoRepository.ObterPorNomeEVendedorAsync(produtoDto.Nome, vendedorIdLogado);
        if (produtoExistente != null)
        {
            throw new ArgumentException($"O produto com o nome '{produtoDto.Nome}' já existe para o vendedor logado.");
        }

        // Criar a entidade de domínio Produto
        var novoProduto = new Produto(
            produtoDto.Id,
            produtoDto.Nome,
            produtoDto.Descricao,
            produtoDto.Preco,
            produtoDto.Estoque,
            produtoDto.CategoriaId,
            categoria,
            vendedorIdLogado,
            produtoDto.Imagem
        );

        // Adicionar o produto ao repositório
        var resultado = await _produtoRepository.AdicionarAsync(novoProduto);

        // Mapear a entidade de domínio de volta para um DTO e retornar
        return new ProdutoApplicationModel
        {
            Id = resultado.Id,
            Nome = resultado.Nome,
            Descricao = resultado.Descricao,
            Preco = resultado.Preco,
            Estoque = resultado.Estoque,
            CategoriaId = resultado.CategoriaId,
            Categoria = new CategoriaApplicationModel(
                resultado.Categoria.Id,
                resultado.Categoria.Nome,
                resultado.Categoria.Descricao
            ),
            Imagem = resultado.Imagem
        };
    }

    #endregion

    #region Read

    /// <summary>
    /// Obtém um produto pelo seu ID de forma assíncrona, incluindo os dados da categoria associada.
    /// </summary>
    /// <param name="id">O ID do produto a ser obtido.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO <see cref="ProdutoApplicationModel"/> do produto encontrado, incluindo os dados da categoria.</returns>
    /// <exception cref="KeyNotFoundException">Ocorre se nenhum produto for encontrado com o ID especificado.</exception>
    public async Task<ProdutoApplicationModel?> ObterPorIdAsync(int id)
    {
        // Obter o produto do repositório pelo ID, incluindo a categoria
        var produto = await _produtoRepository.ObterPorIdAsync(id)
                          ?? throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");

        // Mapear a entidade de domínio para um DTO e retornar
        return new ProdutoApplicationModel
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            CategoriaId = produto.CategoriaId,
            Categoria = new CategoriaApplicationModel(
                produto.Categoria.Id,
                produto.Categoria.Nome,
                produto.Categoria.Descricao
            ),
            Imagem = produto.Imagem
        };
    }

    /// <summary>
    /// Obtém todos os produtos de forma assíncrona, incluindo os dados da categoria associada.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de DTOs <see cref="ProdutoApplicationModel"/> representando todos os produtos,
    /// incluindo os dados de suas respectivas categorias.</returns>
    public async Task<List<ProdutoApplicationModel>> ObterTodosAsync()
    {
        // Obter todos os produtos do repositório, incluindo a categoria
        var produtos = await _produtoRepository.ObterTodosAsync();
        // Mapear a lista de entidades de domínio para uma lista de DTOs e retornar
        return produtos.Select(p => new ProdutoApplicationModel
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            Preco = p.Preco,
            Estoque = p.Estoque,
            CategoriaId = p.CategoriaId,
            Categoria = new CategoriaApplicationModel(
                p.Categoria.Id,
                p.Categoria.Nome,
                p.Categoria.Descricao
            ),
            Imagem = p.Imagem
        }).ToList();
    }

    /// <summary>
    /// Obtém todos os produtos de um determinado vendedor de forma assíncrona.
    /// </summary>
    /// <param name="vendedorId">O ID do vendedor.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de DTOs <see cref="ProdutoApplicationModel"/> representando os produtos do vendedor,
    /// incluindo os dados de suas respectivas categorias.</returns>
    public async Task<List<ProdutoApplicationModel>> ObterTodosPorVendedorAsync(Guid vendedorId)
    {
        // Obter os produtos do vendedor do repositório
        var produtos = await _produtoRepository.ObterTodosPorVendedorAsync(vendedorId);
        // Mapear a lista de entidades de domínio para uma lista de DTOs e retornar
        return produtos.Select(p => new ProdutoApplicationModel
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            Preco = p.Preco,
            Estoque = p.Estoque,
            CategoriaId = p.CategoriaId,
            Categoria = new CategoriaApplicationModel(
                p.Categoria.Id,
                p.Categoria.Nome,
                p.Categoria.Descricao
            ),
            Imagem = p.Imagem
        }).ToList();
    }

    /// <summary>
    /// Obtém todos os produtos de uma determinada categoria de forma assíncrona.
    /// </summary>
    /// <param name="categoriaId">O ID da categoria.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de DTOs <see cref="ProdutoApplicationModel"/> representando os produtos da categoria,
    /// incluindo os dados da categoria.</returns>
    public async Task<List<ProdutoApplicationModel>> ObterProdutosPorCategoriaAsync(int categoriaId)
    {
        // Obter os produtos da categoria do repositório
        var produtos = await _produtoRepository.ObterProdutosPorCategoriaAsync(categoriaId);
        // Mapear a lista de entidades de domínio para uma lista de DTOs e retornar
        return produtos.Select(p => new ProdutoApplicationModel
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            Preco = p.Preco,
            Estoque = p.Estoque,
            CategoriaId = p.CategoriaId,
            Categoria = new CategoriaApplicationModel(
                p.Categoria.Id,
                p.Categoria.Nome,
                p.Categoria.Descricao
            ),
            Imagem = p.Imagem
        }).ToList();
    }

    #endregion

    #region Update

    /// <summary>
    /// Atualiza um produto existente de forma assíncrona.
    /// Valida a existência do produto e da categoria associada, e verifica se o vendedor logado tem permissão para editar o produto.
    /// </summary>
    /// <param name="produtoDto">O DTO <see cref="ProdutoApplicationModel"/> contendo os dados atualizados do produto.</param>
    /// <param name="vendedorIdLogado">O ID do vendedor logado que está tentando atualizar o produto.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO <see cref="ProdutoApplicationModel"/> do produto atualizado, incluindo os dados da categoria.</returns>
    /// <exception cref="KeyNotFoundException">Ocorre se o produto ou a categoria especificada não forem encontrados.</exception>
    /// <exception cref="UnauthorizedAccessException">Ocorre se o vendedor logado não for o proprietário do produto.</exception>
    public async Task<ProdutoApplicationModel> AtualizarAsync(ProdutoApplicationModel produtoDto, Guid vendedorIdLogado)
    {
        // Verificar se o produto existe
        var produtoExistente = await _produtoRepository.ObterPorIdAsync(produtoDto.Id)
                                       ?? throw new KeyNotFoundException($"Produto com Id {produtoDto.Id} não encontrado.");

        // Verificar se o vendedor logado tem permissão para editar este produto
        if (produtoExistente.VendedorId != vendedorIdLogado)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para editar este produto.");
        }

        // Verificar se a categoria existe
        var categoriaExistente = await _categoriaRepository.ObterPorIdAsync(produtoDto.CategoriaId)
                                       ?? throw new KeyNotFoundException($"Categoria com Id {produtoDto.CategoriaId} não encontrada.");

        // Verificar se o produto a ser atualizado já existe para o vendedor logado
        var produtoExistenteComMesmoNome = await _produtoRepository.ObterPorNomeEVendedorAsync(produtoDto.Nome, vendedorIdLogado);
        if (produtoExistenteComMesmoNome != null && produtoExistenteComMesmoNome.Id != produtoDto.Id)
        {
            throw new ArgumentException($"O produto com o nome '{produtoDto.Nome}' já existe para o vendedor logado.");
        }

        // Criar a entidade de domínio Produto para atualização
        var produtoAtualizado = new Produto(
            produtoDto.Id,
            produtoDto.Nome,
            produtoDto.Descricao,
            produtoDto.Preco,
            produtoDto.Estoque,
            produtoDto.CategoriaId,
            categoriaExistente,
            vendedorIdLogado,
            produtoDto.Imagem
        );

        // Atualizar o produto no repositório
        var resultado = await _produtoRepository.AtualizarAsync(produtoAtualizado);

        // Mapear a entidade de domínio atualizada de volta para um DTO e retornar
        return new ProdutoApplicationModel
        {
            Id = resultado.Id,
            Nome = resultado.Nome,
            Descricao = resultado.Descricao,
            Preco = resultado.Preco,
            Estoque = resultado.Estoque,
            CategoriaId = resultado.CategoriaId,
            Categoria = new CategoriaApplicationModel(
                resultado.Categoria.Id,
                resultado.Categoria.Nome,
                resultado.Categoria.Descricao
            ),
            Imagem = resultado.Imagem
        };
    }

    #endregion

    #region Delete

    /// <summary>
    /// Exclui um produto pelo seu ID de forma assíncrona.
    /// Verifica se o produto existe e se o vendedor logado tem permissão para excluir o produto.
    /// </summary>
    /// <param name="id">O ID do produto a ser excluído.</param>
    /// <param name="vendedorId">O ID do vendedor logado que está tentando excluir o produto.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    /// <exception cref="KeyNotFoundException">Ocorre se nenhum produto for encontrado com o ID especificado.</exception>
    /// <exception cref="UnauthorizedAccessException">Ocorre se o vendedor logado não for o proprietário do produto.</exception>
    public async Task ExcluirAsync(int id, Guid vendedorId)
    {
        // Verificar se o produto existe
        var produto = await _produtoRepository.ObterPorIdAsync(id)
                          ?? throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");

        // Verificar se o vendedor logado tem permissão para excluir este produto
        if (produto.VendedorId != vendedorId)
        {
            throw new UnauthorizedAccessException("Você não tem permissão para excluir este produto.");
        }

        // Excluir o produto do repositório
        await _produtoRepository.ExcluirAsync(id);
    }

    #endregion
}