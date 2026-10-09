using System.Text;
using GestionTareas.Domain.Repositories;
using GestionTareas.Infrastructure.Repositories;
using GestionTareasApp.Presentation;

Console.OutputEncoding = Encoding.UTF8;

// Punto de composición: conectar aquí los casos de uso cuando estén implementados.
ITareaRepository tareas = new TareaRepositoryEnMemoria();
IIntegranteRepository integrantes = new IntegranteRepositoryEnMemoria();

Console.WriteLine("Gestión de tareas — estructura base");
Console.WriteLine($"Integrantes disponibles: {integrantes.ObtenerTodos().Count}");
Console.WriteLine($"Tareas en memoria: {tareas.Consultar().Count}");
new MenuConsola().Ejecutar();
