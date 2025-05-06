namespace DevXpertHub.Core.Dtos.Posts;

/// <summary>
/// DTO para atualização de um post existente.
/// </summary>
public record PostUpdateDto
(
    /// <summary>
    /// Identificador único do post.
    /// </summary>
    string Id,

    /// <summary>
    /// Título do post.
    /// </summary>
    string Titulo,

    /// <summary>
    /// Descrição do post.
    /// </summary>
    string Descricao,

    /// <summary>
    /// Preço do post.
    /// </summary>
    decimal Preco,

    /// <summary>
    /// URL da imagem associada ao post.
    /// </summary>
    string ImagemUrl
);