using DevXpertHub.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Banco de Dados
builder.Services.AddDatabaseConfiguration(builder.Configuration,
                                          builder.Environment.IsDevelopment());

// Injeção de Dependências do Identity
builder.Services.AddIdentityConfiguration();

// Injeção de Dependências
builder.Services.AddDependencyInjectionConfiguration();

// Configuração do MVC
builder.Services.AddMvcConfiguration();
builder.Services.AddLocalizationConfiguration();

// Configuração de Logging
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configuração de middlewares específicos do ambiente (desenvolvimento ou produção).
app.UseEnvironmentConfiguration(app.Environment);

// Configuração de middlewares essenciais para a aplicação (HTTPS, roteamento, autenticação, autorização, localização).
app.UseHttpsRedirection();
app.UseRouting();

// Configuração de middlewares de internacionalização e localização.
app.UseLocalizationConfiguration();

// Configuração de middlewares de autenticação e autorização.
app.UseAuthentication();
app.UseAuthorization();

// Configuração de middlewares de arquivos estáticos.
app.UseSession();

// Mapeia os endpoints da aplicação (Controllers, Razor Pages).
app.MapEndpoints();

app.Run();