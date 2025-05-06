using System.ComponentModel;

namespace DevXpertHub.Web.Models;

/// <summary>
/// Modelo de visualização (ViewModel) para um registro de Fornecedor.
/// </summary>
public record FornecedorReadViewModel(
    /// <summary>
    /// Identificador único do fornecedor.
    /// É a chave primária no banco de dados.
    /// </summary>
    string Id,

    /// <summary>
    /// Nome do fornecedor.
    /// </summary>
    string Nome,

    /// <summary>
    /// Email do fornecedor.
    /// </summary>
    [property: DisplayName("E-mail")] string Email
);