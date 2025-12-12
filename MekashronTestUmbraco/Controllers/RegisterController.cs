using Mekashron.Domain;
using Mekashron.Domain.Api;
using Mekashron.Domain.Services;
using Mekashron.Tools;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MekashronTestUmbraco.Controllers
{
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IMekashronApiService _mekashronApiService;
        private readonly IGeoIpService _geoIpService;
        private readonly IConfiguration _configuration;

        public RegisterController(
            IConfiguration configuration,
            IMekashronApiService mekashronApiService,
            IGeoIpService geoIpService
        )
        {
            _configuration = configuration;
            _mekashronApiService = mekashronApiService;
            _geoIpService = geoIpService;
        }

        public class RegisterRequest
        {
            public string? Name { get; set; }
            public string? Company { get; set; }
            public string? Phone { get; set; }
            public string? Email { get; set; }
        }

        [HttpPost("/api/register")]
        public async Task<IActionResult> Post([FromBody] RegisterRequest request)
        {
            if (request is null) return BadRequest(new { error = "Request is empty" });

            String? ip = TryGetVisitorIp();

            String? countryISO = null;
            if (ip is not null)
            {
                countryISO = await _geoIpService.GetCountryAsync(ip);
            }

            var blank = new CustomerBlank
            {
                OlEntityId = Int32.Parse(_configuration["RegisterEntityData:olEntityId"]!),
                OlUsername = _configuration["RegisterEntityData:olUsername"]!,
                OlPassword = _configuration["RegisterEntityData:olPassword"]!,
                BusinessId = Int32.Parse(_configuration["RegisterEntityData:BusinessId"]!),
                CategoryId = Int32.Parse(_configuration["RegisterEntityData:CategoryId"]!),
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                CountryISO = countryISO ?? "IL"
            };

            Result<MekashronRegisterResponse> result = await _mekashronApiService.RegisterNewCustomer(blank);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Error?.Message, key = result.Error?.Key });
            }

            if(result.Value?.EntityId is null) return BadRequest(new { error = "EntityId is null in response" });

            Response.Cookies.Append("download_allowed", "1", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(10)
            });

            if (ip is not null)
            {
                CustomFieldsTableBlank logBlank = GenerateLogBlank(result.Value.EntityId.Value, ip);
                await _mekashronApiService.SaveLog(logBlank);
            }

            return Ok(result.Value);
        }

        private String? TryGetVisitorIp()
        {
            try
            {
                String?[] headerKeys = new[]
                {
                    "CF-Connecting-IP",
                    "X-Forwarded-For",
                    "X-Real-IP"
                };

                foreach (String? key in headerKeys)
                {
                    String? raw = HttpContext.Request.Headers[key].FirstOrDefault();
                    if (!String.IsNullOrWhiteSpace(raw))
                    {
                        String firstIp = raw.Split(',').First().Trim();

                        if (IPAddress.TryParse(firstIp, out var ipParsed))
                        {
                            if (!IsLocalIp(ipParsed))
                                return firstIp;
                        }
                    }
                }

                IPAddress? directIp = HttpContext.Connection.RemoteIpAddress;
                if (directIp != null)
                {
                    String directIpString = directIp.ToString();

                    if (!IsLocalIp(directIp))
                        return directIpString;
                }

                return null;

            }
            catch { return null;  }
        }

        private static bool IsLocalIp(IPAddress ip)
        {
            if (IPAddress.IsLoopback(ip)) return true;

            if (ip.Equals(IPAddress.IPv6Loopback)) return true;

            // Ћокальные диапазоны
            byte[] bytes = ip.GetAddressBytes();

            switch (bytes[0])
            {
                case 10:       // 10.0.0.0 Ц 10.255.255.255
                case 127:      // 127.0.0.1 Ц localhost
                    return true;
                case 172:
                    return bytes[1] >= 16 && bytes[1] <= 31;
                case 192:
                    return bytes[1] == 168;
            }

            return false;
        }

        private CustomFieldsTableBlank GenerateLogBlank(Int32 entityId, String ip) =>
            new CustomFieldsTableBlank
            {
                OlEntityId = Int32.Parse(_configuration["MekashronLogger:olEntityId"]!),
                OlUsername = _configuration["MekashronLogger:olUsername"],
                OlPassword = _configuration["MekashronLogger:olPassword"],
                TableId = Int32.Parse(_configuration["MekashronLogger:tableId"]!),
                RecordId = Int32.Parse(_configuration["MekashronLogger:recordId"]!),
                QueryString = _configuration["MekashronLogger:queryString"],
                EntityId = entityId,
                CurrentDateTimeUTC = DateTime.UtcNow,
                VisitorIp = ip
            };



    [HttpGet("/api/download")]
        public async Task<IActionResult> Download()
        {
            if (!Request.Cookies.TryGetValue("download_allowed", out var allowed) || allowed != "1")
                return Unauthorized("Not allowed");

            String fileUrl = _configuration["MekashronApi:CallCenterV7Url"] ?? throw new Exception("MekashronApi:CallCenterV7Url in appsettings.json are null");

            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(
                fileUrl,
                HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "File unavailable");

            var stream = await response.Content.ReadAsStreamAsync();
            var contentType = response.Content.Headers.ContentType?.MediaType
                    ?? "application/octet-stream";

            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = "mekashroncallcenterV7.exe"
            };
        }
    }
}
