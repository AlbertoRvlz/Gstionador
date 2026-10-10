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
using System.Text;
using GestionTareas.Application.Tareas.Eliminar;
using GestionTareas.Domain.Repositories;
using GestionTareas.Infrastructure.Repositories;
using GestionTareasApp.Presentation;

Console.OutputEncoding = Encoding.UTF8;

ITareaRepository tareas = new TareaRepositoryEnMemoria();
IIntegranteRepository integrantes = new IntegranteRepositoryEnMemoria();

Console.WriteLine("Gestión de tareas — estructura base");
Console.WriteLine($"Integrantes disponibles: {integrantes.ObtenerTodos().Count}");
Console.WriteLine($"Tareas en memoria: {tareas.Consultar().Count}");
new MenuConsola(tareas, integrantes).Ejecutar();
IEliminarTarea eliminarTarea = new EliminarTarea(tareas);
var pantallaEliminar = new PantallaEliminarTarea(eliminarTarea);
new MenuConsola(pantallaEliminar).Ejecutar();
