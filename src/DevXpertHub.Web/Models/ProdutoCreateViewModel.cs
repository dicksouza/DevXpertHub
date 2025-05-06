using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevXpertHub.Web.Models;

/// <summary>
/// Modelo de visualização (ViewModel) para a criação de um Produto.
/// Contém as propriedades que serão exibidas e manipuladas nas views relacionadas a produtos.
/// Utiliza Data Annotations para definir regras de validação dos dados.
/// </summary>
public class ProdutoCreateViewModel
{
    public ProdutoCreateViewModel() { }

    public ProdutoCreateViewModel(string fornecedorId, IEnumerable<SelectListItem> categorias)
    {
        FornecedorId = fornecedorId;
        Categorias = categorias;
    }

    [Required(ErrorMessage = "O preenchimento do nome do produto é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do produto deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O preenchimento da descrição do produto é obrigatório.")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "A descrição do produto deve ter entre 10 e 1000 caracteres.")]
    [DisplayName("Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O upload da imagem principal do produto é obrigatório.")]
    [DisplayName("Imagem Principal")]
    public IFormFile Imagem { get; set; } = null!;

    [Required(ErrorMessage = "O preenchimento do preço do produto é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que 0.")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "O preenchimento do estoque do produto é obrigatório.")]
    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
    public int Estoque { get; set; }

    [Required(ErrorMessage = "O preenchimento da categoria do produto é obrigatório.")]
    [DisplayName("Categoria")]
    public string CategoriaId { get; set; } = string.Empty;

    public IEnumerable<SelectListItem> Categorias { get; set; } = Enumerable.Empty<SelectListItem>();

    [Required(ErrorMessage = "O preenchimento do fornecedor do produto é obrigatório.")]
    [DisplayName("Fornecedor")]
    public string FornecedorId { get; set; } = string.Empty;
}