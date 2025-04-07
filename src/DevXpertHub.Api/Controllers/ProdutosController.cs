using DevXpertHub.Api.Extensions;
using DevXpertHub.Core.Dtos;
using DevXpertHub.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevXpertHub.Api.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações relacionadas a produtos.
/// Requer autenticação para a maioria das ações.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class ProdutosController(IProdutoService produtoService) : ControllerBase
{
    private readonly IProdutoService _produtoService = produtoService;

    #region Create

    /// <summary>
    /// Adiciona um novo produto.
    /// </summary>
    /// <param name="produtoModel">O modelo de dados do produto a ser adicionado.</param>
    /// <returns>Um <see cref="IActionResult"/> que representa o resultado da operação.
    /// Retorna <see cref="StatusCodes.Status201Created"/> com o produto criado em caso de sucesso,
    /// <see cref="StatusCodes.Status400BadRequest"/> se o modelo for inválido,
    /// <see cref="StatusCodes.Status401Unauthorized"/> se o usuário não estiver autenticado,
    /// e <see cref="StatusCodes.Status500InternalServerError"/> em caso de erro interno do servidor.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProdutoApplicationModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AdicionarProdutoAsync(ProdutoApplicationModel produtoModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var vendedorId = ObterIdDoUsuarioLogado();
            var produtoAdicionado = await _produtoService.AdicionarAsync(produtoModel, vendedorId);
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
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError, title: "Erro interno do servidor");
        }
    }

    #endregion

    #region Read

    /// <summary>
    /// Obtém todos os produtos. Acesso anônimo permitido.
    /// </summary>
    /// <returns>Um <see cref="IActionResult"/> que representa o resultado da operação.
    /// Retorna <see cref="StatusCodes.Status200OK"/> com a lista de produtos em caso de sucesso,
    /// e <see cref="StatusCodes.Status500InternalServerError"/> em caso de erro interno do servidor.</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProdutoApplicationModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
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
    /// <returns>Um <see cref="IActionResult"/> que representa o resultado da operação.
    /// Retorna <see cref="StatusCodes.Status200OK"/> com o produto encontrado em caso de sucesso,
    /// <see cref="StatusCodes.Status404NotFound"/> se o produto não for encontrado,
    /// e <see cref="StatusCodes.Status500InternalServerError"/> em caso de erro interno do servidor.</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProdutoApplicationModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ObterPorIdAsync(int id)
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
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError, title: "Erro interno do servidor");
        }
    }

    #endregion

    #region Update

    /// <summary>
    /// Atualiza um produto existente pelo seu ID.
    /// </summary>
    /// <param name="id">O ID do produto a ser atualizado.</param>
    /// <param name="produtoModel">O modelo de dados do produto com as informações atualizadas.</param>
    /// <returns>Um <see cref="IActionResult"/> que representa o resultado da operação.
    /// Retorna <see cref="StatusCodes.Status200OK"/> com o produto atualizado em caso de sucesso,
    /// <see cref="StatusCodes.Status400BadRequest"/> se o modelo for inválido ou os IDs forem incompatíveis,
    /// <see cref="StatusCodes.Status404NotFound"/> se o produto não for encontrado,
    /// <see cref="StatusCodes.Status401Unauthorized"/> se o usuário não tiver permissão,
    /// e <see cref="StatusCodes.Status500InternalServerError"/> em caso de erro interno do servidor.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProdutoApplicationModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AtualizarProdutoAsync(int id, ProdutoApplicationModel produtoModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (id != produtoModel.Id)
        {
            return BadRequest(Problem(title: "Erro na requisição", detail: "IDs de produto incompatíveis.", statusCode: StatusCodes.Status400BadRequest));
        }

        try
        {
            var vendedorIdLogado = ObterIdDoUsuarioLogado();
            var produtoAtualizado = await _produtoService.AtualizarAsync(produtoModel, vendedorIdLogado);
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
        catch (ArgumentException ex)
        {
            return BadRequest(Problem(title: "Erro na requisição", detail: ex.Message, statusCode: StatusCodes.Status400BadRequest));
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError, title: "Erro interno do servidor");
        }
    }

    #endregion

    #region Delete

    /// <summary>
    /// Exclui um produto pelo seu ID.
    /// </summary>
    /// <param name="id">O ID do produto a ser excluído.</param>
    /// <returns>Um <see cref="IActionResult"/> que representa o resultado da operação.
    /// Retorna <see cref="StatusCodes.Status204NoContent"/> em caso de sucesso (produto excluído),
    /// <see cref="StatusCodes.Status404NotFound"/> se o produto não for encontrado,
    /// <see cref="StatusCodes.Status401Unauthorized"/> se o usuário não tiver permissão,
    /// e <see cref="StatusCodes.Status500InternalServerError"/> em caso de erro interno do servidor.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ExcluirProdutosAsync(int id)
    {
        try
        {
            var vendedorId = ObterIdDoUsuarioLogado();
            await _produtoService.ExcluirAsync(id, vendedorId);
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
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError, title: "Erro interno do servidor");
        }
    }

    #endregion

    private Guid ObterIdDoUsuarioLogado()
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
            return userId;
        }

        throw new FormatException($"Id de usuário inválido: {userIdClaim}");
    }}