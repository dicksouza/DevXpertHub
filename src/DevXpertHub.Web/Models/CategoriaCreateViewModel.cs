using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevXpertHub.Web.Models;

/// <summary>
/// Modelo de visualização (ViewModel) para a criar uma Categoria.
/// Contém as propriedades que serão exibidas e manipuladas nas views relacionadas a categorias.
/// Utiliza Data Annotations para definir regras de validação dos dados.
/// </summary>
public class CategoriaCreateViewModel
{
    /// <summary>
    /// Construtor padrão que inicializa as propriedades com valores padrão.
    /// </summary>
    public CategoriaCreateViewModel()
    {
        Nome = string.Empty;
        Descricao = string.Empty;
    }

    public CategoriaCreateViewModel(string nome, string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }

    /// <summary>
    /// Nome da categoria.
    /// O atributo [Required] indica que este campo é obrigatório.
    /// A mensagem de erro personalizada é fornecida.
    /// O atributo [StringLength] define o tamanho máximo da string permitida.
    /// A palavra-chave 'required' garante que a propriedade não seja nula.
    /// </summary>
    [Required(ErrorMessage = "É obrigatório informar o nome da categoria.")]
    [StringLength(100, ErrorMessage = "O nome da categoria deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; }

    /// <summary>
    /// Descrição da categoria.
    /// O atributo [DisplayName] define o nome a ser exibido para este campo nas views (labels de formulário, etc.).
    /// O atributo [Required] indica que este campo é obrigatório.
    /// A mensagem de erro personalizada é fornecida.
    /// O atributo [StringLength] define o tamanho máximo da string permitida.
    /// A palavra-chave 'required' garante que a propriedade não seja nula.
    /// </summary>
    [DisplayName("Descrição")]
    [Required(ErrorMessage = "É obrigatório informar a descrição da categoria.")]
    [StringLength(500, ErrorMessage = "A descrição da categoria deve ter no máximo 500 caracteres.")]
    public string Descricao { get; set; }
}