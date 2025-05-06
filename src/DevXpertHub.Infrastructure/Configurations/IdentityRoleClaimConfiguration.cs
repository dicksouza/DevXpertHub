using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpertHub.Infrastructure.Configurations;

/// <summary>
/// Define a configuração do Entity Framework Core para a entidade <see cref="IdentityRoleClaim{string}"/>.
/// Esta classe especifica como a relação entre um papel (Role) e suas reivindicações (Claims) será mapeada para a tabela correspondente no banco de dados
/// utilizada pelo ASP.NET Core Identity.
/// </summary>
public class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    /// <summary>
    /// Configura as propriedades e os relacionamentos da entidade <see cref="IdentityRoleClaim{string}"/>.
    /// Este método é chamado pelo Entity Framework Core durante a criação do modelo do banco de dados para configurar os detalhes da tabela de reivindicações de papéis.
    /// </summary>
    /// <param name="builder">O construtor usado para configurar a entidade <see cref="IdentityRoleClaim{string}"/>.</param>
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        // Configura a chave primária da tabela de reivindicações de papéis.
        // Por padrão, o IdentityRoleClaim utiliza um Id inteiro como chave primária.
        builder.HasKey(rc => rc.Id);

        // Outras configurações para a tabela de reivindicações de papéis podem ser adicionadas aqui,
        // como mapeamento de colunas, índices ou relacionamentos com outras entidades do Identity.
        // No entanto, para a configuração padrão, apenas a chave primária é explicitamente definida aqui.
    }
}