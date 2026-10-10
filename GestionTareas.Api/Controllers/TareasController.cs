using Microsoft.AspNetCore.Mvc;
using GestionTareas.Application.DTOs;
using GestionTareas.Application.Tareas.Modificar;

namespace GestionTareas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly ModificarTareaHandler _modificarTareaHandler;

    public TareasController(ModificarTareaHandler modificarTareaHandler)
    {
        _modificarTareaHandler = modificarTareaHandler;
    }

    [HttpPut("{id}")]
    public IActionResult Modificar(int id, [FromBody] ModificarTareaDto dto)
    {
        var resultado = _modificarTareaHandler.Ejecutar(id, dto);
        if (resultado == null)
        {
            return NotFound(new { mensaje = $"No se encontró la tarea con ID {id}" });
        }

        return Ok(resultado);
    }
}