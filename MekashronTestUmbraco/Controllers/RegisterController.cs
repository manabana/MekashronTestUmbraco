using Microsoft.AspNetCore.Mvc;
using Mekashron.Domain;
using Mekashron.Domain.Services;

namespace MekashronTestUmbraco.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IMekashronApiService _mekashronApiService;

        public RegisterController(IMekashronApiService mekashronApiService)
        {
            _mekashronApiService = mekashronApiService;
        }

        public class RegisterRequest
        {
            public string? Name { get; set; }
            public string? Company { get; set; }
            public string? Phone { get; set; }
            public string? Email { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterRequest request)
        {
            if (request is null) return BadRequest(new { error = "Request is empty" });

            // Build a CustomerBlank with sensible defaults so service validation passes
            var blank = new CustomerBlank
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                BusinessId = 1,
                OlEntityId = 1,
                CategoryId = 1,
                // OlUsername must be more than one character to avoid accidental regex match in service
                OlUsername = (request.Name ?? "user").Replace(" ", "") + System.DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                // a simple default password that meets length requirement
                OlPassword = GeneratePassword(8)
            };

            var result = await _mekashronApiService.RegisterNewCustomer(blank);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Error?.Message, key = result.Error?.Key });
            }

            // Return the deserialized MekashronRegisterResponse to the caller
            return Ok(result.Value);
        }

        private static string GeneratePassword(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var sb = new System.Text.StringBuilder(length);
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var buffer = new byte[4];
            for (int i = 0; i < length; i++)
            {
                rng.GetBytes(buffer);
                var idx = System.BitConverter.ToUInt32(buffer, 0) % (uint)chars.Length;
                sb.Append(chars[(int)idx]);
            }
            return sb.ToString();
        }
    }
}
