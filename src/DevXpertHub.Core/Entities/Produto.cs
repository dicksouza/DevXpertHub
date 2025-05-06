namespace DevXpertHub.Core.Entities;

/// <summary>
/// Representa um produto dentro do sistema.
/// </summary>
public class Produto : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public decimal Preco { get; private set; }
    public int Estoque { get; private set; }
    public string CategoriaId { get; private set; } = string.Empty;
    public Categoria Categoria { get; private set; } = null!;
    public string FornecedorId { get; private set; } = string.Empty;
    public Fornecedor Fornecedor { get; private set; } = null!;
    public string ImagemPrincipal { get; private set; } = null!;

    /// <summary>
    /// Construtor principal para inicializar um produto.
    /// </summary>
    public Produto(
        string nome,
        string descricao,
        decimal preco,
        int estoque,
        string categoriaId,
        string fornecedorId,
        string imagem)
    {
        Nome = ValidarNome(nome);
        Descricao = descricao;
        Preco = ValidarPreco(preco);
        Estoque = ValidarEstoque(estoque);
        CategoriaId = categoriaId;
        FornecedorId = fornecedorId;
        ImagemPrincipal = string.IsNullOrWhiteSpace(imagem)
        ? $"uploads/{this.FornecedorId}/{this.Id}/{imagem}"
        : imagem;
    }

    /// <summary>
    /// Método para atualizar as propriedades do produto.
    /// </summary>
    public void Atualizar(
        string nome,
        string descricao,
        decimal preco,
        int estoque,
        string categoriaId,
        string fornecedorId,
        string imagemPrincipal)
    {
        Nome = ValidarNome(nome);
        Descricao = descricao;
        Preco = ValidarPreco(preco);
        Estoque = ValidarEstoque(estoque);
        CategoriaId = categoriaId;
        FornecedorId = fornecedorId;
        ImagemPrincipal = imagemPrincipal;
    }

    /// <summary>
    /// Construtor protegido para uso pelo Entity Framework.
    /// </summary>
    protected Produto() { }

    // Métodos de validação
    private static string ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("O nome do produto não pode ser vazio.");
        }
        return nome;
    }

    private static decimal ValidarPreco(decimal preco)
    {
        if (preco < 0)
        {
            throw new ArgumentException("O preço do produto não pode ser negativo.");
        }
        return preco;
    }

    private static int ValidarEstoque(int estoque)
    {
        if (estoque < 0)
        {
            throw new ArgumentException("O estoque do produto não pode ser negativo.");
        }
        return estoque;
    }
}
