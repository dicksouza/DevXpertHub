using DevXpertHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevXpertHub.Infrastructure.Configurations;

public class ComentarioConfiguration : IEntityTypeConfiguration<Comentario>
{
    public void Configure(EntityTypeBuilder<Comentario> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Texto)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.PostId)
            .IsRequired();

        builder.Property(c => c.UsuarioId)
            .IsRequired();

        builder.Property(c => c.DataCriacao)
            .IsRequired();
    }
}