using DevXpertHub.Domain.Entities;
using DevXpertHub.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevXpertHub.Web.Extensions;

public static class DbMigrationHelpers
{
    public static async Task EnsureSeedData(WebApplication serviceScope)
    {
        var services = serviceScope.Services.CreateScope().ServiceProvider;
        await EnsureSeedData(services);
    }

    public static async Task EnsureSeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        // Get the DbContext instance from the service provider
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (env.IsDevelopment() || env.IsEnvironment("Docker") || env.IsStaging())
        {
            await context.Database.MigrateAsync();
            await EnsureSeedCategorias(context);
            await EnsureSeedRoles(serviceProvider);
        }
    }

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
            // Verifica se já existe uma categoria com mesmo nome
            var exists = await context.Categorias
                .AnyAsync(c => c.Nome == seed.Nome);

            if (!exists)
            {
                context.Categorias.Add(seed);
            }
        }

        await context.SaveChangesAsync();
    }
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