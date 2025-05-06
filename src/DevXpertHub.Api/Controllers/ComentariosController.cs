using DevXpertHub.Api.Extensions;
using DevXpertHub.Core.Dtos.Posts;
using DevXpertHub.Core.Entities;
using DevXpertHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevXpertHub.Api.Controllers;

[ApiController]
[Route("api/posts/{postId}/comentarios")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ComentariosController : ControllerBase
{
    private readonly IComentarioService _comentarioService;

    public ComentariosController(IComentarioService comentarioService)
    {
        _comentarioService = comentarioService;
    }

    /// <summary>
    /// Adiciona um novo comentário a um post.
    /// </summary>
    /// <param name="comentarioDto">Dados do comentário a ser criado.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status201Created"/> com o comentário criado em caso de sucesso.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Comentario))]
    public async Task<IActionResult> AdicionarComentario(ComentarioCreateDto comentarioDto)
    {
        try
        {
            var comentario = new Comentario(
                comentarioDto.Texto,
                comentarioDto.PostId,
                comentarioDto.UsuarioId,
                DateTime.UtcNow
            );

            var resultado = await _comentarioService.AdicionarAsync(comentario);
            return this.CreatedAtActionWithoutAsyncSuffix(nameof(ObterPorId), new { id = resultado.Id }, resultado);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError, title: "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Obtém um comentário específico pelo ID.
    /// </summary>
    /// <param name="id">ID do comentário.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com o comentário encontrado em caso de sucesso,
    /// ou <see cref="StatusCodes.Status404NotFound"/> se o comentário não for encontrado.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Comentario))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(string id)
    {
        try
        {
            var comentario = await _comentarioService.ObterPorIdAsync(id);
            if (comentario == null)
            {
                return NotFound(Problem(title: "Comentário não encontrado", statusCode: StatusCodes.Status404NotFound));
            }

            return Ok(comentario);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError, title: "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Atualiza um comentário existente.
    /// </summary>
    /// <param name="id">ID do comentário a ser atualizado.</param>
    /// <param name="comentarioDto">Dados atualizados do comentário.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com o comentário atualizado em caso de sucesso,
    /// ou <see cref="StatusCodes.Status404NotFound"/> se o comentário não for encontrado.
    /// </returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Comentario))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarComentario(string id, ComentarioUpdateDto comentarioDto)
    {
        try
        {
            var comentario = await _comentarioService.ObterPorIdAsync(id);
            if (comentario == null)
            {
                return NotFound(Problem(title: "Comentário não encontrado", statusCode: StatusCodes.Status404NotFound));
            }

            var comentarioAtualizado = new Comentario(
                comentarioDto.Texto,
                comentario.PostId,
                comentario.UsuarioId,
                comentario.DataCriacao
            )
            {
                Id = comentario.Id,
            };

            var atualizado = await _comentarioService.AtualizarAsync(comentarioAtualizado);
            return Ok(atualizado);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError, title: "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Exclui um comentário pelo ID.
    /// </summary>
    /// <param name="id">ID do comentário a ser excluído.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status204NoContent"/> em caso de sucesso,
    /// ou <see cref="StatusCodes.Status404NotFound"/> se o comentário não for encontrado.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExcluirComentario(string id)
    {
        try
        {
            var comentario = await _comentarioService.ObterPorIdAsync(id);
            if (comentario == null)
            {
                return NotFound(Problem(title: "Comentário não encontrado", statusCode: StatusCodes.Status404NotFound));
            }

            await _comentarioService.ExcluirAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError, title: "Erro interno do servidor");
        }
    }
}