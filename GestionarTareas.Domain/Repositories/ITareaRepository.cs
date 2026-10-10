using GestionTareas.Domain.Entities;

namespace GestionTareas.Domain.Repositories;

public interface ITareaRepository
{
    Tarea? ObtenerPorId(int id);
    void Actualizar(Tarea tarea);
}