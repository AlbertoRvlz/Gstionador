using GestionTareas.Application.Tareas.Consultar;
using GestionTareas.Application.Tareas.Eliminar;
using GestionTareas.Application.Tareas.Modificar;
using GestionTareas.Application.Tareas.Registrar;
using GestionTareas.Domain.Repositories;
using GestionTareas.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITareaRepository, TareaRepositoryEnMemoria>();
builder.Services.AddSingleton<IIntegranteRepository, IntegranteRepositoryEnMemoria>();

builder.Services.AddScoped<IRegistrarTarea, RegistrarTarea>();
builder.Services.AddScoped<IConsultarTareas, ConsultarTareas>();
builder.Services.AddScoped<IModificarTarea, ModificarTarea>();
builder.Services.AddScoped<IEliminarTarea, EliminarTarea>();

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