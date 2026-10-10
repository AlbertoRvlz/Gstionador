using GestionTareas.Domain.Repositories;

namespace GestionTareas.Application.Tareas.Eliminar;

public sealed class EliminarTarea : IEliminarTarea
{
    private readonly ITareaRepository _tareas;

    public EliminarTarea(ITareaRepository tareas)
    {
        ArgumentNullException.ThrowIfNull(tareas);
        _tareas = tareas;
    }

    public bool Ejecutar(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("El identificador de la tarea no es válido.", nameof(id));

        return _tareas.Eliminar(id);
    }
}
