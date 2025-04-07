using DevXpertHub.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Banco de Dados
builder.Services.AddDatabaseConfiguration(builder.Configuration,
                                          builder.Environment.IsDevelopment());

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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Mapeamento das Rotas
app.MapDefaultRoutes();

app.Run();