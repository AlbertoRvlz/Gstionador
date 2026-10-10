using GestionTareas.Domain.Entities;

namespace GestionTareas.Domain.Repositories;

public interface IIntegranteRepository
{
    Integrante? ObtenerPorId(Guid id);
    IReadOnlyList<Integrante> ObtenerTodos();
}
