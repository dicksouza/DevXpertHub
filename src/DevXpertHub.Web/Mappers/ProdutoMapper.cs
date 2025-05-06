using DevXpertHub.Core.Dtos.Produtos;
using DevXpertHub.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevXpertHub.Web.Mappers;

/// <summary>
/// Classe responsável por mapear entre ViewModels e DTOs relacionados a produtos.
/// </summary>
public static class ProdutoMapper
{
    /// <summary>
    /// Mapeia um ProdutoCreateViewModel para um ProdutoCreateDto.
    /// </summary>
    /// <param name="viewModel">O ViewModel contendo os dados do produto.</param>
    /// <returns>O DTO correspondente com os dados mapeados.</returns>
    public static ProdutoCreateDto MapToCreateDto(this ProdutoCreateViewModel viewModel)
    {
        return new ProdutoCreateDto(
            viewModel.Nome,
            viewModel.Descricao,
            viewModel.Preco,
            viewModel.Estoque,
            viewModel.CategoriaId,
            viewModel.FornecedorId,
            viewModel.Imagem.FileName
        );
    }

    /// <summary>
    /// Mapeia um ProdutoDto para um ProdutoViewModel.
    /// </summary>
    /// <param name="dto">O DTO contendo os dados do produto.</param>
    /// <returns>O ViewModel correspondente com os dados mapeados.</returns>
    public static ProdutoViewModel MapToViewModel(this ProdutoDto dto)
    {
        return new ProdutoViewModel
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Imagem = dto.ImagemPrincipal,
            Preco = dto.Preco,
            Estoque = dto.Estoque,
            CategoriaId = dto.CategoriaId,
            Categoria = dto.Categoria != null
                ? new CategoriaReadViewModel(dto.Categoria.Id, dto.Categoria.Nome, dto.Categoria.Descricao)
                : null,
            FornecedorId = dto.FornecedorId,
            Fornecedor = dto.Fornecedor != null
                ? new FornecedorReadViewModel(dto.Fornecedor.Id, dto.Fornecedor.Nome, dto.Fornecedor.Email)
                : null
        };
    }
    public static ProdutoUpdateViewModel MapToUpdateViewModel(this ProdutoDto produto, IEnumerable<SelectListItem> categorias)
    {
        return new ProdutoUpdateViewModel(
            produto.Id,
            produto.Nome,
            produto.Descricao,
            produto.ImagemPrincipal,
            produto.Preco,
            produto.Estoque,
            produto.CategoriaId,
            produto.FornecedorId,
            categorias
        );
    }
}