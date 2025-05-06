namespace DevXpertHub.Core.Dtos.Posts;

/// <summary>
/// DTO para representar os dados de um post.
/// </summary>
public record PostDto
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
    string ImagemUrl,

    /// <summary>
    /// Identificador do usuário que criou o post.
    /// </summary>
    string UsuarioId,

    /// <summary>
    /// Data de criação do post.
    /// </summary>
    DateTime DataCriacao
);