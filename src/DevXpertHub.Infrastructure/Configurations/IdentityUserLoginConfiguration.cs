using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpertHub.Infrastructure.Configurations;

/// <summary>
/// Define a configuração do Entity Framework Core para a entidade <see cref="IdentityUserLogin{string}"/>.
/// Esta classe especifica como os logins externos de um usuário (como Facebook, Google) serão mapeados para a tabela correspondente no banco de dados
/// utilizada pelo ASP.NET Core Identity.
/// </summary>
public class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    /// <summary>
    /// Configura as propriedades e os relacionamentos da entidade <see cref="IdentityUserLogin{string}"/>.
    /// Este método é chamado pelo Entity Framework Core durante a criação do modelo do banco de dados para configurar os detalhes da tabela de logins de usuários.
    /// </summary>
    /// <param name="builder">O construtor usado para configurar a entidade <see cref="IdentityUserLogin{string}"/>.</param>
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        // Configura a chave primária composta da tabela de logins de usuários.
        // A chave primária é formada pela combinação de LoginProvider e ProviderKey,
        // garantindo a unicidade de um login externo para um usuário.
        builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });

        // Outras configurações para a tabela de logins de usuários podem ser adicionadas aqui,
        // como mapeamento de colunas, índices ou relacionamentos com a entidade de usuário (IdentityUser).
        // No entanto, para a configuração básica, apenas a chave primária composta é explicitamente definida aqui.
    }
}