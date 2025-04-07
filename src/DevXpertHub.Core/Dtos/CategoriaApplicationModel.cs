using System.ComponentModel.DataAnnotations;

namespace DevXpertHub.Core.Dtos;

/// <summary>
/// Representa o modelo de aplicação para uma categoria. Este DTO (Data Transfer Object)
/// é usado para transferir dados de categoria entre as camadas da aplicação,
/// como a camada de serviço e a camada de apresentação (API/Web).
/// </summary>
/// <param name="Id">O identificador único da categoria.</param>
/// <param name="Nome">O nome da categoria. É um campo obrigatório e deve ter no máximo 100 caracteres.</param>
/// <param name="Descricao">A descrição da categoria. É um campo obrigatório e deve ter no máximo 500 caracteres.</param>
public record CategoriaApplicationModel(
    /// <summary>
    /// Obtém o identificador único da categoria.
    /// </summary>
    int Id,

    /// <summary>
    /// Obtém o nome da categoria. Este campo é obrigatório e tem um limite de 100 caracteres.
    /// </summary>
    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome da categoria deve ter no máximo 100 caracteres.")]
    string Nome,

    /// <summary>
    /// Obtém a descrição da categoria. Este campo é obrigatório e tem um limite de 500 caracteres.
    /// </summary>
    [Required(ErrorMessage = "A descrição da categoria é obrigatória.")]
    [StringLength(500, ErrorMessage = "A descrição da categoria deve ter no máximo 500 caracteres.")]
    string Descricao
);