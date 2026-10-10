using GestionTareas.Domain.Entities;
using GestionTareas.Domain.Enums;

namespace GestionTareas.Domain.Repositories;

public interface ITareaRepository
{
    void Agregar(Tarea tarea);
    Tarea? ObtenerPorId(Guid id);
    IReadOnlyList<Tarea> Consultar(EstadoTarea? estado = null, Guid? responsableId = null);
    // Devuelve false si la tarea no existe.
    bool Actualizar(Tarea tarea);
    bool Eliminar(Guid id);
}
