using DevXpertHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpertHub.Infrastructure.Configurations;

/// <summary>
/// Define a configuração do Entity Framework Core para a entidade <see cref="Fornecedor"/>.
/// Esta classe especifica como a entidade Fornecedor será mapeada para a tabela correspondente no banco de dados.
/// </summary>
public class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
{
    /// <summary>
    /// Configura as propriedades e os relacionamentos da entidade <see cref="Fornecedor"/>.
    /// Este método é chamado pelo Entity Framework Core durante a criação do modelo do banco de dados.
    /// </summary>
    /// <param name="builder">O construtor usado para configurar a entidade <see cref="Fornecedor"/>.</param>
    public void Configure(EntityTypeBuilder<Fornecedor> builder)
    {
        // Configura o nome da tabela no banco de dados.
        builder.ToTable("Fornecedores");

        // Configura a chave primária da tabela Fornecedores.
        builder.HasKey(f => f.Id);

        // Configura a propriedade Id para ser gerada automaticamente pelo banco de dados.
        builder.Property(f => f.Id)
            .ValueGeneratedOnAdd();

        // Configura a propriedade Nome:
        // Define que a coluna Nome é obrigatória e ajusta o tamanho máximo.
        builder.Property(f => f.Nome)
            .IsRequired() // Define que a coluna Nome não pode ser nula no banco de dados.
            .HasMaxLength(100); // Define o tamanho máximo da string.

        // Configura a propriedade Email:
        // Define que a coluna Email terá um tamanho máximo de 255 caracteres.
        builder.Property(f => f.Email)
            .HasMaxLength(255); // Define o tamanho máximo da string.

        // Configura o relacionamento com a entidade Produto:
        // Um fornecedor pode ter muitos produtos, mas um produto pertence a apenas um fornecedor.
        builder.HasMany(f => f.Produtos)
            .WithOne(p => p.Fornecedor)
            .HasForeignKey(p => p.FornecedorId)
            .OnDelete(DeleteBehavior.Restrict); // Define que a exclusão de um fornecedor não exclui seus produtos.
    }
}