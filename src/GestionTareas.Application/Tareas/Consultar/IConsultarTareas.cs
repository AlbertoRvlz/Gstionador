using GestionTareas.Application.DTOs;

namespace GestionTareas.Application.Tareas.Consultar;

public interface IConsultarTareas
{
    IReadOnlyList<TareaDto> Ejecutar(ConsultarTareasFiltro filtro);
    TareaDto? ObtenerPorId(Guid id);
}
