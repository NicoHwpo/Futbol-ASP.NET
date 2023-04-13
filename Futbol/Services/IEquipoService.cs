using Futbol.Models;

namespace Futbol.Services
{
    public interface IEquipoService
    {
        Task<List<Equipo>> GetAll();
        Task<Equipo> Get(string id);
        Task<Equipo> Create(Equipo equipo);
        Task Update(string id, Equipo equipo);
        Task Remove(string id);
    }
}
