using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevXpertHub.Web.Models;

/// <summary>
/// Modelo de visualização (ViewModel) para a atualização de um Produto.
/// Contém as propriedades que serão exibidas e manipuladas nas views relacionadas a produtos.
/// Utiliza Data Annotations para definir regras de validação dos dados.
/// </summary>
public class ProdutoUpdateViewModel
{
    public ProdutoUpdateViewModel()
    {
        Id = string.Empty;
        Nome = string.Empty;
        Descricao = string.Empty;
        ImagemAtual = string.Empty;
        ImagemNova = null!;
        Preco = 0;
        Estoque = 0;
        CategoriaId = string.Empty;
        FornecedorId = string.Empty;
        Categorias = Enumerable.Empty<SelectListItem>();
    }

    public ProdutoUpdateViewModel(
        string id,
        string nome,
        string descricao,
        string imagemAtual,
        decimal preco,
        int estoque,
        string categoriaId,
        string fornecedorId,
        IEnumerable<SelectListItem> categorias)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
        ImagemAtual = imagemAtual;
        Preco = preco;
        Estoque = estoque;
        CategoriaId = categoriaId;
        FornecedorId = fornecedorId;
        Categorias = categorias;
    }

    public ProdutoUpdateViewModel(
        string id,
        string nome,
        string descricao,
        string imagemAtual,
        IFormFile? imagemNova,
        decimal preco,
        int estoque,
        string categoriaId,
        string fornecedorId,
        IEnumerable<SelectListItem> categorias)
        : this(id, nome, descricao, imagemAtual, preco, estoque, categoriaId, fornecedorId, categorias)
    {
        ImagemNova = imagemNova;
    }

    [Key]
    [Required(ErrorMessage = "O preenchimento do ID do produto é obrigatório.")]
    public string Id { get; set; }

    [Required(ErrorMessage = "O preenchimento do nome do produto é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do produto deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O preenchimento da descrição do produto é obrigatório.")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "A descrição do produto deve ter entre 10 e 1000 caracteres.")]
    [DisplayName("Descrição")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "O caminho da imagem principal atual do produto é obrigatório.")]
    [DisplayName("Imagem Atual")]
    public string ImagemAtual { get; set; }

    [DisplayName("Nova Imagem")]
    public IFormFile? ImagemNova { get; set; }

    [Required(ErrorMessage = "O preenchimento do preço do produto é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que 0.")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "O preenchimento do estoque do produto é obrigatório.")]
    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
    public int Estoque { get; set; }

    [Required(ErrorMessage = "O preenchimento da categoria do produto é obrigatório.")]
    [DisplayName("Categoria")]
    public string CategoriaId { get; set; }

    public IEnumerable<SelectListItem> Categorias { get; set; }

    [Required(ErrorMessage = "O preenchimento do fornecedor do produto é obrigatório.")]
    [DisplayName("Fornecedor")]
    public string FornecedorId { get; set; }
}