using Microsoft.AspNetCore.Mvc;

namespace DevXpertHub.Api.Extensions;

/// <summary>
/// Métodos de extensão para a classe ControllerBase, fornecendo helpers para ações comuns.
/// </summary>
public static class ControllerExtensions
{
    /// <summary>
    /// Retorna um objeto CreatedAtActionResult que referencia uma ação para recuperar o recurso recém-criado.
    /// Este método lida com a inconsistência em algumas versões do ASP.NET Core
    /// onde o sufixo "Async" no nome da ação causava problemas na resolução da rota.
    /// </summary>
    /// <param name="controller">A instância da ControllerBase na qual o método de extensão está sendo chamado.</param>
    /// <param name="actionNameWithAsync">O nome da ação para recuperar o recurso, incluindo o sufixo "Async" (se presente).</param>
    /// <param name="routeValues">Um objeto que contém os valores de rota a serem usados para gerar a URL.</param>
    /// <param name="value">O valor do objeto a ser formatado no corpo da resposta.</param>
    /// <returns>Um objeto CreatedAtActionResult que produz uma resposta HTTP 201 (Created) com um header Location.</returns>
    public static CreatedAtActionResult CreatedAtActionWithoutAsyncSuffix(
        this ControllerBase controller,
        string actionNameWithAsync,
        object routeValues,
        object value)
    {
        // Remove o sufixo "Async" do nome da ação, se ele estiver presente.
        string actionName = actionNameWithAsync;
        if (actionName.EndsWith("Async", StringComparison.OrdinalIgnoreCase))
        {
            actionName = actionName.Substring(0, actionName.Length - "Async".Length);
        }

        // Chama o método CreatedAtAction padrão com o nome da ação ajustado.
        return controller.CreatedAtAction(actionName, routeValues, value);
    }
}