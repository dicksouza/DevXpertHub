using DevXpertHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpertHub.Infrastructure.Configurations;

/// <summary>
/// Define a configuração do Entity Framework Core para a entidade <see cref="Categoria"/>.
/// Esta classe especifica como a entidade Categoria será mapeada para a tabela correspondente no banco de dados.
/// </summary>
public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    /// <summary>
    /// Configura as propriedades e os relacionamentos da entidade <see cref="Categoria"/>.
    /// Este método é chamado pelo Entity Framework Core durante a criação do modelo do banco de dados.
    /// </summary>
    /// <param name="builder">O construtor usado para configurar a entidade <see cref="Categoria"/>.</param>
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        // Configura a chave primária da tabela Categoria.
        builder.HasKey(c => c.Id);

        // Configura a propriedade Id para ser gerada automaticamente pelo banco de dados.
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        // Configura a propriedade Nome:
        builder.Property(c => c.Nome)
            .IsRequired() // Define que a coluna Nome não pode ser nula no banco de dados.
                          //.HasMaxLength(100) // Opção comentada para definir o tamanho máximo da string.
            .HasColumnType("VARCHAR(100)"); // Define o tipo da coluna como VARCHAR com tamanho máximo de 100 caracteres.

        // Configura a propriedade Descricao:
        builder.Property(c => c.Descricao)
            //.HasMaxLength(500) // Opção comentada para definir o tamanho máximo da string.
            .HasColumnType("VARCHAR(500)"); // Define o tipo da coluna como VARCHAR com tamanho máximo de 500 caracteres.
    }
}