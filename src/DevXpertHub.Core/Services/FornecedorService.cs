using DevXpertHub.Core.Dtos.Fornecedores;
using DevXpertHub.Core.Entities;
using DevXpertHub.Core.Interfaces.Repositories;
using DevXpertHub.Core.Interfaces.Services;
using DevXpertHub.Core.Mappers;

namespace DevXpertHub.Core.Services;

public class FornecedorService : IFornecedorService
{
    private readonly IFornecedorRepository _fornecedorRepository;

    public FornecedorService(IFornecedorRepository fornecedorRepository)
    {
        _fornecedorRepository = fornecedorRepository;
    }

    public async Task<FornecedorDto?> ObterPorIdAsync(string id)
    {
        var fornecedor = await _fornecedorRepository.ObterPorIdAsync(id);
        if (fornecedor == null)
        {
            return null;
        }

        return FornecedorMapper.MapToDto(fornecedor);
    }

    public async Task<FornecedorDto> AdicionarAsync(FornecedorDto fornecedorDto)
    {
        var fornecedor = new Fornecedor
        (
            fornecedorDto.Id,
            fornecedorDto.Nome,
            fornecedorDto.Email,
            new List<Produto>()
        );

        var resultado = await _fornecedorRepository.AdicionarAsync(fornecedor);
        
        return FornecedorMapper.MapToDto(resultado);
    }

    public async Task<FornecedorDto> AtualizarAsync(FornecedorDto fornecedorDto)
    {
        var fornecedorExistente = await _fornecedorRepository.ObterPorIdAsync(fornecedorDto.Id)
            ?? throw new KeyNotFoundException($"Fornecedor com Id {fornecedorDto.Id} não encontrado.");

        var fornecedorAtualizado = new Fornecedor
            (
                fornecedorDto.Id,
                fornecedorDto.Nome,
                fornecedorDto.Email,
                fornecedorExistente.Produtos
            );
            
        var resultado = await _fornecedorRepository.AtualizarAsync(fornecedorAtualizado);

        return FornecedorMapper.MapToDto(resultado);

    }

    public async Task ExcluirAsync(string id)
    {
        await _fornecedorRepository.RemoverAsync(id);
    }
}