namespace DevXpertHub.Domain.Entities;

/// <summary>
/// Representa um produto dentro do sistema.
/// </summary>
public class Produto
{
    /// <summary>
    /// Obtém o identificador único do produto.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Obtém o nome do produto.
    /// </summary>
    public string Nome { get; init; }

    /// <summary>
    /// Obtém a descrição do produto.
    /// </summary>
    public string Descricao { get; init; }

    /// <summary>
    /// Obtém o preço do produto.
    /// </summary>
    public decimal Preco { get; init; }

    /// <summary>
    /// Obtém a quantidade em estoque do produto.
    /// </summary>
    public int Estoque { get; init; }

    /// <summary>
    /// Obtém o caminho ou URL da imagem do produto.
    /// </summary>
    public string Imagem { get; init; }

    /// <summary>
    /// Obtém o identificador único do vendedor que cadastrou o produto.
    /// </summary>
    public Guid VendedorId { get; init; }

    /// <summary>
    /// Obtém o identificador da categoria à qual o produto pertence.
    /// </summary>
    public int CategoriaId { get; init; }

    /// <summary>
    /// Obtém ou inicializa a categoria à qual o produto pertence (propriedade de navegação).
    /// </summary>
    public Categoria Categoria { get; init; }

    /// <summary>
    /// Construtor principal para criar uma instância de <see cref="Produto"/>.
    /// </summary>
    /// <param name="id">O identificador único do produto.</param>
    /// <param name="nome">O nome do produto.</param>
    /// <param name="descricao">A descrição do produto.</param>
    /// <param name="preco">O preço do produto.</param>
    /// <param name="estoque">A quantidade em estoque do produto.</param>
    /// <param name="categoriaId">O identificador da categoria à qual o produto pertence.</param>
    /// <param name="categoria">A categoria à qual o produto pertence.</param>
    /// <param name="vendedorId">O identificador único do vendedor que cadastrou o produto.</param>
    /// <param name="imagem">O caminho ou URL da imagem do produto.</param>
    public Produto(int id, string nome, string descricao, decimal preco, int estoque, int categoriaId, Categoria categoria, Guid vendedorId, string imagem)
    {
        Id = id;
        Nome = nome;
        //Nome = ValidarNome(nome); // Validação comentada, manter comentário para referência
        Descricao = descricao;
        Preco = ValidarPreco(preco);
        Estoque = ValidarEstoque(estoque);
        Imagem = imagem;
        VendedorId = vendedorId;
        CategoriaId = categoriaId;
        Categoria = categoria;
    }

    /// <summary>
    /// Construtor protegido sem parâmetros para uso por Entity Framework ou outros ORMs.
    /// Inicializa as propriedades com valores padrão.
    /// </summary>
    protected Produto() : this(default, string.Empty, string.Empty, default, default, default, new Categoria(string.Empty, string.Empty, default), default, string.Empty) { }

    /// <summary>
    /// Valida se o nome do produto não é nulo ou vazio.
    /// </summary>
    /// <param name="nome">O nome do produto a ser validado.</param>
    /// <returns>O nome do produto se for válido.</returns>
    /// <exception cref="ArgumentException">Lançada se o nome do produto for nulo ou vazio.</exception>
    private static string ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("O nome do produto não pode ser vazio.");
        }
        return nome;
    }

    /// <summary>
    /// Valida se o preço do produto não é negativo.
    /// </summary>
    /// <param name="preco">O preço do produto a ser validado.</param>
    /// <returns>O preço do produto se for válido.</returns>
    /// <exception cref="ArgumentException">Lançada se o preço do produto for negativo.</exception>
    private static decimal ValidarPreco(decimal preco)
    {
        if (preco < 0)
        {
            throw new ArgumentException("O preço do produto não pode ser negativo.");
        }
        return preco;
    }

    /// <summary>
    /// Valida se o estoque do produto não é negativo.
    /// </summary>
    /// <param name="estoque">A quantidade em estoque a ser validada.</param>
    /// <returns>A quantidade em estoque se for válida.</returns>
    /// <exception cref="ArgumentException">Lançada se o estoque do produto for negativo.</exception>
    private static int ValidarEstoque(int estoque)
    {
        if (estoque < 0)
        {
            throw new ArgumentException("O estoque do produto não pode ser negativo.");
        }
        return estoque;
    }
}