using DevXpertHub.Core.Dtos;
using DevXpertHub.Web.Models;

namespace DevXpertHub.Web.Mappers;

/// <summary>
/// Classe estática responsável por realizar o mapeamento entre os modelos de visualização (ViewModels)
/// e os modelos de transferência de dados da aplicação (Application Models/DTOs) para a entidade Produto.
/// Essa classe facilita a conversão de dados entre as camadas da aplicação.
/// </summary>
public static class ProdutoMapper
{
    /// <summary>
    /// Converte um objeto ProdutoViewModel para um objeto ProdutoApplicationModel.
    /// Utilizado para transferir dados da camada de apresentação para a camada de serviço.
    /// </summary>
    /// <param name="viewModel">O modelo de visualização do produto.</param>
    /// <returns>O modelo de aplicação do produto.</returns>
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
            // Mapeia a CategoriaViewModel para CategoriaApplicationModel, verificando se não é nulo.
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
    /// Utilizado para transferir dados da camada de serviço para a camada de apresentação.
    /// </summary>
    /// <param name="applicationModel">O modelo de aplicação do produto.</param>
    /// <returns>O modelo de visualização do produto.</returns>
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
            // Mapeia a CategoriaApplicationModel para CategoriaViewModel, verificando se não é nulo.
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