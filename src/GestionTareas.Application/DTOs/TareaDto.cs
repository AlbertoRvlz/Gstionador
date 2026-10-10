using GestionTareas.Domain.Enums;

namespace GestionTareas.Application.DTOs;

public sealed record TareaDto(
    Guid Id,
    string Titulo,
    string Descripcion,
    Guid? ResponsableId,
    EstadoTarea Estado,
    PrioridadTarea Prioridad);
