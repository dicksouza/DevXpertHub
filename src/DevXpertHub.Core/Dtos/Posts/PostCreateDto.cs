namespace DevXpertHub.Core.Dtos.Posts;

/// <summary>
/// DTO para criação de um novo post.
/// </summary>
public record PostCreateDto
(
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
    /// Identificador do usuário que está criando o post.
    /// </summary>
    string UsuarioId
);