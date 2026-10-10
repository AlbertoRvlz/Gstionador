using System.Text;
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
