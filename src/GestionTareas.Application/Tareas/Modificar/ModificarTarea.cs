using GestionTareas.Application.DTOs;
using GestionTareas.Application.Tareas.Modificar;
using GestionTareas.Domain.Repositories;

namespace GestionTareas.Application.Tareas.Modificar;

public sealed class ModificarTarea : IModificarTarea
{
    private readonly ITareaRepository _tareas;

    public ModificarTarea(ITareaRepository tareas)
    {
        ArgumentNullException.ThrowIfNull(tareas);
        _tareas = tareas;
    }

    public TareaDto? Ejecutar(ModificarTareaSolicitud solicitud)
    {
        ArgumentNullException.ThrowIfNull(solicitud);

        if (solicitud.Id == Guid.Empty)
            throw new ArgumentException("El identificador de la tarea no es válido.");

        // Validar todos los datos antes de modificar la tarea.
        ArgumentException.ThrowIfNullOrWhiteSpace(solicitud.Titulo);
        ArgumentNullException.ThrowIfNull(solicitud.Descripcion);

        if (solicitud.ResponsableId == Guid.Empty)
            throw new ArgumentException("El identificador del responsable no es válido.");

        if (!Enum.IsDefined(solicitud.Estado))
            throw new ArgumentOutOfRangeException(nameof(solicitud.Estado));

        if (!Enum.IsDefined(solicitud.Prioridad))
            throw new ArgumentOutOfRangeException(nameof(solicitud.Prioridad));

        var tarea = _tareas.ObtenerPorId(solicitud.Id);

        if (tarea is null)
            return null;

        tarea.ActualizarDetalles(solicitud.Titulo, solicitud.Descripcion);
        tarea.AsignarResponsable(solicitud.ResponsableId);
        tarea.CambiarEstado(solicitud.Estado);
        tarea.CambiarPrioridad(solicitud.Prioridad);

        if (!_tareas.Actualizar(tarea))
            return null;

        return new TareaDto(
            tarea.Id,
            tarea.Titulo,
            tarea.Descripcion,
            tarea.ResponsableId,
            tarea.Estado,
            tarea.Prioridad);
    }
}