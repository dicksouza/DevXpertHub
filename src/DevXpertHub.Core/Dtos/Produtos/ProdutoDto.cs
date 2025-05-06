using DevXpertHub.Core.Dtos.Categorias;
using DevXpertHub.Core.Dtos.Fornecedores;
using System.ComponentModel.DataAnnotations;

namespace DevXpertHub.Core.Dtos.Produtos;

/// <summary>
/// Representa o modelo de aplicação para um produto. Este DTO (Data Transfer Object)
/// é utilizado para transferir dados de produto entre as camadas da aplicação,
/// como a camada de serviço e a camada de apresentação (API/Web).
/// </summary>
public record ProdutoDto
(
    /// <summary>
    /// Obtém ou inicializa o identificador único do produto.
    /// </summary>
    string Id,

    /// <summary>
    /// Obtém ou inicializa o nome do produto. Este campo é obrigatório e tem um limite de 100 caracteres.
    /// </summary>
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
    string Nome,

    /// <summary>
    /// Obtém ou inicializa a descrição do produto. Este campo é obrigatório e tem um limite de 500 caracteres.
    /// </summary>
    [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
    [StringLength(500, ErrorMessage = "A descrição do produto deve ter no máximo 500 caracteres.")]
    string Descricao,

    /// <summary>
    /// Obtém ou inicializa o preço do produto. Deve ser um valor maior ou igual a 0.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "O preço deve ser maior ou igual a 0.")]
    decimal Preco,

    /// <summary>
    /// Obtém ou inicializa a quantidade em estoque do produto. Deve ser um valor inteiro maior ou igual a 0.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "O estoque deve ser maior ou igual a 0.")]
    int Estoque,

    /// <summary>
    /// Obtém ou inicializa o identificador da categoria à qual o produto pertence.
    /// </summary>
    string CategoriaId,

    /// <summary>
    /// Obtém ou inicializa o modelo de aplicação da categoria à qual o produto pertence.
    /// </summary>
    CategoriaDto Categoria,

    /// <summary>
    /// Obtém ou inicializa o identificador do fornecedor do produto.
    /// </summary>
    string FornecedorId,

    /// <summary>
    /// Obtém ou inicializa o modelo de aplicação de forncedor ao qual o produto pertence.
    /// </summary>
    FornecedorDto? Fornecedor,

    /// <summary>
    /// Obtém ou inicializa a imagem principal do produto. 
    /// </summary>
    string ImagemPrincipal
);