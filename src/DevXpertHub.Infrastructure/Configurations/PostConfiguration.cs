using DevXpertHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpertHub.Infrastructure.Configurations;

/// <summary>
/// Configuração da entidade <see cref="Post"/> para o Entity Framework Core.
/// </summary>
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Descricao)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.Preco)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.ImagemUrl)
            .HasMaxLength(500);

        builder.Property(p => p.UsuarioId)
            .IsRequired();

        builder.Property(p => p.DataCriacao)
            .IsRequired();

        builder.HasMany(p => p.Comentarios)
            .WithOne(p => p.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}