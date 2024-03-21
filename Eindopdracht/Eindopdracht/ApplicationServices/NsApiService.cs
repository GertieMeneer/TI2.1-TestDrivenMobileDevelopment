using Eindopdracht.Domain;

namespace Eindopdracht.ApplicationServices
{
    public class NsApiService
    {
        private INsApi _nsApi;

        public NsApiService(INsApi nsApi) { _nsApi = nsApi; }

        public async Task<List<NSStation>> getAllStations(double latitude, double longtitude)
        {
            return await _nsApi.GetAllNSStations(latitude, longtitude);
        }
    }
}
