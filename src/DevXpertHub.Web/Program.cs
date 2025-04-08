using DevXpertHub.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Banco de Dados
builder.Services.AddDatabaseConfiguration(builder.Configuration,
                                          builder.Environment.IsDevelopment());

// Injeção de Dependências do Identity
builder.Services.AddIdentityInjection();

// Injeção de Dependências
builder.Services.AddDependencyInjection();

// Configuração do MVC
builder.Services.AddMvcConfiguration();

// Configuração de Logging
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configuração de Localização
app.UseLocalizationConfiguration();

// Configuração do Ambiente
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDbMigrationHelper();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();