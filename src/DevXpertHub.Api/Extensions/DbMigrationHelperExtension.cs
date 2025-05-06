using DevXpertHub.Core.Entities;
using DevXpertHub.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DevXpertHub.Api.Extensions;

/// <summary>
/// Classe auxiliar para aplicar migrações e popular dados iniciais no banco de dados.
/// </summary>
public static class DbMigrationHelperExtension
{
    /// <summary>
    /// Método de extensão para o IApplicationBuilder que aplica migrações e popula dados iniciais no banco de dados.
    /// </summary>
    /// <param name="app">Instância de <see cref="IApplicationBuilder"/> usada para configurar o pipeline da aplicação.</param>
    /// <exception cref="ArgumentNullException">Lançada se o parâmetro <paramref name="app"/> for nulo.</exception>
    /// <exception cref="InvalidOperationException">Lançada se a instância de <paramref name="app"/> não for um <see cref="WebApplication"/>.</exception>
    public static void UseDbMigrationHelper(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (app is not WebApplication webApp)
        {
            throw new InvalidOperationException("A instância fornecida de IApplicationBuilder não é um WebApplication.");
        }

        try
        {
            EnsureSeedData(webApp).Wait();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Erro ao aplicar migrações ou popular dados: {ex.Message}");
            throw; // Re-throw para permitir que o erro seja tratado em outro nível, se necessário.
        }
    }

    /// <summary>
    /// Verifica se o esquema do banco de dados está atualizado.
    /// </summary>
    /// <param name="context">Contexto do banco de dados.</param>
    /// <returns>True se o esquema estiver atualizado; caso contrário, False.</returns>
    private static async Task<bool> IsSchemaUpToDate(AppDbContext context)
    {
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        return !pendingMigrations.Any();
    }

    /// <summary>
    /// Método principal chamado a partir da aplicação para iniciar o processo de seed.
    /// </summary>
    /// <param name="serviceScope">Instância de <see cref="WebApplication"/> usada para acessar os serviços da aplicação.</param>
    private static async Task EnsureSeedData(WebApplication serviceScope)
    {
        using var scope = serviceScope.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<AppDbContext>();

        try
        {
            EnsureDevDataDirectoryExists();

            // Verifica e aplica migrações, se necessário
            if (!await IsSchemaUpToDate(context))
            {
                await context.Database.MigrateAsync();
            }

            // Popula dados iniciais
            await EnsureSeedRoles(serviceProvider);
            await EnsureSeedAdminUser(serviceProvider);
            await EnsureSeedCategorias(context);
            await EnsureSeedProdutos(context);
        }
        catch (DbUpdateException ex)
        {
            Console.Error.WriteLine($"Erro ao atualizar o banco de dados: {ex.Message}");
            throw;
        }
        catch (SqliteException ex)
        {
            Console.Error.WriteLine($"Erro específico do SQLite: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Erro inesperado: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Garante que a pasta de dados exista para o ambiente de desenvolvimento.
    /// Necessário para que o SQLite consiga criar o arquivo .db.
    /// </summary>
    private static void EnsureDevDataDirectoryExists()
    {
        var solutionRoot = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..");
        var dataPath = Path.Combine(solutionRoot, "DevXpertHub.Infrastructure", "Data");
        var fullPath = Path.GetFullPath(dataPath);
        Directory.CreateDirectory(fullPath);
    }

    /// <summary>
    /// Popula o banco com categorias iniciais, se ainda não existirem.
    /// </summary>
    /// <param name="context">Instância de <see cref="AppDbContext"/> usada para interagir com o banco de dados.</param>
    private static async Task EnsureSeedCategorias(AppDbContext context)
    {
        var existingCategorias = await context.Categorias
            .Select(c => c.Nome)
            .ToListAsync();

        var categoriasSeed = new List<Categoria>
        {
        new Categoria("Eletrônicos", "Dispositivos eletrônicos e acessórios."),
        new Categoria("Livros", "Obras literárias de diversos gêneros."),
        new Categoria("Roupas", "Vestuário para todas as ocasiões."),
        new Categoria("Móveis", "Móveis para casa e escritório."),
        new Categoria("Beleza", "Produtos de beleza e cuidados pessoais."),
        new Categoria("Esportes", "Equipamentos e acessórios esportivos."),
        new Categoria("Brinquedos", "Brinquedos e jogos para crianças."),
        new Categoria("Automotivo", "Acessórios e peças para veículos."),
        new Categoria("Alimentos", "Comidas e bebidas."),
        new Categoria("Ferramentas", "Ferramentas e equipamentos para construção.")
        };

        var newCategorias = categoriasSeed
            .Where(seed => !existingCategorias.Contains(seed.Nome))
            .ToList();

        if (newCategorias.Any())
        {
            context.Categorias.AddRange(newCategorias);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Garante que os produtos iniciais existam no banco de dados.
    /// </summary>
    /// <param name="context">
    /// Instância de <see cref="AppDbContext"/> usada para interagir com o banco de dados.
    /// </param>
    /// <returns>
    /// Uma tarefa assíncrona que representa a operação de garantir os produtos. 
    /// </returns>
    private static async Task EnsureSeedProdutos(AppDbContext context)
    {
        var existingProdutos = await context.Produtos
            .Select(p => p.Nome)
            .ToListAsync();

        var categorias = await context.Categorias.ToListAsync();
        var fornecedores = await context.Fornecedores.ToListAsync();

        var produtosSeed = new List<Produto>
    {
        new Produto("Smartphone", "Smartphone de última geração", 2999.99m, 50,
            categorias.FirstOrDefault(c => c.Nome == "Eletrônicos")?.Id ?? string.Empty,
            fornecedores.FirstOrDefault(f => f.Nome == "Admin Fornecedor")?.Id ?? string.Empty,
            "img/produtos/smartphone.jpg"),
        new Produto("Fone de Ouvido Bluetooth", "Fone de ouvido sem fio com alta qualidade de som", 199.99m, 100,
            categorias.FirstOrDefault(c => c.Nome == "Eletrônicos")?.Id ?? string.Empty,
            fornecedores.FirstOrDefault(f => f.Nome == "Admin Fornecedor")?.Id ?? string.Empty,
            "img/produtos/fone-bluetooth.jpg"),
        new Produto("Camiseta Básica", "Camiseta de algodão confortável", 49.99m, 200,
            categorias.FirstOrDefault(c => c.Nome == "Roupas")?.Id ?? string.Empty,
            fornecedores.FirstOrDefault(f => f.Nome == "Admin Fornecedor")?.Id ?? string.Empty,
            "img/produtos/camiseta-basica.jpg"),
        new Produto("Livro de Ficção", "Um best-seller de ficção científica", 39.99m, 80,
            categorias.FirstOrDefault(c => c.Nome == "Livros")?.Id ?? string.Empty,
            fornecedores.FirstOrDefault(f => f.Nome == "Admin Fornecedor")?.Id ?? string.Empty,
            "img/produtos/livro-ficcao.jpg")
    };

        var newProdutos = produtosSeed
            .Where(seed => !existingProdutos.Contains(seed.Nome))
            .ToList();

        if (newProdutos.Any())
        {
            context.Produtos.AddRange(newProdutos);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Garante que roles (funções de usuário) básicas existam no Identity.
    /// </summary>
    /// <param name="serviceProvider">Provedor de serviços usado para resolver dependências.</param>
    private static async Task EnsureSeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var existingRoles = await roleManager.Roles
            .Select(r => r.Name)
            .ToListAsync();

        var roles = new[] { "Admin", "Consumidor", "Fornecedor" };

        foreach (var role in roles.Where(r => !existingRoles.Contains(r)))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    /// <summary>
    /// Garante que ao menos um usuário/fornecedor esteja cadastrado com a role "Administrador".
    /// </summary>
    /// <param name="serviceProvider">Provedor de serviços usado para resolver dependências.</param>
    private static async Task EnsureSeedAdminUser(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var context = serviceProvider.GetRequiredService<AppDbContext>();

        // Verifica se já existe um usuário com a role "Administrador"
        var adminRoleUsers = await userManager.GetUsersInRoleAsync("Admin");
        if (adminRoleUsers.Any())
        {
            return; // Já existe um administrador, não é necessário criar outro
        }

        // Dados do usuário administrador
        var adminUser = new IdentityUser
        {
            Id = "63928ddc-ca9b-49ab-85cd-b5c76f0596eb", // ID fixo para associar ao fornecedor
            UserName = "admin@devxperthub.com",
            Email = "admin@devxperthub.com",
            EmailConfirmed = true
        };

        // Cria o usuário no Identity, se ainda não existir
        var existingUser = await userManager.FindByEmailAsync(adminUser.Email);
        if (existingUser == null)
        {
            var result = await userManager.CreateAsync(adminUser, "Admin@123"); // Senha padrão
            if (!result.Succeeded)
            {
                throw new Exception($"Erro ao criar o usuário administrador: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        // Garante que o usuário tenha a role "Administrador"
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        // Verifica se o fornecedor já existe no banco de dados
        var existingFornecedor = await context.Fornecedores.FindAsync(adminUser.Id);
        if (existingFornecedor == null)
        {
            // Cria o fornecedor associado ao usuário
            var fornecedor = new Fornecedor(adminUser.Id, "Admin Fornecedor", adminUser.Email, null);
            context.Fornecedores.Add(fornecedor);
            await context.SaveChangesAsync();
        }
    }
}