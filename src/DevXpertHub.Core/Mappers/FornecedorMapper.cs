using DevXpertHub.Core.Dtos.Fornecedores;
using DevXpertHub.Core.Entities;

namespace DevXpertHub.Core.Mappers;

public class FornecedorMapper
{
    public static FornecedorDto MapToDto(Fornecedor fornecedor)
    {
        return new FornecedorDto
        (
            fornecedor.Id,
           fornecedor.Nome,
            fornecedor.Email
        );
    }

    public static Fornecedor MapToDomain(FornecedorDto fornecedorDto)
    {
        return new Fornecedor
        (
            fornecedorDto.Id,
            fornecedorDto.Nome,
            fornecedorDto.Email,
            new List<Produto>() // Inicializa a coleção de produtos como uma lista vazia
        );
    }
}