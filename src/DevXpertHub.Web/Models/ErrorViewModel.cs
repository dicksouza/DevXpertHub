namespace DevXpertHub.Web.Models;

/// <summary>
/// Modelo de visualiza��o (ViewModel) padr�o para exibir informa��es de erro.
/// Utilizado geralmente nas views de tratamento de erros da aplica��o.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Identificador da requisi��o que causou o erro.
    /// Pode ser �til para rastreamento e diagn�stico de problemas.
    /// � um tipo nullable (string?) pois pode n�o estar dispon�vel em todos os cen�rios de erro.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Propriedade de leitura que indica se o RequestId deve ser exibido na view.
    /// Retorna true se a propriedade RequestId n�o for nula nem vazia, e false caso contr�rio.
    /// Isso permite controlar a exibi��o do ID da requisi��o na p�gina de erro.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}