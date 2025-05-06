using DevXpertHub.Core.Entities;
using DevXpertHub.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevXpertHub.Infrastructure.Repositories;

public class FornecedorRepository : IFornecedorRepository
{
    private readonly AppDbContext _context;

    public FornecedorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Fornecedor?> ObterPorIdAsync(string id)
    {
        return await _context.Fornecedores.FindAsync(id);
    }

    public async Task<List<Fornecedor>> ObterTodosAsync()
    {
        return await _context.Fornecedores.ToListAsync();
    }

    public async Task<Fornecedor> AdicionarAsync(Fornecedor fornecedor)
    {
        await _context.Fornecedores.AddAsync(fornecedor);
        await _context.SaveChangesAsync();
        return fornecedor;
    }

    public async Task<Fornecedor> AtualizarAsync(Fornecedor fornecedor)
    {
        _context.Fornecedores.Update(fornecedor);
        await _context.SaveChangesAsync();
        return fornecedor;
    }

    public async Task RemoverAsync(string id)
    {
        var fornecedor = await _context.Fornecedores.FindAsync(id);
        if (fornecedor != null)
        {
            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();
        }
        await _context.SaveChangesAsync();
    }
}