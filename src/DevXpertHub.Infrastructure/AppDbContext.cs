using DevXpertHub.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DevXpertHub.Infrastructure;

/// <summary>
/// Classe de contexto do Entity Framework Core para a aplicação DevXpertHub.
/// Herda de <see cref="IdentityDbContext"/> para suportar a autenticação e autorização do ASP.NET Core Identity.
/// Responsável por interagir com o banco de dados, mapeando as entidades de domínio para as tabelas correspondentes.
/// </summary>
public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>
    /// Representa a tabela de Categorias no banco de dados.
    /// </summary>
    public DbSet<Categoria> Categorias { get; set; }

    /// <summary>
    /// Representa a tabela de Comentários no banco de dados.
    /// </summary>
    public DbSet<Comentario> Comentarios { get; set; }

    /// <summary>
    /// Representa a tabela de Posts no banco de dados.
    /// </summary>
    public DbSet<Post> Posts { get; set; }

    /// <summary>
    /// Representa a tabela de Produtos no banco de dados.
    /// </summary>
    public DbSet<Produto> Produtos { get; set; }

    /// <summary>
    /// Representa a tabela de Fornecedores no banco de dados.
    /// </summary>
    public DbSet<Fornecedor> Fornecedores { get; set; }

    /// <summary>
    /// Método chamado durante a criação do modelo do banco de dados pelo Entity Framework Core.
    /// Permite configurar as entidades, seus relacionamentos e outras opções de mapeamento.
    /// </summary>
    /// <param name="modelBuilder">O construtor usado para definir o modelo do banco de dados.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Chama a implementação base para configurar as entidades do ASP.NET Core Identity.
        base.OnModelCreating(modelBuilder);

        // Aplica as configurações de modelo definidas em classes que implementam IEntityTypeConfiguration<T>.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Define convenções globais para tipos de propriedade, como string, com base no provedor de banco de dados.
    /// Esta abordagem permite substituir tipos padrão como nvarchar(max) no SQLite.
    /// </summary>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Quando o banco for SQLite, forçamos string para usar TEXT e tamanho 1000
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            configurationBuilder.Properties<string>()
                .HaveMaxLength(1000)
                .HaveColumnType("TEXT");
        }
    }
}