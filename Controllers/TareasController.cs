using GestionTareas.Application.Tareas.Consultar;
using GestionTareas.Application.Tareas.Eliminar;
using GestionTareas.Application.Tareas.Modificar;
using GestionTareas.Application.Tareas.Registrar;
using GestionTareas.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareasApp.Controllers;

[ApiController]
[Route("api/tareas")]
public sealed class TareasController : ControllerBase
{
    private readonly IRegistrarTarea _registrar;
    private readonly IConsultarTareas _consultar;
    private readonly IModificarTarea _modificar;
    private readonly IEliminarTarea _eliminar;

    public TareasController(
        IRegistrarTarea registrar,
        IConsultarTareas consultar,
        IModificarTarea modificar,
        IEliminarTarea eliminar)
    {
        _registrar = registrar;
        _consultar = consultar;
        _modificar = modificar;
        _eliminar = eliminar;
    }

    [HttpPost]
    public IActionResult Registrar([FromBody] RegistrarTareaSolicitud solicitud)
    {
        try
        {
            var resultado = _registrar.Ejecutar(solicitud);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = resultado.Id },
                resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult Consultar(
        [FromQuery] EstadoTarea? estado = null,
        [FromQuery] Guid? responsableId = null)
    {
        try
        {
            var resultado = _consultar.Ejecutar(
                new ConsultarTareasFiltro(estado, responsableId));

            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public IActionResult ObtenerPorId(Guid id)
    {
        var resultado = _consultar.ObtenerPorId(id);

        if (resultado is null)
        {
            return NotFound(new
            {
                mensaje = $"No se encontro la tarea con ID {id}."
            });
        }

        return Ok(resultado);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Modificar(
        Guid id,
        [FromBody] ModificarTareaSolicitud solicitud)
    {
        try
        {
            var resultado = _modificar.Ejecutar(solicitud with { Id = id });

            if (resultado is null)
            {
                return NotFound(new
                {
                    mensaje = $"No se encontro la tarea con ID {id}."
                });
            }

            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Eliminar(Guid id)
    {
        try
        {
            if (!_eliminar.Ejecutar(id))
            {
                return NotFound(new
                {
                    mensaje = $"No se encontro la tarea con ID {id}."
                });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
