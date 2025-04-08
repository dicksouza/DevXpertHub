namespace DevXpertHub.Domain.Entities;

/// <summary>
/// Representa uma categoria de produtos dentro do sistema.
/// </summary>
/// <param name="id">O identificador único da categoria.</param>
/// <param name="nome">O nome da categoria.</param>
/// <param name="descricao">A descrição da categoria.</param>
public class Categoria(string nome, string descricao, int id = 0)
{
    /// <summary>
    /// Obtém o identificador único da categoria.
    /// </summary>
    public int Id { get; init; } = id;

    /// <summary>
    /// Obtém o nome da categoria.
    /// </summary>
    public string Nome { get; init; } = nome;

    /// <summary>
    /// Obtém a descrição da categoria.
    /// </summary>
    public string Descricao { get; init; } = descricao;

    /// <summary>
    /// Obtém ou inicializa a coleção de produtos pertencentes a esta categoria.
    /// Pode ser nulo se a navegação não for carregada.
    /// </summary>
    public ICollection<Produto>? Produtos { get; init; }
}