using TDMD.DomainLayer;
using TDMD.InfrastructureLayer;

namespace TDMD.ApplicationLayer
{
    public class ApiService
    {
        private IApi _api;

        public ApiService(IApi api)
        {
            _api = api;
        }

        public async Task<List<Lamp>> Loadlamps()
        {
            string jsonstring = await _api.LoadLamps();

            List<Lamp> lamps = new List<Lamp>();

            lamps = _api.ParseLights(jsonstring);

            return lamps;
        }

        public async Task<string> getUserId()
        {
            return await _api.GetUserIdAsync();
        }
    }
}
