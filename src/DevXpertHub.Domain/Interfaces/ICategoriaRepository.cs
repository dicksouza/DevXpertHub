using DevXpertHub.Domain.Entities;

namespace DevXpertHub.Domain.Interfaces;

/// <summary>
/// Define a interface para um repositório de categorias.
/// Esta interface declara os métodos para realizar operações de acesso a dados
/// relacionadas à entidade <see cref="Categoria"/>.
/// </summary>
public interface ICategoriaRepository
{
    /// <summary>
    /// Adiciona uma nova categoria ao repositório de forma assíncrona.
    /// </summary>
    /// <param name="categoria">A entidade <see cref="Categoria"/> a ser adicionada.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Categoria"/> recém-adicionada, incluindo seu ID gerado (se aplicável).</returns>
    Task<Categoria> AdicionarAsync(Categoria categoria);

    /// <summary>
    /// Obtém uma categoria pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único da categoria a ser obtida.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Categoria"/> encontrada ou null se nenhuma categoria com o ID especificado existir.</returns>
    Task<Categoria?> ObterPorIdAsync(int id);

    /// <summary>
    /// Obtém todas as categorias do repositório de forma assíncrona.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de todas as entidades <see cref="Categoria"/> no repositório.</returns>
    Task<List<Categoria>> ObterTodasAsync();

    /// <summary>
    /// Atualiza uma categoria existente no repositório de forma assíncrona.
    /// </summary>
    /// <param name="categoria">A entidade <see cref="Categoria"/> com os dados atualizados.
    /// O ID da categoria a ser atualizada deve estar presente.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém a entidade <see cref="Categoria"/> após a atualização.</returns>
    Task<Categoria> AtualizarAsync(Categoria categoria);

    /// <summary>
    /// Exclui uma categoria do repositório pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único da categoria a ser excluída.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    Task ExcluirAsync(int id);

    /// <summary>
    /// Verifica de forma assíncrona se uma categoria possui produtos associados.
    /// </summary>
    /// <param name="cateogiraId">O identificador único da categoria a ser verificada.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// é true se a categoria possuir produtos associados, caso contrário, false.</returns>
    Task<bool> CategoriaPossuiProdutosAssociadosAsync(int cateogiraId);

    /// <summary>
    /// Verifica de forma assíncrona se já existe uma categoria com o nome especificado.
    /// </summary>
    /// <param name="nome">O nome da categoria a ser verificado.</param>
    /// <param name="id">O identificador único da categoria a ser ignorada na verificação (útil durante a atualização).
    /// Se nulo, a verificação será feita em todas as categorias.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// é true se existir uma categoria com o nome especificado (e ID diferente do fornecido, se houver), caso contrário, false.</returns>
    Task<bool> ExisteCategoriaComNomeAsync(string nome, int? id);
}