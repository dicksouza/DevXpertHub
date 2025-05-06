using DevXpertHub.Core.Dtos.Categorias;
using DevXpertHub.Core.Interfaces.Services;
using DevXpertHub.Web.Mappers;
using DevXpertHub.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DevXpertHub.Web.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações relacionadas a categorias no sistema web.
/// </summary>
[Authorize]
public class CategoriasController(ICategoriaService categoriaService) : Controller
{
    private readonly ICategoriaService _categoriaService = categoriaService;

    // Executado antes de cada ação do controlador.
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //TempData["ErrorMessage"] = null;
        //TempData["SuccessMessage"] = null;
        base.OnActionExecuting(context);
    }

    #region Create

    /// <summary>
    /// Exibe o formulário para criar uma nova categoria.
    /// </summary>
    /// <returns>A view com o formulário de criação.</returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Processa a submissão do formulário de criação de uma nova categoria.
    /// </summary>
    /// <param name="novaCategoria">O modelo de dados da categoria a ser criada.</param>
    /// <returns>Redireciona para a página de índice em caso de sucesso, ou retorna o formulário com erros de validação.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(CategoriaCreateViewModel novaCategoria)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var novaCategoriaDto = new CategoriaCreateDto(
                    novaCategoria.Nome,
                    novaCategoria.Descricao
                );
                await _categoriaService.AdicionarAsync(novaCategoriaDto);
                TempData["SuccessMessage"] = "Categoria cadastrada com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao criar a categoria: " + ex.Message);
            }
        }
        return View(novaCategoria);
    }

    #endregion

    #region Read

    /// <summary>
    /// Exibe a lista de todas as categorias.
    /// </summary>
    /// <returns>A view com a lista de categorias.</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        try
        {
            if (TempData.Peek("ErrorMessage") != null) TempData.Keep();
            var categoriasDto = await _categoriaService.ObterTodasAsync();
            var categoriasViewModel = categoriasDto.Select(dto => new CategoriaReadViewModel
            (
                dto.Id,
                dto.Nome,
                dto.Descricao
            )).ToList();
            return View(categoriasViewModel);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Ocorreu um erro ao carregar as categorias: " + ex.Message;
            return View(new List<CategoriaReadViewModel>());
        }
    }

    #endregion

    #region Update

    /// <summary>
    /// Exibe o formulário para editar uma categoria existente.
    /// </summary>
    /// <param name="id">O ID da categoria a ser editada.</param>
    /// <returns>A view com o formulário de edição preenchido com os dados da categoria, ou NotFound se a categoria não existir.</returns>
    [HttpGet]
    public async Task<IActionResult> EditAsync(string id)
    {
        try
        {
            var categoriaDto = await _categoriaService.ObterPorIdAsync(id);
            return categoriaDto == null ? NotFound() : View(categoriaDto.MapToUpdateViewModel());
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Categoria não encontrada.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Ocorreu um erro inesperado ao carregar a categoria: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Processa a submissão do formulário de edição de uma categoria existente.
    /// </summary>
    /// <param name="id">O ID da categoria a ser editada.</param>
    /// <param name="categoriaAtualizada">O modelo de dados da categoria com as informações atualizadas.</param>
    /// <returns>Redireciona para a página de índice em caso de sucesso, ou retorna o formulário com erros de validação.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(string id, CategoriaUpdateViewModel categoriaAtualizada)
    {
        if (id != categoriaAtualizada.Id)
        {
            TempData["ErrorMessage"] = "O ID da categoria não corresponde ao esperado.";
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _categoriaService.AtualizarAsync(categoriaAtualizada.MapToUpdateDto());
                TempData["SuccessMessage"] = "Categoria atualizada com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (KeyNotFoundException)
            {
                ModelState.AddModelError(string.Empty, "Categoria não encontrada.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao editar a categoria: " + ex.Message);
            }
        }
        return View(categoriaAtualizada);
    }

    #endregion

    #region Delete

    /// <summary>
    /// Exibe a página de confirmação para excluir uma categoria.
    /// </summary>
    /// <param name="id">O ID da categoria a ser excluída.</param>
    /// <returns>A view de confirmação com os dados da categoria, ou NotFound se a categoria não existir.</returns>
    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var categoriaDto = await _categoriaService.ObterPorIdAsync(id);
            return categoriaDto == null ? NotFound() : View(categoriaDto.MapToReadViewModel());
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Categoria não encontrada.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Ocorreu um erro inesperado ao carregar a página de exclusão: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Processa a exclusão de uma categoria.
    /// </summary>
    /// <param name="id">O ID da categoria a ser excluída.</param>
    /// <returns>Redireciona para a página de índice após a exclusão.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            await _categoriaService.ExcluirAsync(id);
            TempData["SuccessMessage"] = "Categoria excluída com sucesso.";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            ModelState.AddModelError(string.Empty, "Categoria não encontrada.");
            var categoriaDto = await _categoriaService.ObterPorIdAsync(id);
            return View(categoriaDto?.MapToReadViewModel());
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var categoriaDto = await _categoriaService.ObterPorIdAsync(id);
            return View(categoriaDto?.MapToReadViewModel());
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao excluir a categoria: " + ex.Message);
            var categoriaDto = await _categoriaService.ObterPorIdAsync(id);
            return View(categoriaDto?.MapToReadViewModel());
        }
    }

    #endregion
}