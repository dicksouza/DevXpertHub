namespace DevXpertHub.Api.Models;

/// <summary>
/// Classe de modelo que representa as configurações do JSON Web Token (JWT)
/// utilizadas para autenticação e autorização na API.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// A chave secreta usada para assinar o token JWT.
    /// É crucial manter essa chave segura e confidencial.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// O tempo de expiração do token JWT em horas.
    /// Após esse período, o token não será mais válido e o usuário precisará se autenticar novamente.
    /// </summary>
    public int ExpirationTime { get; set; }

    /// <summary>
    /// O emissor do token JWT, ou seja, a autoridade que gerou e assinou o token.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// A audiência do token JWT, ou seja, a aplicação ou serviço para o qual o token se destina.
    /// </summary>
    public string Audience { get; set; } = string.Empty;
}