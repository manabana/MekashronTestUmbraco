using Mekashron.Domain;
using Mekashron.Domain.Services;
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

            String? ipString = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                                ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            IPAddress? ip = ipString.IsWhiteSpace() ? null : IPAddress.Parse(ipString);

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

            var result = await _mekashronApiService.RegisterNewCustomer(blank);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Error?.Message, key = result.Error?.Key });
            }

            return Ok(result.Value);
        }


        [HttpGet("/api/download")]
        public async Task<IActionResult> Download()
        {
            String fileUrl = _configuration["MekashronApi:CallCenterV7Url"] ?? throw new Exception("MekashronApi:CallCenterV7Url in appsettings.json are null");

            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(
                fileUrl,
                HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "File unavailable");

            var stream = await response.Content.ReadAsStreamAsync();
            var contentType = response.Content.Headers.ContentType?.ToString()
                ?? "application/octet-stream";

            return File(
                stream,
                contentType,
                "mekashroncallcenterV7.exe"
            );
        }
    }
}
