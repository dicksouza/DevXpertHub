using System.ComponentModel.DataAnnotations;

namespace DevXpertHub.Api.Models;

/// <summary>
/// Classe de modelo para a requisição de registro de um novo usuário.
/// Contém as propriedades necessárias para criar uma nova conta de usuário.
/// </summary>
public class RegisterUserViewModel
{
    /// <summary>
    /// O endereço de e-mail do novo usuário.
    /// É obrigatório e deve estar em um formato de e-mail válido.
    /// </summary>
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// A senha do novo usuário.
    /// É obrigatória e deve ter um comprimento mínimo (definido nas políticas de senha).
    /// </summary>
    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// A confirmação da senha do novo usuário.
    /// Deve ser igual à senha fornecida para garantir que o usuário a digitou corretamente.
    /// </summary>
    [Compare("Password", ErrorMessage = "A senha e a confirmação não coincidem.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}