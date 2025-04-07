using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpertHub.Infrastructure.Configurations;

/// <summary>
/// Define a configuração do Entity Framework Core para a entidade <see cref="IdentityUserRole{string}"/>.
/// Esta classe especifica como a relação entre um usuário (User) e seus papéis (Roles) será mapeada para a tabela correspondente no banco de dados
/// utilizada pelo ASP.NET Core Identity.
/// </summary>
public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    /// <summary>
    /// Configura as propriedades e os relacionamentos da entidade <see cref="IdentityUserRole{string}"/>.
    /// Este método é chamado pelo Entity Framework Core durante a criação do modelo do banco de dados para configurar os detalhes da tabela de junção entre usuários e papéis.
    /// Configurar explicitamente os relacionamentos é uma prática recomendada para garantir a integridade referencial e clareza no modelo.
    /// </summary>
    /// <param name="builder">O construtor usado para configurar a entidade <see cref="IdentityUserRole{string}"/>.</param>
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        // Configura a chave primária composta da tabela de junção entre usuários e papéis.
        // A chave primária é formada pela combinação de UserId e RoleId,
        // garantindo que cada usuário tenha uma única associação com um determinado papel.
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        // Configura o relacionamento com a entidade de Role (IdentityRole).
        // Define que muitos IdentityUserRole podem estar associados a um IdentityRole.
        // Configura a chave estrangeira para RoleId e define que ela é obrigatória.
        builder.HasOne<IdentityRole>()
            .WithMany() // Um Role pode ter muitos UserRoles.
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        // Configura o relacionamento com a entidade de User (IdentityUser).
        // Define que muitos IdentityUserRole podem estar associados a um IdentityUser.
        // Configura a chave estrangeira para UserId e define que ela é obrigatória.
        builder.HasOne<IdentityUser>()
            .WithMany() // Um User pode ter muitos UserRoles.
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        // Outras configurações para a tabela de junção entre usuários e papéis podem ser adicionadas aqui,
        // como mapeamento de colunas ou índices, se necessário.
    }
}