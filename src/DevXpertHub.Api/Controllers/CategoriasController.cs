using DevXpertHub.Api.Extensions;
using DevXpertHub.Core.Dtos;
using DevXpertHub.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevXpertHub.Api.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações relacionadas a categorias de produtos.
/// Requer autenticação para a maioria das ações.
/// </summary>
[ApiController]
[Route("api/categorias")]
[Produces("application/json")]
[Authorize]
public class CategoriasController(ICategoriaService categoriaService) : ControllerBase
{
    private readonly ICategoriaService _categoriaService = categoriaService;

    #region Create

    /// <summary>
    /// Adiciona uma nova categoria.
    /// </summary>
    /// <param name="categoriaModel">O modelo de dados da categoria a ser adicionada.</param>
    /// <returns>Um <see cref="IActionResult"/> que representa o resultado da operação.
    /// Retorna <see cref="StatusCodes.Status201Created"/> com a categoria criada em caso de sucesso,
    /// <see cref="StatusCodes.Status400BadRequest"/> se o modelo for inválido ou ocorrer um erro de argumento,
    /// e <see cref="StatusCodes.Status500InternalServerError"/> em caso de erro interno do servidor.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoriaApplicationModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AdicionarAsync(CategoriaApplicationModel categoriaModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var categoriaAdicionada = await _categoriaService.AdicionarAsync(categoriaModel);
            return this.CreatedAtActionWithoutAsyncSuffix(nameof(ObterPorIdAsync),
                                                          new { id = categoriaAdicionada.Id },
                                                          categoriaAdicionada);
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

    #region Read

    /// <summary>
    /// Obtém todas as categorias. Acesso anônimo permitido.
    /// </summary>
    /// <returns>Um <see cref="IActionResult"/> que representa o resultado da operação.
    /// Retorna <see cref="StatusCodes.Status200OK"/> com a lista de categorias em caso de sucesso,
    /// e <see cref="StatusCodes.Status500InternalServerError"/> em caso de erro interno do servidor.</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoriaApplicationModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ObterTodasCategoriasAsync()
    {
        try
        {
            var categorias = await _categoriaService.ObterTodasAsync();
            return Ok(categorias);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError, title: "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Obtém uma categoria pelo seu ID. Acesso anônimo permitido.
    /// </summary>
    /// <param name="id">O ID da categoria a ser obtida.</param>
    /// <returns>Um <see cref="IActionResult"/> que representa o resultado da operação.
    /// Retorna <see cref="StatusCodes.Status200OK"/> com a categoria encontrada em caso de sucesso,
    /// <see cref="StatusCodes.Status404NotFound"/> se a categoria não for encontrada,
    /// e <see cref="StatusCodes.Status500InternalServerError"/> em caso de erro interno do servidor.</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoriaApplicationModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ObterPorIdAsync(int id)
    {
        try
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            return Ok(categoria);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(Problem(title: "Categoria não encontrada", detail: ex.Message, statusCode: StatusCodes.Status404NotFound));
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError, title: "Erro interno do servidor");
        }
    }

    #endregion

    #region Update

    /// <summary>
    /// Atualiza uma categoria existente pelo seu ID.
    /// </summary>
    /// <param name="id">O ID da categoria a ser atualizada.</param>
    /// <param name="categoriaModel">O modelo de dados da categoria com as informações atualizadas.</param>
    /// <returns>Um <see cref="IActionResult"/> que representa o resultado da operação.
    /// Retorna <see cref="StatusCodes.Status200OK"/> com a categoria atualizada em caso de sucesso,
    /// <see cref="StatusCodes.Status400BadRequest"/> se o modelo for inválido ou os IDs forem incompatíveis,
    /// <see cref="StatusCodes.Status404NotFound"/> se a categoria não for encontrada,
    /// e <see cref="StatusCodes.Status500InternalServerError"/> em caso de erro interno do servidor.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoriaApplicationModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AtualizarAsync(int id, CategoriaApplicationModel categoriaModel)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (id != categoriaModel.Id)
        {
            return BadRequest(Problem(title: "Erro na requisição", detail: "IDs de categoria incompatíveis.", statusCode: StatusCodes.Status400BadRequest));
        }

        try
        {
            var categoriaAtualizada = await _categoriaService.AtualizarAsync(categoriaModel);
            return Ok(categoriaAtualizada);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(Problem(title: "Categoria não encontrada", detail: ex.Message, statusCode: StatusCodes.Status404NotFound));
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
    /// Exclui uma categoria pelo seu ID.
    /// </summary>
    /// <param name="id">O ID da categoria a ser excluída.</param>
    /// <returns>Um <see cref="IActionResult"/> que representa o resultado da operação.
    /// Retorna <see cref="StatusCodes.Status204NoContent"/> em caso de sucesso (categoria excluída),
    /// <see cref="StatusCodes.Status404NotFound"/> se a categoria não for encontrada,
    /// e <see cref="StatusCodes.Status500InternalServerError"/> em caso de erro interno do servidor.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ExcluirAsync(int id)
    {
        try
        {
            await _categoriaService.ExcluirAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(Problem(title: "Categoria não encontrada", detail: ex.Message, statusCode: StatusCodes.Status404NotFound));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(Problem(title: "Erro na requisição", detail: ex.Message, statusCode: StatusCodes.Status400BadRequest));
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError, title: "Erro interno do servidor");
        }
    }

    #endregion
}