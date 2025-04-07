using DevXpertHub.Core.Dtos;

namespace DevXpertHub.Core.Interfaces;

/// <summary>
/// Define a interface para um serviço de gerenciamento de categorias.
/// Esta interface declara os métodos que a camada de serviço de categoria deve implementar,
/// expondo funcionalidades para adicionar, obter, atualizar e excluir categorias.
/// </summary>
public interface ICategoriaService
{
    /// <summary>
    /// Adiciona uma nova categoria de forma assíncrona.
    /// </summary>
    /// <param name="categoriaDto">O DTO (Data Transfer Object) contendo os dados da categoria a ser adicionada.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO da categoria recém-adicionada.</returns>
    /// <exception cref="ArgumentException">Lançada se houver algum problema com os dados da categoria, como nome duplicado.</exception>
    Task<CategoriaApplicationModel> AdicionarAsync(CategoriaApplicationModel categoriaDto);

    /// <summary>
    /// Obtém todas as categorias de forma assíncrona.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de DTOs representando todas as categorias.</returns>
    Task<List<CategoriaApplicationModel>> ObterTodasAsync();

    /// <summary>
    /// Obtém uma categoria pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único da categoria a ser obtida.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO da categoria encontrada ou null se nenhuma categoria com o ID especificado for encontrada.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se nenhuma categoria com o ID especificado for encontrada.</exception>
    Task<CategoriaApplicationModel> ObterPorIdAsync(int id);

    /// <summary>
    /// Atualiza uma categoria existente de forma assíncrona.
    /// </summary>
    /// <param name="categoriaDto">O DTO contendo os dados atualizados da categoria. O ID da categoria deve estar presente.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO da categoria atualizada.</returns>
    /// <exception cref="ArgumentException">Lançada se houver algum problema com os dados da categoria, como nome duplicado ou ID inválido.</exception>
    /// <exception cref="KeyNotFoundException">Lançada se a categoria com o ID especificado não for encontrada.</exception>
    Task<CategoriaApplicationModel> AtualizarAsync(CategoriaApplicationModel categoriaDto);

    /// <summary>
    /// Exclui uma categoria pelo seu identificador único de forma assíncrona.
    /// </summary>
    /// <param name="id">O identificador único da categoria a ser excluída.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    /// <exception cref="KeyNotFoundException">Lançada se a categoria com o ID especificado não for encontrada.</exception>
    /// <exception cref="InvalidOperationException">Lançada se houver algum problema ao excluir a categoria (por exemplo, se houver produtos associados).</exception>
    Task ExcluirAsync(int id);
}