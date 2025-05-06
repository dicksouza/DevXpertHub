using DevXpertHub.Core.Dtos.Categorias;
using DevXpertHub.Web.Models;

namespace DevXpertHub.Web.Mappers;

/// <summary>
/// Classe utilitária para mapear entre ViewModels e DTOs.
/// </summary>
public static class CategoriaMapper
{
    /// <summary>
    /// Mapeia um CategoriaUpdateViewModel para um CategoriaUpdateDto.
    /// </summary>
    /// <param name="viewModel">O ViewModel contendo os dados da categoria.</param>
    /// <returns>O DTO correspondente com os dados mapeados.</returns>
    public static CategoriaUpdateDto MapToUpdateDto(this CategoriaUpdateViewModel viewModel)
    {
        return new CategoriaUpdateDto
        (
            viewModel.Id,
            viewModel.Nome,
            viewModel.Descricao
        );
    }

    /// <summary>
    /// Mapeia um CategoriaDto para um CategoriaUpdateViewModel.
    /// </summary>
    /// <param name="viewModel">O DTO contendo os dados da categoria.</param>
    /// <returns>O ViewModel correspondente com os dados mapeados.</returns>
    public static CategoriaUpdateViewModel MapToUpdateViewModel(this CategoriaDto viewModel)
    {
        return new CategoriaUpdateViewModel
        (
            viewModel.Id,
            viewModel.Nome,
            viewModel.Descricao
        );
    }

    /// <summary>
    /// Mapeia um CategoriaDto para um CategoriaReadViewModel.
    /// </summary>
    /// <param name="viewModel">O DTO contendo os dados da categoria.</param>
    /// <returns>O ViewModel correspondente com os dados mapeados.</returns>
    public static CategoriaReadViewModel MapToReadViewModel(this CategoriaDto viewModel)
    {
        return new CategoriaReadViewModel
        (
            viewModel.Id,
            viewModel.Nome,
            viewModel.Descricao
        );
    }
}