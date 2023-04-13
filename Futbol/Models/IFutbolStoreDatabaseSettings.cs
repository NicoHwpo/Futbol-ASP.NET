namespace Futbol.Models
{
    public interface IFutbolStoreDatabaseSettings
    {
        string EquiposCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
