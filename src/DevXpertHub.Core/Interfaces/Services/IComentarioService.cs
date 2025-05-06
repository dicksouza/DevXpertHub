using DevXpertHub.Core.Entities;

namespace DevXpertHub.Core.Interfaces.Services;

/// <summary>
/// Interface para o serviço de gerenciamento de comentários.
/// Define os métodos para realizar operações de negócios relacionadas aos comentários.
/// </summary>
public interface IComentarioService
{
    /// <summary>
    /// Adiciona um novo comentário de forma assíncrona.
    /// </summary>
    /// <param name="comentario">A entidade <see cref="Comentario"/> a ser adicionada.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o comentário recém-adicionado.</returns>
    Task<Comentario> AdicionarAsync(Comentario comentario);

    /// <summary>
    /// Obtém um comentário pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único do comentário a ser obtido.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o comentário encontrado ou null se não existir.</returns>
    Task<Comentario?> ObterPorIdAsync(string id);

    /// <summary>
    /// Obtém todos os comentários associados a um post de forma assíncrona.
    /// </summary>
    /// <param name="postId">O identificador único do post ao qual os comentários estão associados.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma lista de comentários associados ao post.</returns>
    Task<List<Comentario>> ObterPorPostIdAsync(string postId);

    /// <summary>
    /// Atualiza um comentário existente de forma assíncrona.
    /// </summary>
    /// <param name="comentario">A entidade <see cref="Comentario"/> com os dados atualizados.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o comentário atualizado.</returns>
    Task<Comentario> AtualizarAsync(Comentario comentario);

    /// <summary>
    /// Exclui um comentário pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único do comentário a ser excluído.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task ExcluirAsync(string id);
}