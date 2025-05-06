using DevXpertHub.Api.Models;
using DevXpertHub.Core.Entities;
using DevXpertHub.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SQLitePCL;
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
[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
public class AuthController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly AppDbContext _context;

    /// <summary>
    /// Construtor da classe <see cref="AuthController"/>.
    /// </summary>
    /// <param name="signInManager">Serviço para gerenciar a autenticação de usuários.</param>
    /// <param name="userManager">Serviço para gerenciar usuários.</param>
    /// <param name="jwtSettings">Configurações do JWT injetadas através do sistema de opções.</param>
    public AuthController(SignInManager<IdentityUser> signInManager,
                          UserManager<IdentityUser> userManager,
                          IOptions<JwtSettings> jwtSettings,
                          AppDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _context = context;
    }

    /// <summary>
    /// Endpoint para registrar um novo usuário.
    /// </summary>
    /// <param name="registerUser">Modelo contendo os dados necessários para o registro do usuário.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com o token JWT em caso de sucesso.
    /// </returns>
    [HttpPost("registrar")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<ActionResult<string>> Registrar(RegisterUserViewModel registerUser)
    {
        var user = new IdentityUser
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            EmailConfirmed = true // Em um cenário real, isso exigiria um fluxo de confirmação por e-mail.
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);

        if (!result.Succeeded) return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));

        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // Atribuir a role ao usuário
        await _userManager.AddToRoleAsync(user, registerUser.Role);

        // Se a role for "Fornecedor", registra o usuário na tabela Fornecedor
        if (registerUser.Role == "Fornecedor")
        {
            // Registrar na tabela Fornecedor
            var fornecedor = new Fornecedor
            (
            userId,
            registerUser.Nome,
            user.Email,
            null
            );

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();
        }

        // Após o registro bem-sucedido, o usuário é logado e um token JWT é gerado.
        await _signInManager.SignInAsync(user, isPersistent: false); // isPersistent: false para sessão do navegador.
        return Ok(await GerarJwt(user.Email));
    }

    /// <summary>
    /// Endpoint para realizar o login de um usuário existente.
    /// </summary>
    /// <param name="loginUser">Modelo contendo as credenciais de login do usuário.</param>
    /// <returns>
    /// Retorna <see cref="StatusCodes.Status200OK"/> com o token JWT em caso de sucesso.
    /// </returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
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
        return BadRequest(result.IsLockedOut ? "Conta de usuário bloqueada." : "Usuário ou senha incorretos");
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
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Adiciona as roles do usuário como claims de role.

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