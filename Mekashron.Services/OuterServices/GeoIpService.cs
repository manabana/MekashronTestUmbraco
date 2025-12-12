using Mekashron.Domain.Repositories.GeoIp;
using Mekashron.Domain.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mekashron.Services.OuterServices
{
    public class GeoIpService: IGeoIpService
    {
        private readonly IGeoIpRepository _geoIpRepository;

        public GeoIpService(IGeoIpRepository geoIpRepository)
        {
            _geoIpRepository = geoIpRepository;
        }

        public async Task<String?> GetCountryAsync(String ip)
        {
            var response = await _geoIpRepository.GetGeoIpResponse(ip);

            if (response is not null && response.Status == "success")
                return response.CountryCode;

            return null;
        }
    }
}
