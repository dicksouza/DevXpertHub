using DevXpertHub.Api.Extensions;
using DevXpertHub.Core.Dtos.Posts;
using DevXpertHub.Core.Interfaces.Services;
using DevXpertHub.Core.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevXpertHub.Api.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações relacionadas a posts.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    #region Create

    /// <summary>
    /// Adiciona um novo post.
    /// </summary>
    /// <param name="postCreateDto">O modelo de dados do post a ser adicionado.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status201Created"/> com o post criado em caso de sucesso.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostDto))]
    public async Task<IActionResult> AdicionarPostAsync(PostCreateDto postCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var post = postCreateDto.MapToDomain();
        var postAdicionado = await _postService.AdicionarAsync(post);
        var postDto = postAdicionado.MapToDto();

        // Certifique-se de que o ID é válido e corresponde à rota
        if (string.IsNullOrEmpty(postDto.Id))
        {
            return BadRequest("O ID do post não foi gerado corretamente.");
        }

        return this.CreatedAtActionWithoutAsyncSuffix(nameof(ObterPorIdAsync), new { id = postDto.Id }, postDto);
    }

    #endregion

    #region Read

    /// <summary>
    /// Obtém um post pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único do post.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com o post encontrado em caso de sucesso,
    /// ou <see cref="StatusCodes.Status404NotFound"/> se o post não for encontrado.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [AllowAnonymous]
    public async Task<IActionResult> ObterPorIdAsync(string id)
    {
        var post = await _postService.ObterPorIdAsync(id);
        if (post == null)
        {
            return NotFound(Problem(title: "Post não encontrado", statusCode: StatusCodes.Status404NotFound));
        }

        var postDto = post.MapToDto();
        return Ok(postDto);
    }

    /// <summary>
    /// Obtém todos os posts.
    /// </summary>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com a lista de posts em caso de sucesso.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PostDto>))]
    [AllowAnonymous]
    public async Task<IActionResult> ObterTodosAsync()
    {
        var posts = await _postService.ObterTodosAsync();
        var postDtos = posts.Select(p =>p.MapToDto()).ToList();
        return Ok(postDtos);
    }

    #endregion

    #region Update

    /// <summary>
    /// Atualiza um post existente.
    /// </summary>
    /// <param name="id">O identificador único do post a ser atualizado.</param>
    /// <param name="postUpdateDto">Os dados atualizados do post.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com o post atualizado em caso de sucesso,
    /// ou <see cref="StatusCodes.Status404NotFound"/> se o post não for encontrado.
    /// </returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AtualizarPostAsync(string id, PostUpdateDto postUpdateDto)
    {
        if (id != postUpdateDto.Id)
        {
            return BadRequest(Problem(title: "ID do post não corresponde ao ID fornecido", statusCode: StatusCodes.Status400BadRequest));
        }

        var postExistente = await _postService.ObterPorIdAsync(id);
        if (postExistente == null)
        {
            return NotFound(Problem(title: "Post não encontrado", statusCode: StatusCodes.Status404NotFound));
        }

        postExistente.UpdateDomain(postUpdateDto);
        var postAtualizado = await _postService.AtualizarAsync(postExistente);
        var postDto = postAtualizado.MapToDto();

        return Ok(postDto);
    }

    #endregion

    #region Delete

    /// <summary>
    /// Exclui um post pelo seu identificador único.
    /// </summary>
    /// <param name="id">O identificador único do post a ser excluído.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status204NoContent"/> em caso de sucesso,
    /// ou <see cref="StatusCodes.Status404NotFound"/> se o post não for encontrado.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ExcluirPostAsync(string id)
    {
        var post = await _postService.ObterPorIdAsync(id);
        if (post == null)
        {
            return NotFound(Problem(title: "Post não encontrado", statusCode: StatusCodes.Status404NotFound));
        }

        await _postService.ExcluirAsync(id);
        return NoContent();
    }

    #endregion
}