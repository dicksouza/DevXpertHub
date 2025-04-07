using DevXpertHub.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do Banco de Dados
builder.Services.AddDatabaseConfiguration(builder.Configuration,
                                          builder.Environment.IsDevelopment());

// Inje��o de Depend�ncias
builder.Services.AddDependencyInjection();

// Configura��o do MVC
builder.Services.AddMvcConfiguration();

// Configura��o de Logging
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configura��o de Localiza��o
app.UseLocalizationConfiguration();

// Configura��o do Ambiente
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