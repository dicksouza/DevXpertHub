using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace DevXpertHub.Api.Transformers;

/// <summary>
/// Transformer para o documento OpenAPI que remove o esquema de segurança padrão do Swagger UI
/// (normalmente um esquema OAuth2 implícito) se o esquema de segurança Bearer (JWT) estiver configurado.
/// Isso ajuda a manter a interface do Swagger UI mais limpa e focada no método de autenticação principal da API.
/// </summary>
internal sealed class RemoveDefaultSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    /// <summary>
    /// Transforma o documento OpenAPI assíncronamente para remover o esquema de segurança padrão, se aplicável.
    /// </summary>
    /// <param name="document">O documento OpenAPI a ser transformado.</param>
    /// <param name="context">O contexto da transformação.</param>
    /// <param name="cancellationToken">Um token para cancelar a operação, se necessário.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de transformação.</returns>
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            // Verifica se o esquema de segurança "Bearer" (JWT) foi adicionado aos componentes de segurança.
            if (document?.Components?.SecuritySchemes?.ContainsKey("Bearer") == true)
            {
                // Itera sobre todos os caminhos (endpoints) da API.
                foreach (var pathItem in document.Paths.Values)
                {
                    // Itera sobre todas as operações (métodos HTTP) em cada caminho.
                    foreach (var operation in pathItem.Operations.Values)
                    {
                        // Verifica se a operação possui requisitos de segurança definidos.
                        if (operation.Security != null && operation.Security.Any())
                        {
                            // Cria uma nova lista para armazenar os requisitos de segurança que não são o esquema padrão.
                            var updatedSecurityRequirements = new List<OpenApiSecurityRequirement>();

                            // Itera sobre os requisitos de segurança atuais da operação.
                            foreach (var requirement in operation.Security)
                            {
                                // Verifica se o requisito de segurança atual não é o esquema de segurança padrão (que geralmente não tem uma chave específica).
                                // A lógica exata para identificar o esquema padrão pode variar dependendo da configuração do Swagger/NSwag.
                                // Aqui, assumimos que esquemas com apenas uma chave (que não é "Bearer") podem ser o padrão a ser removido.
                                if (requirement.Keys.Count == 1 && !requirement.ContainsKey(new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme } }))
                                {
                                    // Se for o esquema padrão (ou outro esquema que você deseja remover quando Bearer está presente), ele não é adicionado à lista atualizada.
                                    continue;
                                }
                                // Se não for o esquema padrão (ou for o esquema Bearer), adicione-o à lista atualizada.
                                updatedSecurityRequirements.Add(requirement);
                            }

                            // Substitui a lista de requisitos de segurança da operação pela lista atualizada.
                            operation.Security = updatedSecurityRequirements;
                        }
                    }
                }

                // Opcional: Se o esquema de segurança padrão não estiver sendo usado em nenhum lugar após a remoção,
                // você pode removê-lo completamente dos componentes de segurança para uma limpeza adicional.
                // Isso requer uma análise mais profunda do documento para garantir que não haja referências a ele.
                // Exemplo (requer mais lógica para garantir que não há referências):
                // document.Components.SecuritySchemes.RemoveWhere(kvp => kvp.Key != "Bearer");
            }
        }, cancellationToken);
    }
}
