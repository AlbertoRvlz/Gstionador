using GestionTareas.Domain.Repositories;
using GestionTareas.Infrastructure.Repositories;
using GestionTareas.Application.Tareas.Modificar;

var builder = WebApplication.CreateBuilder(args);

// Registrar controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias para tus capas
builder.Services.AddScoped<ITareaRepository, TareaRepository>();
builder.Services.AddScoped<ModificarTareaHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();