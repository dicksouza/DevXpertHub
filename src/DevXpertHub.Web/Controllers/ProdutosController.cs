using DevXpertHub.Core.Dtos.Categorias;
using DevXpertHub.Core.Dtos.Fornecedores;
using DevXpertHub.Core.Dtos.Produtos;
using DevXpertHub.Core.Entities;
using DevXpertHub.Core.Interfaces.Services;
using DevXpertHub.Core.Services;
using DevXpertHub.Web.Mappers;
using DevXpertHub.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        base.OnActionExecuting(context);
    }

    #region Create
    /// <summary>
    /// Exibe o formulário para criar um novo produto.
    /// Preenche automaticamente o FornecedorId com o ID do usuário logado
    /// e carrega a lista de categorias disponíveis.
    /// </summary>
    /// <returns>A view com o formulário de criação.</returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            // Obtém o ID do fornecedor logado.
            var fornecedorIdLogado = ObterFornecedorIdDoUsuarioLogado();

            // Obtém a lista de categorias disponíveis.
            var categorias = await _produtoService.ObterCategoriasAsync();

            // Cria o ViewModel usando o construtor parametrizado.
            var viewModel = new ProdutoCreateViewModel(
            fornecedorIdLogado,
            categorias.Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.Nome
            })
             );

            return View(viewModel);
        }
        catch (Exception ex)
        {
            // Em caso de erro, armazena a mensagem no TempData para exibição na view.
            TempData["ErrorMessage"] = "Ocorreu um erro ao carregar o formulário de criação: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Processa a submissão do formulário de criação de um novo produto.
    /// </summary>
    /// <param name="produtoViewModel">O modelo de dados do produto a ser criado.</param>
    /// <returns>Redireciona para a página de índice em caso de sucesso, ou retorna o formulário com erros de validação.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(ProdutoCreateViewModel produtoViewModel)
    {
        // Verifica se o arquivo foi enviado
        if (produtoViewModel.Imagem == null || produtoViewModel.Imagem.Length == 0)
        {
            // Adiciona um erro ao ModelState caso o campo Imagem não tenha sido preenchido
            ModelState.AddModelError("Imagem", "O campo Imagem Principal é obrigatório.");
        }

        // Verifica se o modelo recebido é válido de acordo com as Data Annotations e validações adicionais
        if (ModelState.IsValid)
        {
            using var transaction = await _produtoService.BeginTransactionAsync();
            try
            {
                // Obtém o ID do fornecedor logado
                var fornecedorIdLogado = ObterFornecedorIdDoUsuarioLogado();

                // Mapeia o ViewModel para o DTO utilizando o mapper
                var produtoDto = ProdutoMapper.MapToCreateDto(produtoViewModel);

                // Chama o serviço para adicionar o novo produto e obtém o produto criado com o ID gerado
                var produtoCriado = await _produtoService.AdicionarAsync(produtoDto, fornecedorIdLogado);

                // Diretório para armazenar as imagens (baseado no FornecedorId e ProdutoId)
                var fornecedorFolder = Path.Combine("wwwroot/uploads", fornecedorIdLogado);
                var produtoFolder = Path.Combine(fornecedorFolder, produtoCriado.Id.ToString());

                // Garante que o diretório existe
                Directory.CreateDirectory(produtoFolder);

                // Processa o upload da imagem
                var filePath = Path.Combine(produtoFolder, produtoViewModel.Imagem!.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await produtoViewModel.Imagem.CopyToAsync(stream);

                // Commit da transação
                await transaction.CommitAsync();

                // Armazena uma mensagem de sucesso no TempData para exibição na view
                TempData["SuccessMessage"] = "Produto criado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Rollback em caso de erro
                await transaction.RollbackAsync();

                // Adiciona uma mensagem genérica ao ModelState em caso de erro
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar o produto: " + ex.Message);
            }
        }

        // Repopula as categorias ao retornar a view com erros
        produtoViewModel.Categorias = await _produtoService.ObterCategoriasAsync()
            .ContinueWith(task => task.Result.Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.Nome
            }));

        // Retorna a view com o ViewModel atualizado para exibir os erros
        return View(produtoViewModel);
    }

    #endregion

    #region Read

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
            List<ProdutoDto> produtos;

            // Verifica se o usuário está logado.
            if (User.Identity?.IsAuthenticated == true)
            {
                // Obtém o ID do fornecedor logado.
                var fornecedorIdLogado = ObterFornecedorIdDoUsuarioLogado();
                // Chama o serviço para obter todos os produtos.
                produtos = await _produtoService.ObterTodosPorFornecedorAsync(fornecedorIdLogado);
            }
            else
            {
                // Chama o serviço para obter todos os produtos.
                produtos = await _produtoService.ObterTodosAsync();
            }

            // Mapeia a lista de DTOs para uma lista de ViewModels utilizando o mapper.
            produtosViewModel = produtos.Select(ProdutoMapper.MapToViewModel).ToList();

            // Verifica se existem categorias cadastradas
            var existemCategorias = (await _produtoService.ObterCategoriasAsync()).Any();
            ViewBag.ExistemCategorias = existemCategorias;

            return View(produtosViewModel);
        }
        catch (Exception ex)
        {
            // Em caso de erro, armazena a mensagem no TempData para exibição na view.
            TempData["ErrorMessage"] = "Ocorreu um erro ao carregar os produtos: " + ex.Message;
            return View(new List<ProdutoViewModel>());
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(string produtoId)
    {
        if (produtoId != null)
        {
            var produto = await _produtoService.ObterPorIdAsync(produtoId);

            return produto != null ? View(produto.MapToViewModel()) : NotFound();
        }

        return NotFound();
    }


    [HttpGet]
    public async Task<IActionResult> ObterProdutosPorCategoria([FromQuery] string categoriaId)
    {
        var produtos = await _produtoService.ObterProdutosPorCategoriaAsync(categoriaId);
        return Ok(produtos);
    }

    [HttpGet]
    public async Task<IActionResult> ObterProdutosPorFornecedor([FromQuery] string fornecedorId)
    {
        var produtos = await _produtoService.ObterTodosPorFornecedorAsync(fornecedorId);
        return Ok(produtos);
    }

    [HttpGet]
    public async Task<IActionResult> ObterProdutoPorId([FromQuery] string produtoId)
    {
        var produto = await _produtoService.ObterPorIdAsync(produtoId);
        return Ok(produto);
    }

    [HttpGet]
    public async Task<IActionResult> ObterProdutoPorNome([FromQuery] string nome)
    {
        var produtos = await _produtoService.ObterTodosAsync();
        var produto = produtos.FirstOrDefault(p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));
        return Ok(produto);
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var produtos = await _produtoService.ObterTodosAsync();
        return Ok(produtos);
    }

    #endregion

    #region Edit

    /// <summary>
    /// Exibe o formulário para editar um produto existente.
    /// </summary>
    /// <param name="id">O ID do produto a ser editado.</param>
    /// <returns>A view com o formulário de edição preenchido com os dados do produto, ou NotFound    [HttpPost]
    [HttpGet]
    public async Task<IActionResult> EditAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            TempData["ErrorMessage"] = "O ID do produto é inválido.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            // Busca o produto pelo ID
            var produtoDto = await _produtoService.ObterPorIdAsync(id);
            if (produtoDto == null)
            {
                TempData["ErrorMessage"] = "Produto não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            // Obter o nome do fornecedor (assumindo que o FornecedorId está presente no produtoDto)
            var fornecedorNome = produtoDto.Fornecedor?.Nome ?? "Fornecedor não encontrado";
            ViewBag.FornecedorNome = fornecedorNome;

            // Mapeia o DTO para o ViewModel
            var produtoUpdateViewModel = new ProdutoUpdateViewModel(
                produtoDto.Id,
                produtoDto.Nome,
                produtoDto.Descricao,
                produtoDto.ImagemPrincipal,
                produtoDto.Preco,
                produtoDto.Estoque,
                produtoDto.CategoriaId,
                produtoDto.FornecedorId,
                (await _produtoService.ObterCategoriasAsync()).Select(c => new SelectListItem
                {
                    Value = c.Id,
                    Text = c.Nome
                })
            );

            return View(produtoUpdateViewModel);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Ocorreu um erro ao carregar o produto: " + ex.Message;
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
    public async Task<IActionResult> EditAsync(string id, ProdutoUpdateViewModel produtoUpdateViewModel)
    {
        if (id != produtoUpdateViewModel.Id)
        {
            TempData["ErrorMessage"] = "O ID do produto não corresponde ao esperado.";
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            // Repopula as categorias em caso de erro de validação
            produtoUpdateViewModel.Categorias = (await _produtoService.ObterCategoriasAsync()).Select(c => new SelectListItem
            {
                Value = c.Id,
                Text = c.Nome
            });
            return View(produtoUpdateViewModel);
        }

        try
        {
            // Verifica se o produto existe
            var produtoExistente = await _produtoService.ObterPorIdAsync(id);
            if (produtoExistente == null)
            {
                TempData["ErrorMessage"] = "Produto não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            // Atualiza os dados do produto
            var produtoDto = new ProdutoDto(
                produtoUpdateViewModel.Id,
                produtoUpdateViewModel.Nome,
                produtoUpdateViewModel.Descricao,
                produtoUpdateViewModel.Preco,
                produtoUpdateViewModel.Estoque,
                produtoUpdateViewModel.CategoriaId,
                produtoExistente.Categoria,
                produtoUpdateViewModel.FornecedorId,
                produtoExistente.Fornecedor,
                produtoUpdateViewModel.ImagemAtual
            );

            // Processa o upload da nova imagem, se necessário
            if (produtoUpdateViewModel.ImagemNova != null)
            {
                var fornecedorFolder = Path.Combine("uploads", produtoUpdateViewModel.FornecedorId);
                var produtoFolder = Path.Combine(fornecedorFolder, id);
                Directory.CreateDirectory(produtoFolder);

                var filePath = Path.Combine("wwwroot/", produtoFolder, produtoUpdateViewModel.ImagemNova.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await produtoUpdateViewModel.ImagemNova.CopyToAsync(stream);

                // Atualiza o caminho da imagem no DTO
                produtoDto = produtoDto with { ImagemPrincipal = Path.Combine(produtoFolder, produtoUpdateViewModel.ImagemNova.FileName) };
            }

            // Atualiza o produto no banco de dados
            var fornecedorIdLogado = ObterFornecedorIdDoUsuarioLogado();
            await _produtoService.AtualizarAsync(produtoDto, fornecedorIdLogado);

            TempData["SuccessMessage"] = "Produto atualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao atualizar o produto: " + ex.Message);
        }

        // Repopula as categorias em caso de erro
        produtoUpdateViewModel.Categorias = (await _produtoService.ObterCategoriasAsync()).Select(c => new SelectListItem
        {
            Value = c.Id,
            Text = c.Nome
        });

        return View(produtoUpdateViewModel);
    }

    #endregion

    #region Delete

    /// <summary>
    /// Exibe a página de confirmação para excluir um produto.
    /// </summary>
    /// <param name="id">O ID do produto a ser excluído.</param>
    /// <returns>A view de confirmação com os dados do produto, ou NotFound se o produto não existir.</returns>
    [HttpGet]
    public async Task<IActionResult> Delete(string id)
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
            var produtoViewModel = ProdutoMapper.MapToViewModel(produtoDto);
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
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            // Verifica se o produto existe antes de tentar excluir.
            var produtoExistente = await _produtoService.ObterPorIdAsync(id);
            if (produtoExistente == null)
            {
                return NotFound();
            }

            // Obtém o ID do fornecedor logado.
            var fornecedorIdLogado = ObterFornecedorIdDoUsuarioLogado();
            // Chama o serviço para excluir o produto.
            await _produtoService.ExcluirAsync(id, fornecedorIdLogado);
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
    /// Obtém o ID do fornecedor (usuário logado) a partir das Claims do usuário.
    /// </summary>
    /// <returns>O ID do fornecedor logado como Guid, ou Guid.Empty se não encontrado ou inválido.</returns>
    private string ObterFornecedorIdDoUsuarioLogado()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(userIdClaim, out Guid userId))
        {
            return userId.ToString();
        }
        throw new UnauthorizedAccessException("Usuário não autenticado ou ID inválido.");
    }
}