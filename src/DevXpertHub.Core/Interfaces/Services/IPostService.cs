using DevXpertHub.Core.Entities;

namespace DevXpertHub.Core.Interfaces.Services;

/// <summary>
/// Define a interface para o serviço relacionado à entidade <see cref="Post"/>.
/// Esta interface declara os métodos para realizar operações de negócios relacionadas aos posts.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Adiciona um novo post de forma assíncrona.
    /// </summary>
    /// <param name="post">A entidade <see cref="Post"/> a ser adicionada.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o post recém-adicionado.</returns>
    Task<Post> AdicionarAsync(Post post);

    /// <summary>
    /// Obtém um post pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único do post a ser obtido.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o post encontrado ou null se não existir.</returns>
    Task<Post?> ObterPorIdAsync(string id);

    /// <summary>
    /// Obtém todos os posts de forma assíncrona.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém uma lista de todos os posts.</returns>
    Task<List<Post>> ObterTodosAsync();

    /// <summary>
    /// Atualiza um post existente de forma assíncrona.
    /// </summary>
    /// <param name="post">A entidade <see cref="Post"/> com os dados atualizados.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa contém o post atualizado.</returns>
    Task<Post> AtualizarAsync(Post post);

    /// <summary>
    /// Exclui um post pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único do post a ser excluído.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task ExcluirAsync(string id);
}