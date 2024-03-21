namespace Eindopdracht.Domain
{
    public interface INsApi
    {
        public Task<List<NSStation>> GetAllNSStations(double latitude, double longitude);
    }
}