using System.ComponentModel.DataAnnotations;

namespace DevXpertHub.Core.Dtos.Fornecedores;

/// <summary>
/// Representa o modelo de aplicação para um fornecedor. Este DTO (Data Transfer Object)
/// é usado para transferir dados de um fornecedor entre as camadas da aplicação,
/// como a camada de serviço e a camada de apresentação (API/Web).
/// </summary>
public record FornecedorDto
(
    /// <summary>
    /// Identificador único e obrigatório do fornecedor.
    /// Este identificador é o mesmo utilizado pelo Asp.Net Identity User.
    /// </summary>
    [Required(ErrorMessage = "O ID é obrigatório.")]
    string Id,

    /// <summary>
    /// Nome do fornecedor.
    /// Este campo é obrigatório e tem um limite de 100 caracteres.
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    string Nome,

    /// <summary>
    /// Email do fornecedor.
    /// Este campo é obrigatório e deve ser um e-mail válido.
    /// </summary>
    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email informado não é válido.")]
    string Email
);