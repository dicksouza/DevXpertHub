using DevXpertHub.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddDebug();
builder.Logging.AddConsole();

builder.Services.AddAuthorization();

// Configuração do banco de dados
builder.Services.AddDatabaseConfiguration(builder.Configuration,
                                          builder.Environment.IsDevelopment());

// Configuração do Identity
builder.Services.AddIdentityConfiguration();

// Injeção de dependência - Repositórios
builder.Services.AddRepositories();

// Injeção de dependência - Serviços
builder.Services.AddServices();

// Configuração da Autenticação JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

// Configuração do OpenAPI (Swagger)
builder.Services.AddOpenApiConfiguration();

// Configuração dos Controllers e Rotas
builder.Services.AddControllersConfiguration();

// Configuração do CORS (se necessário)
// builder.Services.AddCorsConfiguration();

var app = builder.Build();

// Configuração do ambiente de desenvolvimento
app.UseEnvironmentConfiguration(app.Environment);

// Configuração do Swagger/OpenAPI
app.UseOpenApiConfiguration();

// Configuração do CORS (se necessário)
// app.UseCors();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapOpenApi();

app.Run();