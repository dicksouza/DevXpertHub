using DevXpertHub.Api.Extensions;
using DevXpertHub.Core.Dtos.Categorias;
using DevXpertHub.Core.Interfaces.Services;
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
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
[Authorize]
public class CategoriasController(ICategoriaService categoriaService) : ControllerBase
{
    private readonly ICategoriaService _categoriaService = categoriaService;

    #region Create

    /// <summary>
    /// Adiciona uma nova categoria.
    /// </summary>
    /// <param name="categoriaModel">O modelo de dados da categoria a ser adicionada.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status201Created"/> com a categoria criada em caso de sucesso.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoriaDto))]
    public async Task<IActionResult> AdicionarAsync(CategoriaCreateDto categoriaModel)
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
    }

    #endregion

    #region Read

    /// <summary>
    /// Obtém todas as categorias. Acesso anônimo permitido.
    /// </summary>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com a lista de categorias em caso de sucesso.
    /// </returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoriaDto>))]
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
    /// Obtém todas as categorias com produtos associados. Acesso anônimo permitido.
    /// </summary>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com a lista de categorias em caso de sucesso.
    /// </returns>
    [HttpGet("com-produtos-associados")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoriaDto>))]
    public async Task<IActionResult> ObterCategoriasComProdutos()
    {
        var categorias = await _categoriaService.ObterCategoriasComProdutosAsync();
        return Ok(categorias);
    }

    /// <summary>
    /// Obtém uma categoria pelo seu ID. Acesso anônimo permitido.
    /// </summary>
    /// <param name="id">O ID da categoria a ser obtida.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com a categoria encontrada em caso de sucesso,
    /// ou <see cref="StatusCodes.Status404NotFound"/> se a categoria não for encontrada.
    /// </returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoriaDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ObterPorIdAsync(string id)
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
    }

    #endregion

    #region Update

    /// <summary>
    /// Atualiza uma categoria existente pelo seu ID.
    /// </summary>
    /// <param name="id">O ID da categoria a ser atualizada.</param>
    /// <param name="categoriaModel">O modelo de dados da categoria com as informações atualizadas.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com a categoria atualizada em caso de sucesso,
    /// ou <see cref="StatusCodes.Status404NotFound"/> se a categoria não for encontrada.
    /// </returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoriaDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AtualizarAsync(string id, CategoriaUpdateDto categoriaModel)
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
    }

    #endregion

    #region Delete

    /// <summary>
    /// Exclui uma categoria pelo seu ID.
    /// </summary>
    /// <param name="id">O ID da categoria a ser excluída.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status204NoContent"/> em caso de sucesso,
    /// ou <see cref="StatusCodes.Status404NotFound"/> se a categoria não for encontrada.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ExcluirAsync(string id)
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
    }

    #endregion
}
