using Futbol.Models;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace Futbol.Services
{
    public class EquipoService : IEquipoService
    {
        private readonly IMongoCollection<Equipo> _equipo;

        public EquipoService(IFutbolStoreDatabaseSettings settings)
        {
            var mongoClient = new MongoClient(settings.ConnectionString);
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _equipo = database.GetCollection<Equipo>(settings.EquiposCollectionName);
            
        }

        public async Task<List<Equipo>> GetAll()
        {
            return await _equipo.Find(Equipo => true).ToListAsync();
        }
        public async Task<Equipo> Get(string id)
        {
            return await _equipo.Find<Equipo>(Equipo => Equipo.Id == id).FirstOrDefaultAsync();

        }
        public async Task<Equipo> Create(Equipo Equipo)
        {
            await _equipo.InsertOneAsync(Equipo);
            return Equipo;
        }
        public async Task Update(string id, Equipo equipoIn)
        {
            await _equipo.ReplaceOneAsync(equipo => equipo.Id == id, equipoIn);
        }

        public async Task Remove(string id)
        {
            await _equipo.DeleteOneAsync(equipo => equipo.Id == id);

        }
    }

}
