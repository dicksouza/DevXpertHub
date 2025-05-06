using DevXpertHub.Core.Interfaces.Services;
using DevXpertHub.Web.Mappers;
using DevXpertHub.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DevXpertHub.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICategoriaService _categoriaService;
    private readonly IProdutoService _produtoService;

    public HomeController(ILogger<HomeController> logger, ICategoriaService categoriaService, IProdutoService produtoService)
    {
        _logger = logger;
        _categoriaService = categoriaService;
        _produtoService = produtoService;
    }

    public async Task<IActionResult> IndexAsync(string? categoriaId)
    {
        // Obtém as categorias com produtos associados
        var categorias = await _categoriaService.ObterCategoriasComProdutosAsync();
        ViewBag.Categorias = categorias;

        // Define a categoria selecionada no ViewBag para manter o estado do dropdown
        ViewBag.CategoriaSelecionada = categoriaId;

        // Filtra os produtos com base na categoria selecionada
        var produtos = !string.IsNullOrEmpty(categoriaId)
            ? await _produtoService.ObterProdutosPorCategoriaAsync(categoriaId)
            : await _produtoService.ObterTodosAsync();

        // Mapeia a lista de DTOs para uma lista de ViewModels utilizando o mapper
        var produtosViewModel = produtos.Select(ProdutoMapper.MapToViewModel).ToList();

        return View(produtosViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}