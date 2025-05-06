using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpertHub.Infrastructure.Configurations;

/// <summary>
/// Define a configuração do Entity Framework Core para a entidade <see cref="IdentityUserToken{string}"/>.
/// Esta classe especifica como os tokens de usuário (usados para funcionalidades como redefinição de senha, confirmação de e-mail)
/// serão mapeados para a tabela correspondente no banco de dados utilizada pelo ASP.NET Core Identity.
/// </summary>
public class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    /// <summary>
    /// Configura as propriedades e os relacionamentos da entidade <see cref="IdentityUserToken{string}"/>.
    /// Este método é chamado pelo Entity Framework Core durante a criação do modelo do banco de dados para configurar os detalhes da tabela de tokens de usuários.
    /// </summary>
    /// <param name="builder">O construtor usado para configurar a entidade <see cref="IdentityUserToken{string}"/>.</param>
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        // Configura a chave primária composta da tabela de tokens de usuários.
        // A chave primária é formada pela combinação de UserId, LoginProvider e Name,
        // garantindo a unicidade de um token específico para um usuário e um provedor de login (se aplicável).
        builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

        // Configura o relacionamento com a entidade de User (IdentityUser).
        // Define que muitos IdentityUserToken podem estar associados a um IdentityUser.
        // Configura a chave estrangeira para UserId e define que ela é obrigatória.
        builder.HasOne<IdentityUser>()
            .WithMany() // Um User pode ter muitos tokens.
            .HasForeignKey(t => t.UserId)
            .IsRequired();

        // Define o tamanho máximo para a coluna LoginProvider, seguindo as convenções do Identity.
        builder.Property(t => t.LoginProvider)
            .HasMaxLength(128)
            .IsRequired();

        // Define o tamanho máximo para a coluna Name, seguindo as convenções do Identity.
        builder.Property(t => t.Name)
            .HasMaxLength(128)
            .IsRequired();

        // Configura a coluna Value (o valor do token).
        // O tipo será ajustado automaticamente pelo provedor de banco de dados (e.g., TEXT no SQLite).
        builder.Property(t => t.Value)
            .IsRequired();

        // Adiciona índices para melhorar o desempenho de consultas frequentes.
        builder.HasIndex(t => t.LoginProvider)
            .HasDatabaseName("IX_UserToken_LoginProvider");

        builder.HasIndex(t => t.Name)
            .HasDatabaseName("IX_UserToken_Name");
    }
}