namespace Futbol.Models
{
    public class FutbolStoreDatabaseSettings : IFutbolStoreDatabaseSettings
    {
        public string EquiposCollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}
