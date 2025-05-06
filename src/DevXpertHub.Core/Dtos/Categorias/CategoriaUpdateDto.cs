namespace DevXpertHub.Core.Dtos.Categorias;

/// <summary>
/// DTO para atualização de uma categoria.
/// Contém as propriedades necessárias para atualizar uma categoria existente.
/// </summary>
public record CategoriaUpdateDto(

    /// <summary>
    /// Identificador único da categoria.
    /// </summary>
    string Id,

    /// <summary>
    /// Nome da categoria.
    /// </summary>
    string Nome,

    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    string Descricao
);