using DevXpertHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpertHub.Infrastructure.Configurations;

/// <summary>
/// Define a configuração do Entity Framework Core para a entidade <see cref="Produto"/>.
/// Esta classe especifica como a entidade Produto será mapeada para a tabela correspondente no banco de dados,
/// incluindo suas propriedades, tipos de dados, restrições e relacionamentos.
/// </summary>
public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    /// <summary>
    /// Configura as propriedades e os relacionamentos da entidade <see cref="Produto"/>.
    /// Este método é chamado pelo Entity Framework Core durante a criação do modelo do banco de dados.
    /// </summary>
    /// <param name="builder">O construtor usado para configurar a entidade <see cref="Produto"/>.</param>
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        // Configura a chave primária da tabela Produto.
        builder.HasKey(p => p.Id);

        // Configura a propriedade Id para ser gerada automaticamente pelo banco de dados.
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        // Configura a propriedade Nome:
        builder.Property(p => p.Nome)
            .IsRequired() // Define que a coluna Nome não pode ser nula no banco de dados.
                          //.HasMaxLength(200) // Opção comentada para definir o tamanho máximo da string.
            .HasColumnType("VARCHAR(200)"); // Define o tipo da coluna como VARCHAR com tamanho máximo de 200 caracteres.

        // Configura a propriedade Descricao:
        builder.Property(p => p.Descricao)
            //.HasMaxLength(1000) // Opção comentada para definir o tamanho máximo da string.
            .HasColumnType("VARCHAR(1000)"); // Define o tipo da coluna como VARCHAR com tamanho máximo de 1000 caracteres.

        // Configura a propriedade Preco para armazenar valores decimais com precisão de 18 dígitos e 2 casas decimais.
        builder.Property(p => p.Preco)
            .HasColumnType("decimal(18, 2)");

        // Configura a propriedade Imagem:
        builder.Property(p => p.Imagem)
            //.HasMaxLength(200) // Opção comentada para definir o tamanho máximo da string.
            .HasColumnType("VARCHAR(200)"); // Define o tipo da coluna como VARCHAR com tamanho máximo de 200 caracteres.

        // Configura o relacionamento com a entidade Categoria:
        // Um Produto pertence a uma Categoria.
        builder.HasOne(p => p.Categoria)
            .WithMany(c => c.Produtos) // Uma Categoria pode ter muitos Produtos.
            .HasForeignKey(p => p.CategoriaId) // Define CategoriaId como a chave estrangeira.
            .IsRequired() // Garante que todo Produto deve ter uma Categoria associada (a chave estrangeira não pode ser nula).
            .OnDelete(DeleteBehavior.Restrict); // Define o comportamento de exclusão para Restrict.
                                                // Isso impede a exclusão de uma Categoria se houver Produtos associados a ela,
                                                // garantindo a integridade referencial.

        // Outras configurações para a entidade Produto podem ser adicionadas aqui,
        // como índices para otimizar consultas ou outras restrições de banco de dados.
    }
}