using GestionTareas.Domain.Enums;

namespace GestionTareas.Application.Tareas.Registrar;

public sealed record RegistrarTareaSolicitud(
    string Titulo,
    string Descripcion,
    PrioridadTarea Prioridad,
    Guid? ResponsableId = null);
