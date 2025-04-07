using DevXpertHub.Core.Dtos;
using DevXpertHub.Core.Interfaces;
using DevXpertHub.Core.Mappers;
using DevXpertHub.Domain.Entities;
using DevXpertHub.Domain.Interfaces;

namespace DevXpertHub.Services;

/// <summary>
/// Implementação do serviço para a entidade <see cref="Categoria"/>.
/// Fornece a lógica de negócios para operações relacionadas a categorias,
/// utilizando o repositório <see cref="ICategoriaRepository"/> e os mappers <see cref="CategoriaMapper"/>.
/// </summary>
public class CategoriaService(ICategoriaRepository categoriaRepository) : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;

    /// <summary>
    /// Adiciona uma nova categoria de forma assíncrona.
    /// Realiza a validação para garantir que não exista outra categoria com o mesmo nome antes de adicionar.
    /// </summary>
    /// <param name="categoriaDto">O DTO <see cref="CategoriaApplicationModel"/> contendo os dados da nova categoria.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO <see cref="CategoriaApplicationModel"/> da categoria adicionada.</returns>
    /// <exception cref="ArgumentException">Ocorre se já existir uma categoria com o mesmo nome.</exception>
    public async Task<CategoriaApplicationModel> AdicionarAsync(CategoriaApplicationModel categoriaDto)
    {
        // Verificar se já existe uma categoria com o mesmo nome (ignorando o ID se estiver presente, para edições)
        if (await _categoriaRepository.ExisteCategoriaComNomeAsync(categoriaDto.Nome, categoriaDto.Id))
        {
            throw new ArgumentException($"Já existe uma categoria com o nome '{categoriaDto.Nome}'.");
        }
        // Mapear o DTO para a entidade de domínio
        var categoriaEntity = CategoriaMapper.ParaDominio(categoriaDto);
        // Adicionar a entidade ao repositório
        var categoriaEntityAdicionada = await _categoriaRepository.AdicionarAsync(categoriaEntity);
        // Mapear a entidade de domínio adicionada de volta para um DTO e retornar
        return CategoriaMapper.ParaAplicacao(categoriaEntityAdicionada);
    }

    /// <summary>
    /// Obtém todas as categorias de forma assíncrona.
    /// </summary>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém uma lista de DTOs <see cref="CategoriaApplicationModel"/> representando todas as categorias.</returns>
    public async Task<List<CategoriaApplicationModel>> ObterTodasAsync()
    {
        // Obter todas as categorias do repositório
        var categorias = await _categoriaRepository.ObterTodasAsync();
        // Mapear a lista de entidades de domínio para uma lista de DTOs e retornar
        return categorias
            .Where(c => c != null) // Filtrar possíveis valores nulos (embora improvável com EF)
            .Select(CategoriaMapper.ParaAplicacao)
            .ToList();
    }

    /// <summary>
    /// Obtém uma categoria pelo seu ID de forma assíncrona.
    /// </summary>
    /// <param name="id">O ID da categoria a ser obtida.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO <see cref="CategoriaApplicationModel"/> da categoria encontrada.</returns>
    /// <exception cref="KeyNotFoundException">Ocorre se nenhuma categoria for encontrada com o ID especificado.</exception>
    public async Task<CategoriaApplicationModel> ObterPorIdAsync(int id)
    {
        // Obter a categoria do repositório pelo ID
        var categoria = await _categoriaRepository.ObterPorIdAsync(id);
        // Verificar se a categoria foi encontrada
        if (categoria == null)
        {
            throw new KeyNotFoundException($"Categoria com Id {id} não encontrada.");
        }
        // Mapear a entidade de domínio para um DTO e retornar
        return CategoriaMapper.ParaAplicacao(categoria);
    }

    /// <summary>
    /// Atualiza uma categoria existente de forma assíncrona.
    /// Realiza a validação para garantir que a categoria exista e que não haja outra categoria com o mesmo nome (ignorando a própria categoria).
    /// </summary>
    /// <param name="categoriaDto">O DTO <see cref="CategoriaApplicationModel"/> contendo os dados atualizados da categoria.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O resultado da tarefa
    /// contém o DTO <see cref="CategoriaApplicationModel"/> da categoria atualizada.</returns>
    /// <exception cref="KeyNotFoundException">Ocorre se nenhuma categoria for encontrada com o ID especificado.</exception>
    /// <exception cref="InvalidOperationException">Ocorre se já existir outra categoria com o mesmo nome.</exception>
    public async Task<CategoriaApplicationModel> AtualizarAsync(CategoriaApplicationModel categoriaDto)
    {
        // Verificar se a categoria existe antes de atualizar
        var categoriaExistente = await _categoriaRepository.ObterPorIdAsync(categoriaDto.Id)
                                       ?? throw new KeyNotFoundException($"Categoria com Id {categoriaDto.Id} não encontrada.");

        // Verificar se a categoria já existe com o novo nome (ignorando a categoria sendo atualizada)
        if (await _categoriaRepository.ExisteCategoriaComNomeAsync(categoriaDto.Nome, categoriaDto.Id))
        {
            throw new InvalidOperationException("Já existe uma categoria com este nome.");
        }

        // Mapear o DTO para a entidade de domínio para atualização
        var categoriaParaAtualizacao = CategoriaMapper.ParaDominio(categoriaDto);

        // Atualizar a categoria no repositório
        var categoriaAtualizada = await _categoriaRepository.AtualizarAsync(categoriaParaAtualizacao);
        // Mapear a entidade de domínio atualizada de volta para um DTO e retornar
        return CategoriaMapper.ParaAplicacao(categoriaAtualizada);
    }

    /// <summary>
    /// Exclui uma categoria pelo seu ID de forma assíncrona.
    /// Verifica se a categoria existe antes de tentar excluir.
    /// </summary>
    /// <param name="id">O ID da categoria a ser excluída.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    /// <exception cref="KeyNotFoundException">Ocorre se nenhuma categoria for encontrada com o ID especificado.</exception>
    /// <exception cref="InvalidOperationException">Ocorre se a categoria possuir produtos associados.</exception>
    public async Task ExcluirAsync(int id)
    {
        // Verificar se a categoria existe antes de excluir
        var categoriaExistente = await _categoriaRepository.ObterPorIdAsync(id);
        if (categoriaExistente == null)
        {
            throw new KeyNotFoundException($"Categoria com Id {id} não encontrada.");
        }

        // Verificar se a categoria possui produtos associados
        if (await _categoriaRepository.CategoriaPossuiProdutosAssociadosAsync(id))
        {
            throw new InvalidOperationException("Não é possível excluir a categoria, pois ela possui produtos associados.");
        }

        // Excluir a categoria do repositório
        await _categoriaRepository.ExcluirAsync(id);
    }

    /// <summary>
    /// Verifica de forma assíncrona se já existe uma categoria com o nome fornecido.
    /// </summary>
    /// <param name="categoriaModel">O DTO <see cref="CategoriaApplicationModel"/> contendo o nome a ser verificado.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    private async Task VerificarSeCategoriaJaExisteAsync(CategoriaApplicationModel categoriaModel)
    {
        if (await _categoriaRepository.ExisteCategoriaComNomeAsync(categoriaModel.Nome, categoriaModel.Id))
        {
            throw new InvalidOperationException("Já existe uma categoria com este nome.");
        }
    }
}