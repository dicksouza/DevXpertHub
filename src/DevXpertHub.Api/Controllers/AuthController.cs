using DevXpertHub.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevXpertHub.Api.Controllers;

/// <summary>
/// Controller responsável pela autenticação de usuários, incluindo registro e login,
/// e geração de tokens JWT para acesso a recursos protegidos.
/// </summary>
[ApiController]
[AllowAnonymous] // Permite acesso a todos os endpoints deste controller sem autenticação.
[Route("api/conta")]
[Produces("application/json")] // Especifica que as respostas da API serão no formato JSON.
public class AuthController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    /// <summary>
    /// Construtor da classe <see cref="AuthController"/>.
    /// </summary>
    /// <param name="signInManager">Serviço para gerenciar a autenticação de usuários.</param>
    /// <param name="userManager">Serviço para gerenciar usuários.</param>
    /// <param name="jwtSettings">Configurações do JWT injetadas através do sistema de opções.</param>
    public AuthController(SignInManager<IdentityUser> signInManager,
                          UserManager<IdentityUser> userManager,
                          IOptions<JwtSettings> jwtSettings)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
    /// Endpoint para registrar um novo usuário.
    /// </summary>
    /// <param name="registerUser">Modelo contendo os dados necessários para o registro do usuário.</param>
    /// <returns>Um <see cref="IActionResult"/> que representa o resultado da operação de registro.
    /// Em caso de sucesso, retorna um token JWT no formato OK (200).
    /// Em caso de falha, retorna um BadRequest (400) com uma mensagem de erro.</returns>
    [HttpPost("registrar")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))] // Indica que em caso de sucesso retorna uma string (o token JWT).
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))] // Indica que em caso de erro retorna uma string (a mensagem de erro).
    public async Task<ActionResult<string>> Registrar(RegisterUserViewModel registerUser)
    {
        var user = new IdentityUser
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            EmailConfirmed = true // Em um cenário real, isso exigiria um fluxo de confirmação por e-mail.
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);

        if (result.Succeeded)
        {
            // Após o registro bem-sucedido, o usuário é logado e um token JWT é gerado.
            await _signInManager.SignInAsync(user, isPersistent: false); // isPersistent: false para sessão do navegador.
            return Ok(await GerarJwt(user.Email));
        }

        // Se o registro falhar, retorna um BadRequest com os erros.
        return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    /// <summary>
    /// Endpoint para realizar o login de um usuário existente.
    /// </summary>
    /// <param name="loginUser">Modelo contendo as credenciais de login do usuário.</param>
    /// <returns>Um <see cref="IActionResult"/> que representa o resultado da operação de login.
    /// Em caso de sucesso, retorna um token JWT no formato OK (200).
    /// Em caso de falha, retorna um BadRequest (400) com uma mensagem de erro.</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))] // Indica que em caso de sucesso retorna uma string (o token JWT).
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))] // Indica que em caso de erro retorna uma string (a mensagem de erro).
    public async Task<ActionResult<string>> Login(LoginUserViewModel loginUser)
    {
        // Tenta realizar o login do usuário usando as credenciais fornecidas.
        var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, isPersistent: false, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            // Se o login for bem-sucedido, gera e retorna um token JWT.
            return Ok(await GerarJwt(loginUser.Email));
        }

        // Se o login falhar devido a credenciais inválidas ou conta bloqueada.
        if (result.IsLockedOut)
        {
            return BadRequest("Conta de usuário bloqueada.");
        }

        return BadRequest("Usuário ou senha incorretos");
    }

    /// <summary>
    /// Método privado para gerar um token JWT para um usuário com base em seu e-mail.
    /// </summary>
    /// <param name="email">O e-mail do usuário para o qual o token será gerado.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o token JWT codificado como uma string.</returns>
    /// <exception cref="ArgumentNullException">Ocorre se o usuário não for encontrado pelo e-mail fornecido.</exception>
    private async Task<string> GerarJwt(string email)
    {
        // Busca o usuário pelo e-mail.
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "O usuário não pode ser nulo.");
        }

        // Obtém as roles do usuário.
        var roles = await _userManager.GetRolesAsync(user);

        // Cria as claims (informações) que serão incluídas no token.
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // ID do usuário.
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty), // Nome de usuário.
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),   // E-mail do usuário.
        };

        // Adiciona as roles do usuário como claims de role.
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Cria um handler de token JWT.
        var tokenHandler = new JwtSecurityTokenHandler();
        // Obtém a chave secreta para assinar o token, convertendo-a para bytes.
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

        // Define as características do token a ser criado.
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), // As claims a serem incluídas no token.
            Issuer = _jwtSettings.Issuer,         // A autoridade que emite o token.
            Audience = _jwtSettings.Audience,     // Os destinatários válidos do token.
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationTime), // A data e hora de expiração do token.
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // As credenciais de assinatura.
        };

        // Cria o token JWT.
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Escreve (serializa) o token para uma string JWT formatada.
        var encodedToken = tokenHandler.WriteToken(token);

        return encodedToken;
    }
}