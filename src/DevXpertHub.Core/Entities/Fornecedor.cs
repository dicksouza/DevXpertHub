namespace DevXpertHub.Core.Entities;

/// <summary>
/// Representa um fornecedor dentro do sistema.
/// </summary>
public class Fornecedor(string id, string nome, string email, ICollection<Produto>? produtos)
{
    /// <summary>
    /// Construtor protegido sem parâmetros para uso por Entity Framework ou outros ORMs.
    /// Inicializa as propriedades com valores padrão.
    /// </summary>
    protected Fornecedor() : this(string.Empty, string.Empty, string.Empty, null) { }

    /// <summary>
    /// Obtém ou inicializa o identificador único do fornecedor.
    /// Este identificador é o mesmo utilizado pelo Asp.Net Identity User.
    /// </summary>
    public string Id { get; init; } = id;

    /// <summary>
    /// Obtém ou inicializa o nome do fornecedor.
    /// </summary>
    public string Nome { get; init; } = nome;

    /// <summary>
    /// Obtém ou inicializa o endereço de e-mail do fornecedor.
    /// </summary>
    public string Email { get; init; } = email;

    /// <summary>
    /// Coleção de produtos associados a este fornecedor.
    /// </summary>
    public ICollection<Produto>? Produtos { get; init; } = produtos;
}