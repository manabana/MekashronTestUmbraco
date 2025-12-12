
using Mekashron.Domain;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mekashron.Domain.Repositories.GeoIp
{
    public interface IGeoIpRepository
    {
        Task<GeoIpResponse?> GetGeoIpResponse(String ip);

    }
}
