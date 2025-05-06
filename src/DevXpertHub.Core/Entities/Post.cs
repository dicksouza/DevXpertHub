namespace DevXpertHub.Core.Entities;

/// <summary>
/// Representa uma entidade de post com detalhes como título, descrição, preço, imagem e comentários associados.
/// </summary>
public class Post : Entity
{
    /// <summary>
    /// Construtor principal para inicializar um post.
    /// </summary>
    /// <param name="titulo">Título do post.</param>
    /// <param name="descricao">Descrição do post.</param>
    /// <param name="preco">Preço do post.</param>
    /// <param name="imagemUrl">URL da imagem associada ao post.</param>
    /// <param name="usuarioId">Identificador do usuário que criou o post.</param>
    /// <param name="dataCriacao">Data de criação do post.</param>
    public Post(
        string titulo,
        string descricao,
        decimal preco,
        string imagemUrl,
        string usuarioId,
        DateTime dataCriacao)
    {
        Titulo = ValidarTitulo(titulo);
        Descricao = descricao;
        Preco = ValidarPreco(preco);
        ImagemUrl = imagemUrl;
        UsuarioId = usuarioId;
        DataCriacao = dataCriacao;
        Comentarios = new List<Comentario>();
    }

    /// <summary>
    /// Construtor protegido para uso pelo Entity Framework.
    /// </summary>
    protected Post()
    {
        Titulo = string.Empty;
        Descricao = string.Empty;
        ImagemUrl = string.Empty;
        UsuarioId = string.Empty;
        Comentarios = new List<Comentario>();
    }

    /// <summary>
    /// Obtém ou inicializa o título do post.
    /// </summary>
    public string Titulo { get; private set; }

    /// <summary>
    /// Obtém ou inicializa a descrição do post.
    /// </summary>
    public string Descricao { get; private set; }

    /// <summary>
    /// Obtém ou inicializa o preço do post.
    /// </summary>
    public decimal Preco { get; private set; }

    /// <summary>
    /// Obtém ou inicializa a URL da imagem associada ao post.
    /// </summary>
    public string ImagemUrl { get; private set; }

    /// <summary>
    /// Obtém ou inicializa o identificador do usuário que criou o post.
    /// </summary>
    public string UsuarioId { get; private set; }

    /// <summary>
    /// Obtém ou inicializa a data de criação do post.
    /// </summary>
    public DateTime DataCriacao { get; private set; }

    /// <summary>
    /// Obtém ou inicializa a lista de comentários associados ao post.
    /// </summary>
    public List<Comentario> Comentarios { get; private set; }

    /// <summary>
    /// Atualiza as propriedades do post.
    /// </summary>
    /// <param name="titulo">Novo título do post.</param>
    /// <param name="descricao">Nova descrição do post.</param>
    /// <param name="preco">Novo preço do post.</param>
    /// <param name="imagemUrl">Nova URL da imagem associada ao post.</param>
    public void Atualizar(string titulo, string descricao, decimal preco, string imagemUrl)
    {
        Titulo = ValidarTitulo(titulo);
        Descricao = descricao;
        Preco = ValidarPreco(preco);
        ImagemUrl = imagemUrl;
    }

    // Métodos de validação
    private static string ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("O título do post não pode ser vazio.");
        }
        return titulo;
    }

    private static decimal ValidarPreco(decimal preco)
    {
        if (preco < 0)
        {
            throw new ArgumentException("O preço do post não pode ser negativo.");
        }
        return preco;
    }
}