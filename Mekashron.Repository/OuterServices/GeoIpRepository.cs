using Mekashron.Domain;
using Mekashron.Domain.Repositories.GeoIp;
using System.Net;
using System.Net.Http.Json;

namespace Mekashron.Repository.OuterServices
{
    public class GeoIpRepository : IGeoIpRepository
    {
        private readonly HttpClient _http;

        public GeoIpRepository(HttpClient http)
        {
            _http = http;
        }

        public Task<GeoIpResponse?> GetGeoIpResponse(String ip)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            try
            {
                return _http.GetFromJsonAsync<GeoIpResponse>(
                    $"http://ip-api.com/json/{ip}?fields=status,countryCode", cts.Token);
            }
            catch
            {
                return Task.FromResult<GeoIpResponse?>(null);
            }
        }
    }
}
