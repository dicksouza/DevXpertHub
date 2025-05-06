namespace DevXpertHub.Core.Dtos.Produtos;

/// <summary>
/// DTO (Data Transfer Object) que representa as informações de uma imagem associada a um produto.
/// </summary>
public class ProdutoImagemDto
{
    /// <summary>
    /// Identificador único do produto ao qual a imagem está associada.
    /// </summary>
    public string ProdutoId { get; set; } = string.Empty;

    /// <summary>
    /// Caminho completo onde a imagem está armazenada no servidor.
    /// </summary>
    public string Caminho { get; set; } = string.Empty;
}