using GestionTareas.Application.Tareas.Eliminar;

namespace GestionTareasApp.Presentation;

public sealed class PantallaEliminarTarea
{
    private readonly IEliminarTarea _eliminarTarea;
    private readonly TextReader _entrada;
    private readonly TextWriter _salida;

    public PantallaEliminarTarea(
        IEliminarTarea eliminarTarea,
        TextReader? entrada = null,
        TextWriter? salida = null)
    {
        ArgumentNullException.ThrowIfNull(eliminarTarea);
        _eliminarTarea = eliminarTarea;
        _entrada = entrada ?? Console.In;
        _salida = salida ?? Console.Out;
    }

    public void Ejecutar()
    {
        _salida.Write("Identificador de la tarea (Enter para cancelar): ");
        var textoId = _entrada.ReadLine();
        if (string.IsNullOrWhiteSpace(textoId))
        {
            _salida.WriteLine("Eliminación cancelada.");
            return;
        }

        if (!Guid.TryParse(textoId.Trim(), out var id) || id == Guid.Empty)
        {
            _salida.WriteLine("Identificador no válido. Debes ingresar un GUID.");
            return;
        }

        _salida.Write($"¿Eliminar la tarea {id}? (s/N): ");
        var confirmacion = _entrada.ReadLine();
        if (!string.Equals(confirmacion?.Trim(), "s", StringComparison.OrdinalIgnoreCase))
        {
            _salida.WriteLine("Eliminación cancelada.");
            return;
        }

        _salida.WriteLine(_eliminarTarea.Ejecutar(id)
            ? "Tarea eliminada correctamente."
            : "No existe una tarea con ese identificador.");
    }
}
