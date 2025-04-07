using DevXpertHub.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddDebug();
builder.Logging.AddConsole();

builder.Services.AddAuthorization();

// Configura��o do banco de dados
builder.Services.AddDatabaseConfiguration(builder.Configuration,
                                          builder.Environment.IsDevelopment());

// Configura��o do Identity
builder.Services.AddIdentityConfiguration();

// Inje��o de depend�ncia - Reposit�rios
builder.Services.AddRepositories();

// Inje��o de depend�ncia - Servi�os
builder.Services.AddServices();

// Configura��o da Autentica��o JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

// Configura��o do OpenAPI (Swagger)
builder.Services.AddOpenApiConfiguration();

// Configura��o dos Controllers e Rotas
builder.Services.AddControllersConfiguration();

// Configura��o do CORS (se necess�rio)
// builder.Services.AddCorsConfiguration();

var app = builder.Build();

// Configura��o do ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperEnvironment();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Configura��o do Swagger/OpenAPI
//app.UseSwaggerConfiguration(app.Environment);

// Configura��o do CORS (se necess�rio)
// app.UseCors();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapOpenApi();

app.Run();