using DevXpertHub.Api.Transformers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

namespace DevXpertHub.Api.Extensions;

public static class OpenApiConfigurationExtensions
{
    /// <summary>
    /// Adiciona a configuração do Swagger/OpenAPI para a documentação da API.
    /// Utiliza a biblioteca Swashbuckle.AspNetCore.
    /// </summary>
    /// <param name="services">A interface IServiceCollection para adicionar os serviços.</param>
    /// <returns>A interface IServiceCollection para encadeamento.</returns>
    public static IServiceCollection AddOpenApiConfiguration(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });
        services.AddTransient<BearerSecuritySchemeTransformer>();
        services.AddTransient<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DevXpertHub API",
                Version = "v1",
                Description = "API para gerenciar categorias, produtos e usuários no frontend MVC."
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT usando o esquema Bearer: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                 {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                         }
                     },
                     new string[] {}
                 }
             });
        });

        return services;
    }
}