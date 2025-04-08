using DevXpertHub.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DevXpertHub.Infrastructure.DataInitialization
{
    public static class DatabaseInitializer
    {
        public static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Aplica as migrations se elas ainda não foram aplicadas
                context.Database.Migrate();

                // Chama o método de Seed para popular os dados iniciais
                SeedData(context);
            }
        }

        private static void SeedData(AppDbContext context)
        {
            // Seed de Categorias
            if (!context.Categorias.Any())
            {
                context.Categorias.AddRange(
                    new Categoria ( "Eletrônicos",  "Dispositivos eletrônicos e acessórios."),
                    new Categoria ("Eletrônicos", "Obras literárias de diversos gêneros."),
                    new Categoria ("Eletrônicos", "Vestuário para todas as ocasiões.")
                );
            }

            context.SaveChanges();
        }
    }
}