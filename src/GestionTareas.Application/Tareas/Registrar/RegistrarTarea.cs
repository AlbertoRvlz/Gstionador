using GestionTareas.Application.DTOs;
using GestionTareas.Domain.Entities;
using GestionTareas.Domain.Repositories;

namespace GestionTareas.Application.Tareas.Registrar;

public sealed class RegistrarTarea : IRegistrarTarea
{
    private readonly ITareaRepository _tareas;

    public RegistrarTarea(ITareaRepository tareas)
    {
        ArgumentNullException.ThrowIfNull(tareas);
        _tareas = tareas;
    }

    public TareaDto Ejecutar(RegistrarTareaSolicitud solicitud)
    {
        ArgumentNullException.ThrowIfNull(solicitud);

        var tarea = new Tarea(
            solicitud.Titulo,
            solicitud.Descripcion,
            solicitud.Prioridad,
            solicitud.ResponsableId);

        _tareas.Agregar(tarea);

        return new TareaDto(
            tarea.Id,
            tarea.Titulo,
            tarea.Descripcion,
            tarea.ResponsableId,
            tarea.Estado,
            tarea.Prioridad);
    }
}
