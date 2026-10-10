using GestionTareas.Application.DTOs;
using GestionTareas.Domain.Entities;
using GestionTareas.Domain.Repositories;

namespace GestionTareas.Application.Tareas.Modificar;

public class ModificarTareaHandler
{
    private readonly ITareaRepository _tareaRepository;

    public ModificarTareaHandler(ITareaRepository tareaRepository)
    {
        _tareaRepository = tareaRepository;
    }

    public Tarea? Ejecutar(int id, ModificarTareaDto dto)
    {
        var tareaExistente = _tareaRepository.ObtenerPorId(id);
        if (tareaExistente == null)
            return null;

        tareaExistente.Titulo = dto.Titulo;
        tareaExistente.Descripcion = dto.Descripcion;
        tareaExistente.ResponsableId = dto.ResponsableId;
        tareaExistente.Estado = dto.Estado;
        tareaExistente.Prioridad = dto.Prioridad;

        _tareaRepository.Actualizar(tareaExistente);
        return tareaExistente;
    }
}