using DevXpertHub.Core.Dtos;
using DevXpertHub.Core.Interfaces;
using DevXpertHub.Web.Mappers;
using DevXpertHub.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DevXpertHub.Web.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações relacionadas a produtos no sistema web.
/// </summary>
[Authorize]
public class ProdutosController(IProdutoService produtoService) : Controller
{
    private readonly IProdutoService _produtoService = produtoService;

    // Executado antes de cada ação do controlador.
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        TempData["ErrorMessage"] = null;
        TempData["SuccessMessage"] = null;
        base.OnActionExecuting(context);
    }

    #region Index

    /// <summary>
    /// Exibe a lista de todos os produtos.
    /// </summary>
    /// <returns>A view com a lista de produtos.</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        TempData["ErrorMessage"] = null;
        try
        {
            List<ProdutoViewModel> produtosViewModel;
            List<ProdutoApplicationModel> produtos;

            // Verifica se o usuário está logado.
            if (User.Identity?.IsAuthenticated == true)
            {
                // Obtém o ID do vendedor logado.
                var vendedorIdLogado = ObterVendedorIdDoUsuarioLogado();
                // Chama o serviço para obter todos os produtos.
                produtos = await _produtoService.ObterTodosPorVendedorAsync(vendedorIdLogado);
                // Mapeia a lista de DTOs para uma lista de ViewModels utilizando o mapper.
                produtosViewModel = produtos.Select(ProdutoMapper.ToViewModel).ToList();
                return View(produtosViewModel);
            }
            else
            {
                // Chama o serviço para obter todos os produtos.
                produtos = await _produtoService.ObterTodosAsync();
                // Mapeia a lista de DTOs para uma lista de ViewModels utilizando o mapper.
                produtosViewModel = produtos.Select(ProdutoMapper.ToViewModel).ToList();
            }
            return View(produtosViewModel);
        }
        catch (Exception ex)
        {
            // Em caso de erro, armazena a mensagem no TempData para exibição na view.
            TempData["ErrorMessage"] = "Ocorreu um erro ao carregar os produtos: " + ex.Message;
            return View(new List<ProdutoViewModel>());
        }
    }

    #endregion

    #region Create

    /// <summary>
    /// Exibe o formulário para criar um novo produto.
    /// </summary>
    /// <returns>A view com o formulário de criação.</returns>
    [HttpGet]
    public IActionResult Create()
    {
        TempData["ErrorMessage"] = null;
        return View();
    }

    /// <summary>
    /// Processa a submissão do formulário de criação de um novo produto.
    /// </summary>
    /// <param name="produtoViewModel">O modelo de dados do produto a ser criado.</param>
    /// <returns>Redireciona para a página de índice em caso de sucesso, ou retorna o formulário com erros de validação.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(ProdutoViewModel produtoViewModel)
    {
        // Verifica se o modelo recebido é válido de acordo com as Data Annotations.
        if (ModelState.IsValid)
        {
            try
            {
                // Obtém o ID do vendedor logado.
                var vendedorIdLogado = ObterVendedorIdDoUsuarioLogado();
                // Mapeia o ViewModel para o DTO utilizando o mapper.
                var produtoDto = ProdutoMapper.ToApplicationModel(produtoViewModel);
                // Chama o serviço para adicionar o novo produto.
                await _produtoService.AdicionarAsync(produtoDto, vendedorIdLogado);
                // Em caso de sucesso, armazena uma mensagem no TempData e redireciona para o índice.
                TempData["SuccessMessage"] = "Produto criado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                // Se ocorrer um erro de argumento, adiciona o erro ao ModelState.
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                // Se ocorrer um erro inesperado, adiciona uma mensagem genérica ao ModelState.
                ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao criar o produto: " + ex.Message);
            }
        }
        // Se o ModelState não for válido, retorna a view com o ViewModel para exibir os erros.
        return View(produtoViewModel);
    }

    #endregion

    #region Edit

    /// <summary>
    /// Exibe o formulário para editar um produto existente.
    /// </summary>
    /// <param name="id">O ID do produto a ser editado.</param>
    /// <returns>A view com o formulário de edição preenchido com os dados do produto, ou NotFound se o produto não existir.</returns>
    [HttpGet]
    public async Task<IActionResult> EditAsync(int id)
    {
        TempData["ErrorMessage"] = null;
        try
        {
            // Chama o serviço para obter o produto pelo ID.
            var produto = await _produtoService.ObterPorIdAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            // Mapeia o DTO para o ViewModel utilizando o mapper.
            var produtoViewModel = ProdutoMapper.ToViewModel(produto);
            return View(produtoViewModel);
        }
        catch (KeyNotFoundException)
        {
            // Se o produto não for encontrado, armazena a mensagem no TempData e redireciona para o índice.
            TempData["ErrorMessage"] = "Produto não encontrado.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Se ocorrer um erro inesperado, armazena a mensagem no TempData e redireciona para o índice.
            TempData["ErrorMessage"] = "Ocorreu um erro inesperado ao carregar o produto: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Processa a submissão do formulário de edição de um produto existente.
    /// </summary>
    /// <param name="id">O ID do produto a ser editado.</param>
    /// <param name="produtoViewModel">O modelo de dados do produto com as informações atualizadas.</param>
    /// <returns>Redireciona para a página de índice em caso de sucesso, ou retorna o formulário com erros de validação.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(int id, ProdutoViewModel produtoViewModel)
    {
        // Verifica se o ID da rota corresponde ao ID no ViewModel.
        if (id != produtoViewModel.Id)
        {
            TempData["ErrorMessage"] = "O ID do produto não corresponde ao esperado.";
            return BadRequest();
        }

        // Verifica se o modelo recebido é válido.
        if (ModelState.IsValid)
        {
            try
            {
                // Verifica se o produto existe antes de tentar atualizar.
                var produtoExistente = await _produtoService.ObterPorIdAsync(id);
                if (produtoExistente == null)
                {
                    return NotFound();
                }

                // Cria um novo DTO com os dados atualizados do ViewModel.
                var produtoDto = new ProdutoApplicationModel
                {
                    Id = produtoViewModel.Id,
                    Nome = produtoViewModel.Nome,
                    Descricao = produtoViewModel.Descricao,
                    Preco = produtoViewModel.Preco,
                    Estoque = produtoViewModel.Estoque,
                    CategoriaId = produtoViewModel.CategoriaId,
                    Categoria = produtoViewModel.Categoria != null ? new CategoriaApplicationModel(
                        produtoViewModel.Categoria.Id,
                        produtoViewModel.Categoria.Nome,
                        produtoViewModel.Categoria.Descricao
                    ) : null,
                    Imagem = produtoViewModel.Imagem
                };

                // Obtém o ID do vendedor logado.
                var vendedorIdLogado = ObterVendedorIdDoUsuarioLogado();
                // Chama o serviço para atualizar o produto.
                await _produtoService.AtualizarAsync(produtoDto, vendedorIdLogado);
                // Em caso de sucesso, armazena uma mensagem no TempData e redireciona para o índice.
                TempData["SuccessMessage"] = "Produto atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                // Adiciona erros específicos ao ModelState.
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (KeyNotFoundException)
            {
                // Adiciona erro de produto não encontrado ao ModelState.
                ModelState.AddModelError(string.Empty, "Produto não encontrado.");
            }
            catch (Exception ex)
            {
                // Adiciona erro genérico ao ModelState.
                ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao editar o produto: " + ex.Message);
            }
        }
        // Se o ModelState não for válido, retorna a view com o ViewModel para exibir os erros.
        return View(produtoViewModel);
    }

    #endregion

    #region Delete

    /// <summary>
    /// Exibe a página de confirmação para excluir um produto.
    /// </summary>
    /// <param name="id">O ID do produto a ser excluído.</param>
    /// <returns>A view de confirmação com os dados do produto, ou NotFound se o produto não existir.</returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        TempData["ErrorMessage"] = null;
        try
        {
            // Chama o serviço para obter o produto pelo ID.
            var produtoDto = await _produtoService.ObterPorIdAsync(id);
            if (produtoDto == null)
            {
                return NotFound();
            }
            // Mapeia o DTO para o ViewModel utilizando o mapper.
            var produtoViewModel = ProdutoMapper.ToViewModel(produtoDto);
            return View(produtoViewModel);
        }
        catch (KeyNotFoundException)
        {
            // Se o produto não for encontrado, armazena a mensagem no TempData e redireciona para o índice.
            TempData["ErrorMessage"] = "Produto não encontrado.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Se ocorrer um erro inesperado, armazena a mensagem no TempData e redireciona para o índice.
            TempData["ErrorMessage"] = "Ocorreu um erro inesperado ao carregar a página de exclusão: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Processa a exclusão de um produto.
    /// </summary>
    /// <param name="id">O ID do produto a ser excluído.</param>
    /// <returns>Redireciona para a página de índice após a exclusão.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            // Verifica se o produto existe antes de tentar excluir.
            var produtoExistente = await _produtoService.ObterPorIdAsync(id);
            if (produtoExistente == null)
            {
                return NotFound();
            }

            // Obtém o ID do vendedor logado.
            var vendedorIdLogado = ObterVendedorIdDoUsuarioLogado();
            // Chama o serviço para excluir o produto.
            await _produtoService.ExcluirAsync(id, vendedorIdLogado);
            // Em caso de sucesso, armazena uma mensagem no TempData e redireciona para o índice.
            TempData["SuccessMessage"] = "Produto excluído com sucesso.";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            // Se o produto não for encontrado, armazena a mensagem no TempData.
            TempData["ErrorMessage"] = "Produto não encontrado.";
        }
        catch (UnauthorizedAccessException ex)
        {
            // Se o acesso não for autorizado, armazena a mensagem no TempData.
            TempData["ErrorMessage"] = ex.Message;
        }
        catch (Exception ex)
        {
            // Se ocorrer um erro inesperado, armazena a mensagem no TempData.
            TempData["ErrorMessage"] = "Ocorreu um erro inesperado ao excluir o produto: " + ex.Message;
        }
        // Redireciona para a página de índice.
        return RedirectToAction(nameof(Index));
    }

    #endregion

    /// <summary>
    /// Obtém o ID do vendedor (usuário logado) a partir das Claims do usuário.
    /// </summary>
    /// <returns>O ID do vendedor logado como Guid, ou Guid.Empty se não encontrado ou inválido.</returns>
    private Guid ObterVendedorIdDoUsuarioLogado()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(userIdClaim, out Guid userId))
        {
            return userId;
        }
        return Guid.Empty;
    }
}