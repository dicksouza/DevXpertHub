using DevXpertHub.Domain.Entities;
using DevXpertHub.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevXpertHub.Web.Extensions;

/// <summary>
/// Classe auxiliar para aplicar migrações e popular dados iniciais no banco de dados.
/// </summary>
public static class DbMigrationHelpers
{
    /// <summary>
    /// Método principal chamado a partir da aplicação para iniciar o processo de seed.
    /// Cria um escopo e chama a versão interna baseada em IServiceProvider.
    /// </summary>
    public static async Task EnsureSeedData(WebApplication serviceScope)
    {
        var services = serviceScope.Services.CreateScope().ServiceProvider;
        await EnsureSeedData(services);
    }

    /// <summary>
    /// Aplica migrações e popula dados apenas nos ambientes de desenvolvimento, Docker ou staging.
    /// </summary>
    public static async Task EnsureSeedData(IServiceProvider serviceProvider)
    {
        // Cria um escopo para obter os serviços necessários
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        // Obtém o contexto do banco de dados
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Verifica se está em um ambiente apropriado para aplicar migrações e popular dados
        if (env.IsDevelopment() || env.IsEnvironment("Docker") || env.IsStaging())
        {
            // Garante que a pasta 'Data' exista antes de criar o banco SQLite
            EnsureDevDataDirectoryExists();

            // Aplica quaisquer migrações pendentes
            await context.Database.MigrateAsync();

            // Popula categorias iniciais
            await EnsureSeedCategorias(context);

            // Cria roles iniciais no Identity
            await EnsureSeedRoles(scope.ServiceProvider);
        }
    }

    /// <summary>
    /// Garante que a pasta de dados exista para o ambiente de desenvolvimento.
    /// Necessário para que o SQLite consiga criar o arquivo .db.
    /// </summary>
    private static void EnsureDevDataDirectoryExists()
    {
        // Caminho do projeto raiz
        var solutionRoot = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..");

        // Caminho da pasta DevXpertHub.Infrastructure/Data
        var dataPath = Path.Combine(solutionRoot, "DevXpertHub.Infrastructure", "Data");

        var fullPath = Path.GetFullPath(dataPath);
        Directory.CreateDirectory(fullPath); // Cria a pasta se não existir
    }

    /// <summary>
    /// Popula o banco com categorias iniciais, se ainda não existirem.
    /// </summary>
    private static async Task EnsureSeedCategorias(AppDbContext context)
    {
        var categoriasSeed = new List<Categoria>
        {
            new Categoria("Eletrônicos", "Dispositivos eletrônicos e acessórios."),
            new Categoria("Livros", "Obras literárias de diversos gêneros."),
            new Categoria("Roupas", "Vestuário para todas as ocasiões.")
        };

        foreach (var seed in categoriasSeed)
        {
            // Verifica se a categoria já existe antes de inserir
            if (!await context.Categorias.AnyAsync(c => c.Nome == seed.Nome))
            {
                context.Categorias.Add(seed);
            }
        }

        await context.SaveChangesAsync(); // Salva alterações no banco
    }

    /// <summary>
    /// Garante que roles (funções de usuário) básicas existam no Identity.
    /// </summary>
    private static async Task EnsureSeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new[] { "Administrador", "Consumidor", "Vendedor" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}