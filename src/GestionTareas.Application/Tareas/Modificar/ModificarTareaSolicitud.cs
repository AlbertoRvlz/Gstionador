using GestionTareas.Domain.Enums;

namespace GestionTareas.Application.Tareas.Modificar;

// Actualización completa; ResponsableId null significa retirar la asignación.
public sealed record ModificarTareaSolicitud(
    Guid Id,
    string Titulo,
    string Descripcion,
    Guid? ResponsableId,
    EstadoTarea Estado,
    PrioridadTarea Prioridad);
