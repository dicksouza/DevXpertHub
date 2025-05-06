using DevXpertHub.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DevXpertHub.Core.Dtos.Produtos;
using DevXpertHub.Core.Interfaces.Services;

namespace DevXpertHub.Api.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações relacionadas a produtos.
/// Requer autenticação para a maioria das ações.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
[Authorize]
public class ProdutosController(IProdutoService produtoService) : ControllerBase
{
    private readonly IProdutoService _produtoService = produtoService;

    #region Create

    /// <summary>
    /// Adiciona um novo produto.
    /// </summary>
    /// <param name="produtoModel">O modelo de dados do produto a ser adicionado.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status201Created"/> com o produto criado em caso de sucesso,
    /// <see cref="StatusCodes.Status401Unauthorized"/> se o usuário não estiver autenticado.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProdutoCreateDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AdicionarProdutoAsync(ProdutoCreateDto produtoModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var fornecedorId = ObterIdDoUsuarioLogado();
            var produtoAdicionado = await _produtoService.AdicionarAsync(produtoModel, fornecedorId);
            return this.CreatedAtActionWithoutAsyncSuffix(nameof(ObterPorIdAsync), new { id = produtoAdicionado.Id }, produtoAdicionado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(Problem(title: "Erro na requisição", detail: ex.Message, statusCode: StatusCodes.Status400BadRequest));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(Problem(title: "Acesso não autorizado", detail: ex.Message, statusCode: StatusCodes.Status401Unauthorized));
        }
    }

    #endregion

    #region Read

    /// <summary>
    /// Obtém todos os produtos. Acesso anônimo permitido.
    /// </summary>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com a lista de produtos em caso de sucesso.
    /// </returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProdutoCreateDto>))]
    public async Task<IActionResult> ObterTodosProdutosAsync()
    {
        try
        {
            var produtos = await _produtoService.ObterTodosAsync();
            return Ok(produtos);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError, title: "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Obtém um produto pelo seu ID. Acesso anônimo permitido.
    /// </summary>
    /// <param name="id">O ID do produto a ser obtido.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com o produto encontrado em caso de sucesso,
    /// ou <see cref="StatusCodes.Status404NotFound"/> se o produto não for encontrado.
    /// </returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProdutoCreateDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ObterPorIdAsync(string id)
    {
        try
        {
            var produto = await _produtoService.ObterPorIdAsync(id);
            if (produto == null)
            {
                return NotFound(Problem(statusCode: StatusCodes.Status404NotFound, title: "Produto não encontrado."));
            }
            return Ok(produto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(Problem(title: "Produto não encontrado", detail: ex.Message, statusCode: StatusCodes.Status404NotFound));
        }
    }

    #endregion

    #region Update

    /// <summary>
    /// Atualiza um produto existente pelo seu ID.
    /// </summary>
    /// <param name="id">O ID do produto a ser atualizado.</param>
    /// <param name="produtoModel">O modelo de dados do produto com as informações atualizadas.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com o produto atualizado em caso de sucesso,
    /// <see cref="StatusCodes.Status404NotFound"/> se o produto não for encontrado,
    /// ou <see cref="StatusCodes.Status401Unauthorized"/> se o usuário não tiver permissão.
    /// </returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProdutoDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AtualizarProdutoAsync(string id, ProdutoDto produtoModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (id != produtoModel.Id)
        {
            return BadRequest(Problem(title: "Erro na requisição", detail: "IDs de produto incompatíveis.", statusCode: StatusCodes.Status400BadRequest));
        }

        if (produtoModel.Categoria?.Id != produtoModel.CategoriaId)
        {
            return BadRequest(Problem(title: "Erro na requisição", detail: "IDs de categoria incompatíveis.", statusCode: StatusCodes.Status400BadRequest));
        }

        try
        {
            var fornecedorIdLogado = ObterIdDoUsuarioLogado();
            var produtoAtualizado = await _produtoService.AtualizarAsync(produtoModel, fornecedorIdLogado);
            return Ok(produtoAtualizado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(Problem(title: "Produto não encontrado", detail: ex.Message, statusCode: StatusCodes.Status404NotFound));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(Problem(title: "Acesso não autorizado", detail: ex.Message, statusCode: StatusCodes.Status401Unauthorized));
        }
    }

    #endregion

    #region Delete

    /// <summary>
    /// Exclui um produto pelo seu ID.
    /// </summary>
    /// <param name="id">O ID do produto a ser excluído.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status204NoContent"/> em caso de sucesso,
    /// ou <see cref="StatusCodes.Status404NotFound"/> se o produto não for encontrado.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ExcluirProdutosAsync(string id)
    {
        try
        {
            var fornecedorId = ObterIdDoUsuarioLogado();
            await _produtoService.ExcluirAsync(id, fornecedorId);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(Problem(title: "Acesso não autorizado", detail: ex.Message, statusCode: StatusCodes.Status401Unauthorized));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(Problem(title: "Produto não encontrado", detail: ex.Message, statusCode: StatusCodes.Status404NotFound));
        }
    }

    #endregion

    private string ObterIdDoUsuarioLogado()
    {
        if (User == null)
        {
            throw new UnauthorizedAccessException("Usuário não autenticado.");
        }

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException("Reivindicação NameIdentifier não encontrada.");
        }

        if (Guid.TryParse(userIdClaim, out Guid userId))
        {
            return userId.ToString();
        }

        throw new FormatException($"Id de usuário inválido: {userIdClaim}");
    }
}