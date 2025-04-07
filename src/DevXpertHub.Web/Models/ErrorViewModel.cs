namespace DevXpertHub.Web.Models;

/// <summary>
/// Modelo de visualização (ViewModel) padrão para exibir informações de erro.
/// Utilizado geralmente nas views de tratamento de erros da aplicação.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Identificador da requisição que causou o erro.
    /// Pode ser útil para rastreamento e diagnóstico de problemas.
    /// É um tipo nullable (string?) pois pode não estar disponível em todos os cenários de erro.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Propriedade de leitura que indica se o RequestId deve ser exibido na view.
    /// Retorna true se a propriedade RequestId não for nula nem vazia, e false caso contrário.
    /// Isso permite controlar a exibição do ID da requisição na página de erro.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}