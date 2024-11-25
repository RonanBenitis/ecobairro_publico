using ecobairroServer.Data;
using ecobairroServer.Source.Core.Services;
using ecobairroServer.Source.Core.Services.Interface;
using ecobairroServer.Source.Inbound.Handler;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var appVersion = "v0";

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v0", new OpenApiInfo
    {
        Title = "EcoBairro",
        Version = appVersion,
        Description = "Documenta��o de endpoints"
    });

    // Adicione o caminho do arquivo XML gerado pela documenta��o
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath); // Inclui os coment�rios XML na documenta��o
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder
        .Configuration
        .GetConnectionString("DefaultConnection")
        );
});

// Inje��o de dependencia
builder.Services.AddScoped<IMunicipeService, MunicipeService>();
builder.Services.AddScoped<IFiscalService, FiscalService>();
builder.Services.AddScoped<IPinService, PinService>();
builder.Services.AddScoped<IChamadasPinService, ChamadasPinService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBairroService, BairroService>();

// Ativando CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("endere�o_do_renderer")
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/{appVersion}/swagger.json", "Documenta��o de endpoints");
    });
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

// Middleware para tratamento de exce��es de HTTPS
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
