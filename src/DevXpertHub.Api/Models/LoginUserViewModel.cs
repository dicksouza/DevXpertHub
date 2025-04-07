using System.ComponentModel.DataAnnotations;

namespace DevXpertHub.Api.Models;

/// <summary>
/// Classe de modelo para a requisição de login de um usuário existente.
/// Contém as propriedades necessárias para autenticar um usuário.
/// </summary>
public class LoginUserViewModel
{
    /// <summary>
    /// O endereço de e-mail do usuário para login.
    /// É obrigatório e deve estar em um formato de e-mail válido.
    /// </summary>
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// A senha do usuário para login.
    /// É obrigatória.
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Password { get; set; } = string.Empty;
}