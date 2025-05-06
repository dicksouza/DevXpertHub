using DevXpertHub.Core.Dtos.Posts;
using DevXpertHub.Core.Entities;

namespace DevXpertHub.Core.Mappers;

/// <summary>
/// Classe utilitária para mapear entre a entidade <see cref="Post"/> e seus DTOs.
/// </summary>
public static class PostMapper
{
    /// <summary>
    /// Mapeia a entidade <see cref="Post"/> para um DTO simplificado.
    /// </summary>
    /// <param name="post">A entidade <see cref="Post"/> a ser mapeada.</param>
    /// <returns>Um objeto DTO contendo os dados do post.</returns>
    public static PostDto MapToDto(this Post post)
    {
        return new PostDto(
            post.Id,
            post.Titulo,
            post.Descricao,
            post.Preco,
            post.ImagemUrl,
            post.UsuarioId,
            post.DataCriacao
        );
    }

    /// <summary>
    /// Mapeia um DTO para a entidade <see cref="Post"/>.
    /// </summary>
    /// <param name="dto">O DTO contendo os dados do post.</param>
    /// <returns>A entidade <see cref="Post"/> correspondente.</returns>
    public static Post MapToDomain(this PostDto dto)
    {
        return new Post(
            dto.Titulo,
            dto.Descricao,
            dto.Preco,
            dto.ImagemUrl,
            dto.UsuarioId,
            dto.DataCriacao
        )
        {
            Id = dto.Id
        };
    }

    /// <summary>
    /// Mapeia um DTO de criação para a entidade <see cref="Post"/>.
    /// </summary>
    /// <param name="dto">O DTO de criação contendo os dados do post.</param>
    /// <returns>A entidade <see cref="Post"/> correspondente.</returns>
    public static Post MapToDomain(this PostCreateDto dto)
    {
        return new Post(
            dto.Titulo,
            dto.Descricao,
            dto.Preco,
            dto.ImagemUrl,
            dto.UsuarioId,
            DateTime.UtcNow
        );
    }

    /// <summary>
    /// Atualiza uma entidade <see cref="Post"/> existente com base em um DTO de atualização.
    /// </summary>
    /// <param name="dto">O DTO de atualização contendo os novos dados do post.</param>
    /// <param name="existingPost">A entidade <see cref="Post"/> existente a ser atualizada.</param>
    public static void UpdateDomain(this Post existingPost, PostUpdateDto dto)
    {
        existingPost.Atualizar(
            dto.Titulo,
            dto.Descricao,
            dto.Preco,
            dto.ImagemUrl
        );
    }}