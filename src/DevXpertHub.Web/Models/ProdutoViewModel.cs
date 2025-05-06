using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevXpertHub.Web.Models;

/// <summary>
/// Modelo de visualização (ViewModel) para a entidade Produto.
/// Contém as propriedades que serão exibidas e manipuladas nas views relacionadas a produtos.
/// Utiliza Data Annotations para definir regras de validação dos dados.
/// </summary>
public class ProdutoViewModel
{
    /// <summary>
    /// Identificador único do produto.
    /// </summary>
    [Key]
    public required string Id { get; set; }

    /// <summary>
    /// Nome do produto.
    /// </summary>
    [Required(ErrorMessage = "O preenchimento do nome do produto é obrigatório.")]
    public required string Nome { get; set; }

    /// <summary>
    /// Descrição do produto.
    /// </summary>
    [DisplayName("Descrição")]
    [Required(ErrorMessage = "O preenchimento da descrição do produto é obrigatório.")]
    public required string Descricao { get; set; }

    /// <summary>
    /// Caminho ou nome do arquivo da imagem do produto.
    /// </summary>
    [Required(ErrorMessage = "O preenchimento da imagem do produto é obrigatório.")]
    public required string Imagem { get; set; }

    /// <summary>
    /// Preço do produto.
    /// </summary>
    [Required(ErrorMessage = "O preenchimento do preço do produto é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que 0.")]
    public decimal Preco { get; set; }

    /// <summary>
    /// Quantidade em estoque do produto.
    /// </summary>
    [Required(ErrorMessage = "O preenchimento do estoque do produto é obrigatório.")]
    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
    public int Estoque { get; set; }

    /// <summary>
    /// Identificador da categoria à qual o produto pertence.
    /// É uma chave estrangeira referenciando a tabela de categorias.
    /// </summary>
    [Required(ErrorMessage = "O preenchimento da categoria do produto é obrigatório.")]
    [DisplayName("Categoria")]
    public required string CategoriaId { get; set; }

    /// <summary>
    /// Navegação property para a CategoriaViewModel relacionada.
    /// Permite acessar os dados da categoria associada ao produto.
    /// </summary>
    public CategoriaReadViewModel? Categoria { get; set; }

    /// <summary>
    /// Identificador do fornecedor que cadastrou o produto.
    /// É uma chave estrangeira referenciando a tabela de usuários (fornecedores).
    /// </summary>
    [DisplayName("Fornecedor")]
    [Required(ErrorMessage = "O preenchimento do fornecedor do produto é obrigatório.")]
    public required string FornecedorId { get; set; }

    /// <summary>
    /// Navegação property para a FornecedorViewModel relacionada.
    /// Permite acessar os dados do fornecedor associado ao produto.
    /// </summary>
    public FornecedorReadViewModel? Fornecedor { get; set; }
}