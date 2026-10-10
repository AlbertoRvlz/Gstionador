using GestionTareas.Domain.Entities;
using GestionTareas.Domain.Repositories;

namespace GestionTareas.Infrastructure.Repositories;

public sealed class IntegranteRepositoryEnMemoria : IIntegranteRepository
{
    private readonly IReadOnlyList<Integrante> _integrantes = new List<Integrante>
    {
        new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Agustín"),
        new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Joquin"),
        new(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Ángel"),
        new(Guid.Parse("44444444-4444-4444-4444-444444444444"), "Edwin")
    }.AsReadOnly();

    public Integrante? ObtenerPorId(Guid id) => _integrantes.FirstOrDefault(i => i.Id == id);
    public IReadOnlyList<Integrante> ObtenerTodos() => _integrantes;
}
