using Microsoft.EntityFrameworkCore.Storage;

namespace DevXpertHub.Core.Interfaces.Repositories;

/// <summary>
/// Interface para suporte a transações no repositório.
/// </summary>
public interface ITransactionSupport
{
    Task<IDbContextTransaction> BeginTransactionAsync();
}