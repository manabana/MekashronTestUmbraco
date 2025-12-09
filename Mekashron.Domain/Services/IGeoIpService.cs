using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mekashron.Domain.Services
{
    public interface IGeoIpService
    {
        Task<String?> GetCountryAsync(IPAddress ip);
    }
}
