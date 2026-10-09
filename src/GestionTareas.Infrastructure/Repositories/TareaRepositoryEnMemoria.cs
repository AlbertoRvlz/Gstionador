using GestionTareas.Domain.Entities;
using GestionTareas.Domain.Enums;
using GestionTareas.Domain.Repositories;

namespace GestionTareas.Infrastructure.Repositories;

// Compartir una sola instancia durante la ejecución de la consola.
public sealed class TareaRepositoryEnMemoria : ITareaRepository
{
    private readonly Dictionary<Guid, Tarea> _tareas = new();

    public void Agregar(Tarea tarea)
    {
        ArgumentNullException.ThrowIfNull(tarea);
        _tareas.Add(tarea.Id, tarea);
    }

    public Tarea? ObtenerPorId(Guid id) => _tareas.GetValueOrDefault(id);

    public IReadOnlyList<Tarea> Consultar(EstadoTarea? estado = null, Guid? responsableId = null)
    {
        if (estado.HasValue && !Enum.IsDefined(estado.Value))
            throw new ArgumentOutOfRangeException(nameof(estado));
        return _tareas.Values
            .Where(t => !estado.HasValue || t.Estado == estado.Value)
            .Where(t => !responsableId.HasValue || t.ResponsableId == responsableId.Value)
            .ToList();
    }

    public bool Actualizar(Tarea tarea)
    {
        ArgumentNullException.ThrowIfNull(tarea);
        if (!_tareas.ContainsKey(tarea.Id))
            return false;
        _tareas[tarea.Id] = tarea;
        return true;
    }

    public bool Eliminar(Guid id) => _tareas.Remove(id);
}
