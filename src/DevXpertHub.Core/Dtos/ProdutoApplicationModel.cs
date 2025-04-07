using System.ComponentModel.DataAnnotations;

namespace DevXpertHub.Core.Dtos;

/// <summary>
/// Representa o modelo de aplicação para um produto. Este DTO (Data Transfer Object)
/// é utilizado para transferir dados de produto entre as camadas da aplicação,
/// como a camada de serviço e a camada de apresentação (API/Web).
/// </summary>
public record ProdutoApplicationModel
{
    /// <summary>
    /// Obtém ou inicializa o identificador único do produto.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Obtém ou inicializa o nome do produto. Este campo é obrigatório e tem um limite de 100 caracteres.
    /// </summary>
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
    public required string Nome { get; init; }

    /// <summary>
    /// Obtém ou inicializa a descrição do produto. Este campo é obrigatório e tem um limite de 500 caracteres.
    /// </summary>
    [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
    [StringLength(500, ErrorMessage = "A descrição do produto deve ter no máximo 500 caracteres.")]
    public required string Descricao { get; init; }

    /// <summary>
    /// Obtém ou inicializa o caminho ou URL da imagem do produto. Este campo é obrigatório.
    /// </summary>
    [Required(ErrorMessage = "A imagem do produto é obrigatória.")]
    public required string Imagem { get; init; }

    /// <summary>
    /// Obtém ou inicializa o preço do produto. Deve ser um valor maior ou igual a 0.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "O preço deve ser maior ou igual a 0.")]
    public decimal Preco { get; init; }

    /// <summary>
    /// Obtém ou inicializa a quantidade em estoque do produto. Deve ser um valor inteiro maior ou igual a 0.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "O estoque deve ser maior ou igual a 0.")]
    public int Estoque { get; init; }

    /// <summary>
    /// Obtém ou inicializa o identificador da categoria à qual o produto pertence.
    /// </summary>
    public int CategoriaId { get; init; }

    /// <summary>
    /// Obtém ou inicializa o modelo de aplicação da categoria à qual o produto pertence.
    /// </summary>
    public required CategoriaApplicationModel? Categoria { get; init; }
}