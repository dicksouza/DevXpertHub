using DevXpertHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpertHub.Infrastructure.Configurations;

/// <summary>
/// Configuração do Entity Framework Core para a entidade <see cref="Produto"/>.
/// Esta classe define como a entidade Produto será mapeada para a tabela no banco de dados,
/// especificando propriedades, tipos de dados, restrições e relacionamentos.
/// </summary>
public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    /// <summary>
    /// Configura as propriedades e relacionamentos da entidade <see cref="Produto"/>.
    /// </summary>
    /// <param name="builder">O construtor usado para configurar a entidade.</param>
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        // Define a chave primária
        builder.HasKey(p => p.Id);

        // Configura a propriedade Nome como obrigatória e com tamanho máximo de 100 caracteres
        builder.Property(p => p.Nome).IsRequired().HasMaxLength(100);

        // Configura a propriedade Descricao com tamanho máximo de 500 caracteres
        builder.Property(p => p.Descricao).HasMaxLength(500);

        // Configura a propriedade Preco com o tipo decimal(18,2)
        builder.Property(p => p.Preco).HasColumnType("decimal(18,2)");

        // Configura o relacionamento com a entidade Categoria
        builder.HasOne(p => p.Categoria)
               .WithMany(c => c.Produtos)
               .HasForeignKey(p => p.CategoriaId)
               .OnDelete(DeleteBehavior.Restrict);

        // Configura o relacionamento com a entidade Fornecedor
        builder.HasOne(p => p.Fornecedor)
               .WithMany(f => f.Produtos)
               .HasForeignKey(p => p.FornecedorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}