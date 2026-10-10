using GestionTareas.Domain.Enums;

namespace GestionTareas.Application.Tareas.Consultar;

// Si se indican ambos filtros, se combinan con AND.
public sealed record ConsultarTareasFiltro(EstadoTarea? Estado = null, Guid? ResponsableId = null);