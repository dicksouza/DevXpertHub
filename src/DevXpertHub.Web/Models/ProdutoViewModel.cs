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
    /// É a chave primária no banco de dados.
    /// O atributo [Key] indica isso ao Entity Framework.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Nome do produto.
    /// O atributo [Required] indica que este campo é obrigatório.
    /// A palavra-chave 'required' garante que a propriedade não seja nula.
    /// </summary>
    [Required(ErrorMessage = "O preenchimento do nome do produto é obrigatório.")]
    public required string Nome { get; set; }

    /// <summary>
    /// Descrição do produto.
    /// O atributo [DisplayName] define o nome a ser exibido para este campo nas views.
    /// O atributo [Required] indica que este campo é obrigatório.
    /// A palavra-chave 'required' garante que a propriedade não seja nula.
    /// </summary>
    [DisplayName("Descrição")]
    [Required(ErrorMessage = "O preenchimento da descrição do produto é obrigatório.")]
    public required string Descricao { get; set; }

    /// <summary>
    /// Caminho ou nome do arquivo da imagem do produto.
    /// O atributo [Required] indica que este campo é obrigatório.
    /// A palavra-chave 'required' garante que a propriedade não seja nula.
    /// </summary>
    [Required(ErrorMessage = "O preenchimento da imagem do produto é obrigatório.")]
    public required string Imagem { get; set; }

    /// <summary>
    /// Preço do produto.
    /// O atributo [Required] indica que este campo é obrigatório.
    /// O atributo [Range] define os limites mínimo e máximo para o valor do preço.
    /// A mensagem de erro personalizada é fornecida para valores fora do intervalo.
    /// </summary>
    [Required(ErrorMessage = "O preenchimento do preço do produto é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que 0.")]
    public decimal Preco { get; set; }

    /// <summary>
    /// Quantidade em estoque do produto.
    /// O atributo [Required] indica que este campo é obrigatório.
    /// O atributo [Range] define os limites mínimo e máximo para o valor do estoque.
    /// A mensagem de erro personalizada é fornecida para valores fora do intervalo.
    /// </summary>
    [Required(ErrorMessage = "O preenchimento do estoque do produto é obrigatório.")]
    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
    public int Estoque { get; set; }

    /// <summary>
    /// Identificador da categoria à qual o produto pertence.
    /// O atributo [Required] indica que este campo é obrigatório.
    /// O atributo [DisplayName] define o nome a ser exibido para este campo nas views.
    /// É uma chave estrangeira referenciando a tabela de categorias.
    /// </summary>
    [Required(ErrorMessage = "O preenchimento da categoria do produto é obrigatório.")]
    [DisplayName("Categoria")]
    public int CategoriaId { get; set; }

    /// <summary>
    /// Navegação property para a CategoriaViewModel relacionada.
    /// Permite acessar os dados da categoria associada ao produto.
    /// É um tipo nullable (CategoriaViewModel?) pois pode não ser carregado em todas as consultas.
    /// </summary>
    public CategoriaViewModel? Categoria { get; set; }

    /// <summary>
    /// Identificador do vendedor que cadastrou o produto.
    /// O atributo [DisplayName] define o nome a ser exibido para este campo nas views.
    /// O atributo [Required] indica que este campo é obrigatório.
    /// É uma chave estrangeira referenciando a tabela de usuários (vendedores).
    /// </summary>
    [DisplayName("Vendedor")]
    [Required(ErrorMessage = "O preenchimento do vendedor do produto é obrigatório.")]
    public Guid VendedorId { get; set; }
}