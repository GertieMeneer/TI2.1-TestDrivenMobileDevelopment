namespace TDMD.DomainLayer
{
    public interface IApi
    {
        public Task<string> LoadLamps();
        public List<Lamp> ParseLights(string jsonResponse);
        public Task<string> GetUserIdAsync();
    }
}
