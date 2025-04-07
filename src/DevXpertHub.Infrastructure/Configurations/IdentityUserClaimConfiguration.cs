using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpertHub.Infrastructure.Configurations;

/// <summary>
/// Define a configuração do Entity Framework Core para a entidade <see cref="IdentityUserClaim{string}"/>.
/// Esta classe especifica como a relação entre um usuário (User) e suas reivindicações (Claims) será mapeada para a tabela correspondente no banco de dados
/// utilizada pelo ASP.NET Core Identity.
/// </summary>
public class IdentityUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    /// <summary>
    /// Configura as propriedades e os relacionamentos da entidade <see cref="IdentityUserClaim{string}"/>.
    /// Este método é chamado pelo Entity Framework Core durante a criação do modelo do banco de dados para configurar os detalhes da tabela de reivindicações de usuários.
    /// </summary>
    /// <param name="builder">O construtor usado para configurar a entidade <see cref="IdentityUserClaim{string}"/>.</param>
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
        // Configura a chave primária da tabela de reivindicações de usuários.
        // Por padrão, o IdentityUserClaim utiliza um Id inteiro como chave primária.
        builder.HasKey(uc => uc.Id);

        // Outras configurações para a tabela de reivindicações de usuários podem ser adicionadas aqui,
        // como mapeamento de colunas, índices ou relacionamentos com outras entidades do Identity.
        // No entanto, para a configuração padrão, apenas a chave primária é explicitamente definida aqui.
    }
}