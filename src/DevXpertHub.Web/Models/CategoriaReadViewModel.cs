using System.ComponentModel;

namespace DevXpertHub.Web.Models;

/// <summary>
/// Modelo de visualização (ViewModel) para um registro de Categoria.
/// Contém as propriedades que serão exibidas nas views relacionadas a categorias.
/// </summary>
public record CategoriaReadViewModel(
    /// <summary>
    /// Identificador único da categoria.
    /// É a chave primária no banco de dados.
    /// </summary>
    string Id,

    /// <summary>
    /// Nome da categoria.
    /// </summary>
    string Nome,

    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    [property: DisplayName("Descrição")] string Descricao
);