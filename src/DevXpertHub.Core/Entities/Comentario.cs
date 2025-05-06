namespace DevXpertHub.Core.Entities;

/// <summary>
/// Representa um comentário associado a um post.
/// </summary>
public class Comentario : Entity
{
    /// <summary>
    /// Construtor com parâmetros para inicializar as propriedades.
    /// </summary>
    public Comentario(string texto, string postId, string usuarioId, DateTime dataCriacao)
    {
        Texto = texto;
        PostId = postId;
        UsuarioId = usuarioId;
        DataCriacao = dataCriacao;
    }

    /// <summary>
    /// Construtor protegido para uso pelo Entity Framework.
    /// </summary>
    protected Comentario()
    {
        Texto = string.Empty;
        PostId = string.Empty;
        UsuarioId = string.Empty;
    }

    /// <summary>
    /// Texto do comentário.
    /// </summary>
    public string Texto { get; init; }

    /// <summary>
    /// ID do usuário que fez o comentário.
    /// </summary>
    public string UsuarioId { get; init; }

    /// <summary>
    /// Data / Hora de criação do comentário.
    /// </summary>
    public DateTime DataCriacao { get; init; }

    /// <summary>
    /// ID do post relacionado.
    /// </summary>
    public string PostId { get; init; }

    /// <summary>
    /// Navegação para o post relacionado.
    /// </summary>
    public Post Post { get; set; } = null!;
}