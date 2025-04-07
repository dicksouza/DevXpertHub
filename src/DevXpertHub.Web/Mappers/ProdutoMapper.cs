using DevXpertHub.Core.Dtos;
using DevXpertHub.Web.Models;

namespace DevXpertHub.Web.Mappers;

/// <summary>
/// Classe est�tica respons�vel por realizar o mapeamento entre os modelos de visualiza��o (ViewModels)
/// e os modelos de transfer�ncia de dados da aplica��o (Application Models/DTOs) para a entidade Produto.
/// Essa classe facilita a convers�o de dados entre as camadas da aplica��o.
/// </summary>
public static class ProdutoMapper
{
    /// <summary>
    /// Converte um objeto ProdutoViewModel para um objeto ProdutoApplicationModel.
    /// Utilizado para transferir dados da camada de apresenta��o para a camada de servi�o.
    /// </summary>
    /// <param name="viewModel">O modelo de visualiza��o do produto.</param>
    /// <returns>O modelo de aplica��o do produto.</returns>
    public static ProdutoApplicationModel ToApplicationModel(ProdutoViewModel viewModel)
    {
        return new ProdutoApplicationModel
        {
            Id = viewModel.Id,
            Nome = viewModel.Nome,
            Descricao = viewModel.Descricao,
            Preco = viewModel.Preco,
            Estoque = viewModel.Estoque,
            CategoriaId = viewModel.CategoriaId,
            // Mapeia a CategoriaViewModel para CategoriaApplicationModel, verificando se n�o � nulo.
            Categoria = viewModel.Categoria != null ? new CategoriaApplicationModel(
                viewModel.Categoria.Id,
                viewModel.Categoria.Nome,
                viewModel.Categoria.Descricao
            ) : null,
            Imagem = viewModel.Imagem
        };
    }

    /// <summary>
    /// Converte um objeto ProdutoApplicationModel para um objeto ProdutoViewModel.
    /// Utilizado para transferir dados da camada de servi�o para a camada de apresenta��o.
    /// </summary>
    /// <param name="applicationModel">O modelo de aplica��o do produto.</param>
    /// <returns>O modelo de visualiza��o do produto.</returns>
    public static ProdutoViewModel ToViewModel(ProdutoApplicationModel applicationModel)
    {
        return new ProdutoViewModel
        {
            Id = applicationModel.Id,
            Nome = applicationModel.Nome,
            Descricao = applicationModel.Descricao,
            Preco = applicationModel.Preco,
            Estoque = applicationModel.Estoque,
            CategoriaId = applicationModel.CategoriaId,
            // Mapeia a CategoriaApplicationModel para CategoriaViewModel, verificando se n�o � nulo.
            Categoria = applicationModel.Categoria != null ? new CategoriaViewModel
            {
                Id = applicationModel.Categoria.Id,
                Nome = applicationModel.Categoria.Nome,
                Descricao = applicationModel.Categoria.Descricao
            } : null,
            Imagem = applicationModel.Imagem
        };
    }
}