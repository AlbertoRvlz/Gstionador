using GestionTareas.Application.DTOs;
using GestionTareas.Domain.Entities;
using GestionTareas.Domain.Repositories;

namespace GestionTareas.Application.Tareas.Consultar;

public sealed class ConsultarTareas : IConsultarTareas
{
    private readonly ITareaRepository _tareaRepository;

    public ConsultarTareas(ITareaRepository tareaRepository)
    {
        ArgumentNullException.ThrowIfNull(tareaRepository);
        _tareaRepository = tareaRepository;
    }

    public IReadOnlyList<TareaDto> Ejecutar(ConsultarTareasFiltro filtro)
    {
        ArgumentNullException.ThrowIfNull(filtro);

        return _tareaRepository
            .Consultar(filtro.Estado, filtro.ResponsableId)
            .Select(Mapear)
            .ToList();
    }

    public TareaDto? ObtenerPorId(Guid id)
    {
        var tarea = _tareaRepository.ObtenerPorId(id);
        return tarea is null ? null : Mapear(tarea);
    }

    private static TareaDto Mapear(Tarea tarea)
    {
        ArgumentNullException.ThrowIfNull(tarea);

        return new TareaDto(
            tarea.Id,
            tarea.Titulo,
            tarea.Descripcion,
            tarea.ResponsableId,
            tarea.Estado,
            tarea.Prioridad);
    }
}
