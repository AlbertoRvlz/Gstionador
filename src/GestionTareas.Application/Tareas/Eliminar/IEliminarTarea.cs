namespace GestionTareas.Application.Tareas.Eliminar;

public interface IEliminarTarea
{
    // true si se eliminó; false si el identificador no existe.
    bool Ejecutar(Guid id);
}
