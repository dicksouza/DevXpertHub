using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace DevXpertHub.Api.Transformers;

/// <summary>
/// Transformer para o documento OpenAPI que configura o esquema de segurança "Bearer"
/// para autenticação JWT e o aplica como um requisito de segurança global para todas as operações da API.
/// </summary>
internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider = authenticationSchemeProvider;

    /// <summary>
    /// Transforma o documento OpenAPI assíncronamente para adicionar e aplicar o esquema de segurança Bearer (JWT).
    /// </summary>
    /// <param name="document">O documento OpenAPI a ser transformado.</param>
    /// <param name="context">O contexto da transformação.</param>
    /// <param name="cancellationToken">Um token para cancelar a operação, se necessário.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de transformação.</returns>
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        // Obtém todos os esquemas de autenticação configurados na aplicação.
        var authenticationSchemes = await _authenticationSchemeProvider.GetAllSchemesAsync();

        // Verifica se o esquema "Bearer" está configurado.
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            // Define o esquema de segurança Bearer.
            var bearerSecurityScheme = new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT usando o seguinte esquema: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header, // O token JWT será enviado no cabeçalho "Authorization".
                Type = SecuritySchemeType.ApiKey, // Indica que é uma chave de API (embora seja um token Bearer).
                Scheme = "Bearer", // O nome do esquema.
                BearerFormat = "JWT" // Especifica o formato como JWT.
            };

            // Adiciona o esquema de segurança aos componentes do documento OpenAPI.
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();
            document.Components.SecuritySchemes["Bearer"] = bearerSecurityScheme;

            // Aplica o requisito de segurança Bearer a todas as operações (endpoints) da API.
            foreach (var pathItem in document.Paths.Values)
            {
                foreach (var operation in pathItem.Operations.Values)
                {
                    operation.Security ??= new List<OpenApiSecurityRequirement>();
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        // Define o requisito de segurança referenciando o esquema "Bearer" definido nos componentes.
                        [new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }] = Array.Empty<string>() // Um array vazio de scopes, pois JWT geralmente não usa scopes dessa forma.
                    });
                }
            }
        }
    }
}