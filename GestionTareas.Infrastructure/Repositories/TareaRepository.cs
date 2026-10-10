using GestionTareas.Domain.Entities;
using GestionTareas.Domain.Enums;
using GestionTareas.Domain.Repositories;

namespace GestionTareas.Infrastructure.Repositories;

public class TareaRepository : ITareaRepository
{
    private static readonly List<Tarea> _baseDatos = new()
    {
        new Tarea 
        { 
            Id = 1, 
            Titulo = "Diseñar UI", 
            Descripcion = "Pantallas iniciales en Angular", 
            Estado = EstadoTarea.Pendiente, 
            Prioridad = PrioridadTarea.Media 
        }
    };

    public Tarea? ObtenerPorId(int id)
    {
        return _baseDatos.FirstOrDefault(t => t.Id == id);
    }

    public void Actualizar(Tarea tarea)
    {
        var index = _baseDatos.FindIndex(t => t.Id == tarea.Id);
        if (index != -1)
        {
            _baseDatos[index] = tarea;
        }
    }
}