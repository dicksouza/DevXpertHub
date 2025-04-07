using DevXpertHub.Core.Dtos;
using DevXpertHub.Core.Interfaces;
using DevXpertHub.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevXpertHub.Web.Controllers;

/// <summary>
/// Controller responsável por gerenciar as operações relacionadas a categorias no sistema web.
/// </summary>
public class CategoriasController(ICategoriaService categoriaService) : Controller
{
    private readonly ICategoriaService _categoriaService = categoriaService;

    #region Index

    /// <summary>
    /// Exibe a lista de todas as categorias.
    /// </summary>
    /// <returns>A view com a lista de categorias.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var categoriasDto = await _categoriaService.ObterTodasAsync();
            var categoriasViewModel = categoriasDto.Select(dto => new CategoriaViewModel
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Descricao = dto.Descricao
            }).ToList();
            return View(categoriasViewModel);
        }
        catch (Exception ex)
        {
            // Em caso de erro ao carregar as categorias, armazena a mensagem no TempData para exibição na view.
            TempData["ErrorMessage"] = "Ocorreu um erro ao carregar as categorias: " + ex.Message;
            return View(new List<CategoriaViewModel>());
        }
    }

    #endregion

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
    /// <param name="categoriaViewModel">O modelo de dados da categoria a ser criada.</param>
    /// <returns>Redireciona para a página de índice em caso de sucesso, ou retorna o formulário com erros de validação.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(CategoriaViewModel categoriaViewModel)
    {
        // Verifica se o modelo recebido é válido de acordo com as Data Annotations definidas no ViewModel.
        if (ModelState.IsValid)
        {
            try
            {
                // Mapeia o ViewModel para o DTO (Data Transfer Object) utilizado pela camada de serviço.
                var categoriaDto = new CategoriaApplicationModel(
                    categoriaViewModel.Id,
                    categoriaViewModel.Nome,
                    categoriaViewModel.Descricao
                );
                // Chama o serviço para adicionar a nova categoria.
                await _categoriaService.AdicionarAsync(categoriaDto);
                // Em caso de sucesso, redireciona para a página de índice para exibir a lista atualizada.
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                // Se ocorrer um erro de argumento (por exemplo, nome já existente), adiciona o erro ao ModelState para exibição no formulário.
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                // Se ocorrer um erro inesperado, adiciona uma mensagem genérica ao ModelState.
                ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao criar a categoria: " + ex.Message);
            }
        }
        // Se o ModelState não for válido, retorna a view com o ViewModel para exibir os erros de validação.
        return View(categoriaViewModel);
    }

    #endregion

    #region Edit

    /// <summary>
    /// Exibe o formulário para editar uma categoria existente.
    /// </summary>
    /// <param name="id">O ID da categoria a ser editada.</param>
    /// <returns>A view com o formulário de edição preenchido com os dados da categoria, ou NotFound se a categoria não existir.</returns>
    [HttpGet]
    public async Task<IActionResult> EditAsync(int id)
    {
        try
        {
            // Chama o serviço para obter a categoria pelo ID.
            var categoriaDto = await _categoriaService.ObterPorIdAsync(id);
            if (categoriaDto == null)
            {
                return NotFound();
            }
            // Mapeia o DTO para o ViewModel para exibição no formulário de edição.
            var viewModel = new CategoriaViewModel
            {
                Id = categoriaDto.Id,
                Nome = categoriaDto.Nome,
                Descricao = categoriaDto.Descricao
            };
            return View(viewModel);
        }
        catch (KeyNotFoundException)
        {
            // Se a categoria não for encontrada, armazena a mensagem no TempData e redireciona para o índice.
            TempData["ErrorMessage"] = "Categoria não encontrada.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Se ocorrer um erro inesperado, armazena a mensagem no TempData e redireciona para o índice.
            TempData["ErrorMessage"] = "Ocorreu um erro inesperado ao carregar a categoria: " + ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Processa a submissão do formulário de edição de uma categoria existente.
    /// </summary>
    /// <param name="id">O ID da categoria a ser editada.</param>
    /// <param name="categoriaViewModel">O modelo de dados da categoria com as informações atualizadas.</param>
    /// <returns>Redireciona para a página de índice em caso de sucesso, ou retorna o formulário com erros de validação.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(int id, CategoriaViewModel categoriaViewModel)
    {
        // Verifica se o ID da rota corresponde ao ID no ViewModel para evitar manipulação.
        if (id != categoriaViewModel.Id)
        {
            return BadRequest();
        }

        // Verifica se o modelo recebido é válido.
        if (ModelState.IsValid)
        {
            try
            {
                // Mapeia o ViewModel para o DTO.
                var categoriaDto = new CategoriaApplicationModel(
                    categoriaViewModel.Id,
                    categoriaViewModel.Nome,
                    categoriaViewModel.Descricao
                );
                // Chama o serviço para atualizar a categoria.
                await _categoriaService.AtualizarAsync(categoriaDto);
                // Em caso de sucesso, redireciona para o índice.
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                // Adiciona erros específicos ao ModelState.
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (KeyNotFoundException)
            {
                // Adiciona erro de categoria não encontrada ao ModelState.
                ModelState.AddModelError(string.Empty, "Categoria não encontrada.");
            }
            catch (Exception ex)
            {
                // Adiciona erro genérico ao ModelState.
                ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado ao editar a categoria: " + ex.Message);
            }
        }
        // Se o ModelState não for válido, retorna a view com o ViewModel para exibir os erros.
        return View(categoriaViewModel);
    }

    #endregion

    #region Delete

    /// <summary>
    /// Exibe a página de confirmação para excluir uma categoria.
    /// </summary>
    /// <param name="id">O ID da categoria a ser excluída.</param>
    /// <returns>A view de confirmação com os dados da categoria, ou NotFound se a categoria não existir.</returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            // Obtém a categoria pelo ID.
            var categoriaDto = await _categoriaService.ObterPorIdAsync(id);
            if (categoriaDto == null)
            {
                return NotFound();
            }
            // Mapeia para o ViewModel para exibição na página de confirmação.
            var viewModel = new CategoriaViewModel
            {
                Id = categoriaDto.Id,
                Nome = categoriaDto.Nome,
                Descricao = categoriaDto.Descricao
            };
            return View(viewModel);
        }
        catch (KeyNotFoundException)
        {
            // Se não encontrada, armazena mensagem e redireciona.
            TempData["ErrorMessage"] = "Categoria não encontrada.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Se ocorrer erro, armazena mensagem e redireciona.
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
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            // Chama o serviço para excluir a categoria pelo ID.
            await _categoriaService.ExcluirAsync(id);
            // Em caso de sucesso, armazena uma mensagem de sucesso no TempData.
            TempData["SuccessMessage"] = "Categoria excluída com sucesso.";
        }
        catch (KeyNotFoundException)
        {
            // Se não encontrada, armazena mensagem de erro.
            TempData["ErrorMessage"] = "Categoria não encontrada.";
        }
        catch (Exception ex)
        {
            // Se ocorrer erro, armazena mensagem de erro.
            TempData["ErrorMessage"] = "Ocorreu um erro inesperado ao excluir a categoria: " + ex.Message;
        }
        // Redireciona para a página de índice, exibindo qualquer mensagem armazenada no TempData.
        return RedirectToAction(nameof(Index));
    }

    #endregion
}