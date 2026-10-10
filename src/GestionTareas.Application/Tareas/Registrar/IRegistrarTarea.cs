using GestionTareas.Application.DTOs;

namespace GestionTareas.Application.Tareas.Registrar;

public interface IRegistrarTarea
{
    TareaDto Ejecutar(RegistrarTareaSolicitud solicitud);
}
