using DevXpertHub.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DevXpertHub.Infrastructure;

/// <summary>
/// Classe de contexto do Entity Framework Core para a aplicação DevXpertHub.
/// Herda de <see cref="IdentityDbContext"/> para suportar a autenticação e autorização do ASP.NET Core Identity.
/// Responsável por interagir com o banco de dados, mapeando as entidades de domínio para as tabelas correspondentes.
/// </summary>
/// <param name="options">As opções de configuração do contexto, fornecidas durante a inicialização.</param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
{
    /// <summary>
    /// Representa a tabela de Categorias no banco de dados.
    /// </summary>
    public DbSet<Categoria> Categorias { get; set; }

    /// <summary>
    /// Representa a tabela de Produtos no banco de dados.
    /// </summary>
    public DbSet<Produto> Produtos { get; set; }

    /// <summary>
    /// Método chamado durante a criação do modelo do banco de dados pelo Entity Framework Core.
    /// Permite configurar as entidades, seus relacionamentos e outras opções de mapeamento.
    /// </summary>
    /// <param name="modelBuilder">O construtor usado para definir o modelo do banco de dados.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Chama a implementação base para configurar as entidades do ASP.NET Core Identity.
        base.OnModelCreating(modelBuilder);

        // Aplica as configurações de modelo definidas em classes que implementam
        // IEntityTypeConfiguration<TEntity> neste assembly. Isso permite manter
        // as configurações de mapeamento separadas das classes de entidade e do contexto.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Você pode adicionar configurações de modelo adicionais aqui, se necessário,
        // para entidades que não possuem uma classe de configuração separada
        // ou para configurações globais do modelo.
    }
}