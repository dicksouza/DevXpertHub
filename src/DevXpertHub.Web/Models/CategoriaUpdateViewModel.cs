using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevXpertHub.Web.Models;

/// <summary>
/// Modelo de visualização (ViewModel) para atualizar uma Categoria.
/// Contém as propriedades que serão exibidas e manipuladas nas views relacionadas a categorias.
/// </summary>
public class CategoriaUpdateViewModel
{
    /// <summary>
    /// Construtor padrão sem parâmetros.
    /// </summary>
    public CategoriaUpdateViewModel()
    {
        Id = string.Empty;
        Nome = string.Empty;
        Descricao = string.Empty;
    }

    /// <summary>
    /// Construtor com parâmetros para inicializar as propriedades.
    /// </summary>
    public CategoriaUpdateViewModel(string id, string nome, string descricao)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
    }

    /// <summary>
    /// Identificador único da categoria.
    /// </summary>
    [Key]
    public string Id { get; set; }

    /// <summary>
    /// Nome da categoria.
    /// </summary>
    [Required(ErrorMessage = "É obrigatório informar o nome da categoria.")]
    [StringLength(100, ErrorMessage = "O nome da categoria deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; }

    /// <summary>
    /// Descrição da categoria.
    /// </summary>
    [DisplayName("Descrição")]
    [Required(ErrorMessage = "É obrigatório informar a descrição da categoria.")]
    [StringLength(500, ErrorMessage = "A descrição da categoria deve ter no máximo 500 caracteres.")]
    public string Descricao { get; set; }
}
