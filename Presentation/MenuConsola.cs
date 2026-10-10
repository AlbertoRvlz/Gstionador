using GestionTareas.Application.DTOs;
using GestionTareas.Application.Tareas.Consultar;
using GestionTareas.Domain.Enums;
using GestionTareas.Domain.Repositories;

namespace GestionTareasApp.Presentation;

public sealed class MenuConsola
{
    private readonly ITareaRepository _tareas;
    private readonly IIntegranteRepository _integrantes;

    public MenuConsola(ITareaRepository tareas, IIntegranteRepository integrantes)
    {
        _tareas = tareas ?? throw new ArgumentNullException(nameof(tareas));
        _integrantes = integrantes ?? throw new ArgumentNullException(nameof(integrantes));
    }

    public void Ejecutar()
    {
        while (true)
        {
            Console.WriteLine("\n1. Registrar tarea");
            Console.WriteLine("2. Consultar tareas (estado o responsable)");
            Console.WriteLine("3. Modificar tarea (detalles, responsable, estado o prioridad)");
            Console.WriteLine("4. Eliminar tarea");
            Console.WriteLine("0. Salir");
            Console.Write("Opción: ");
            var opcion = Console.ReadLine();
            if (opcion is null or "0")
                return;

            switch (opcion)
            {
                case "1":
                    Console.WriteLine("Pendiente: Agustín — feature/registro-tareas.");
                    break;
                case "2":
                    ConsultarTareasMenu();
                    break;
                case "3":
                    Console.WriteLine("Pendiente: Ángel — feature/modificar-tareas.");
                    break;
                case "4":
                    Console.WriteLine("Pendiente: Edwin — feature/eliminar.");
                    break;
                default:
                    Console.WriteLine("Opción no válida. Selecciona del 0 al 4.");
                    break;
            }
        }
    }

    private void ConsultarTareasMenu()
    {
        var servicio = new ConsultarTareas(_tareas);

        Console.WriteLine("\nConsulta de tareas");
        Console.WriteLine("1. Ver todas");
        Console.WriteLine("2. Buscar por ID");
        Console.WriteLine("3. Filtrar por estado");
        Console.WriteLine("4. Filtrar por responsable");
        Console.WriteLine("5. Filtrar por estado y responsable");
        Console.WriteLine("0. Volver");
        Console.Write("Opción: ");

        var opcion = Console.ReadLine();
        if (opcion is null or "0")
            return;

        try
        {
            IReadOnlyList<TareaDto> tareas = opcion switch
            {
                "1" => servicio.Ejecutar(new ConsultarTareasFiltro()),
                "2" => BuscarPorId(servicio),
                "3" => servicio.Ejecutar(new ConsultarTareasFiltro(LeerEstado())),
                "4" => servicio.Ejecutar(new ConsultarTareasFiltro(ResponsableId: LeerResponsableId())),
                "5" => servicio.Ejecutar(new ConsultarTareasFiltro(LeerEstado(), LeerResponsableId())),
                _ => throw new InvalidOperationException("Opción no válida.")
            };

            MostrarTareas(tareas);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"No se pudo consultar la tarea: {ex.Message}");
        }
    }

    private IReadOnlyList<TareaDto> BuscarPorId(ConsultarTareas servicio)
    {
        Console.Write("ID de la tarea: ");
        var texto = Console.ReadLine();
        if (!Guid.TryParse(texto, out var id))
            throw new InvalidOperationException("El identificador no es válido.");

        var tarea = servicio.ObtenerPorId(id);
        return tarea is null ? Array.Empty<TareaDto>() : new[] { tarea };
    }

    private EstadoTarea? LeerEstado()
    {
        Console.WriteLine("Estados disponibles: 1=Pendiente, 2=EnProceso, 3=Terminada");
        Console.Write("Estado: ");
        var texto = Console.ReadLine();
        return texto is null || texto.Trim() == string.Empty
            ? null
            : Enum.TryParse<EstadoTarea>(texto, out var estado)
                ? estado
                : throw new InvalidOperationException("El estado indicado no es válido.");
    }

    private Guid? LeerResponsableId()
    {
        Console.WriteLine("Responsables disponibles:");
        foreach (var integrante in _integrantes.ObtenerTodos())
            Console.WriteLine($"- {integrante.Id} => {integrante.Nombre}");

        Console.Write("ID del responsable (dejar vacío para omitir): ");
        var texto = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(texto))
            return null;

        if (!Guid.TryParse(texto, out var responsableId))
            throw new InvalidOperationException("El identificador del responsable no es válido.");

        if (_integrantes.ObtenerPorId(responsableId) is null)
            throw new InvalidOperationException("El responsable indicado no existe.");

        return responsableId;
    }

    private static void MostrarTareas(IEnumerable<TareaDto> tareas)
    {
        var lista = tareas.ToList();
        if (lista.Count == 0)
        {
            Console.WriteLine("No hay tareas que coincidan con la búsqueda.");
            return;
        }

        foreach (var tarea in lista)
        {
            var responsable = tarea.ResponsableId is null ? "Sin asignar" : tarea.ResponsableId.Value.ToString();
            Console.WriteLine($"- {tarea.Id} | {tarea.Titulo} | {tarea.Estado} | {tarea.Prioridad} | Responsable: {responsable}");
        }
    }
}
