using GestionTareas.Application.DTOs;

namespace GestionTareas.Application.Tareas.Modificar;

public interface IModificarTarea
{
    // null si la tarea no existe; validar toda la solicitud antes de aplicar cambios.
    TareaDto? Ejecutar(ModificarTareaSolicitud solicitud);
}
