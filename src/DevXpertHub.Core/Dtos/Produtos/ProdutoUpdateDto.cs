namespace DevXpertHub.Core.Dtos.Produtos;

/// <summary>
/// Representa o modelo de aplicação para a atualização de um produto. Este DTO (Data Transfer Object)
/// é utilizado para transferir dados de produto entre as camadas da aplicação,
/// como a camada de serviço e a camada de apresentação (API/Web).
/// </summary>
public record ProdutoUpdateDto
(
    /// <summary>
    /// Identificador único do produto a ser atualizado.
    /// </summary>
    string Id,

    /// <summary>
    /// Nome do produto.
    /// </summary>
    string Nome,

    /// <summary>
    /// Descrição do produto.
    /// </summary>
    string Descricao,

    /// <summary>
    /// Preço do produto.
    /// </summary>
    decimal Preco,

    /// <summary>
    /// Quantidade em estoque do produto.
    /// </summary>
    int Estoque,

    /// <summary>
    /// Identificador da categoria à qual o produto pertence.
    /// </summary>
    string CategoriaId,

    /// <summary>
    /// Identificador do fornecedor que cadastrou o produto.
    /// </summary>
    string FornecedorId,

    /// <summary>
    /// Lista de caminhos ou URLs das imagens do produto.
    /// </summary>
    List<string> Imagens
);