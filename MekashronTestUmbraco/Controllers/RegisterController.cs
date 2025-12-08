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
        private readonly IConfiguration _configuration;

        public RegisterController(IConfiguration configuration, IMekashronApiService mekashronApiService)
        {
            _configuration = configuration;
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

            var blank = new CustomerBlank
            {
                OlEntityId = Int32.Parse(_configuration["RegisterEntityData:olEntityId"]!),
                OlUsername = _configuration["RegisterEntityData:olUsername"]!,
                OlPassword = _configuration["RegisterEntityData:olPassword"]!,
                BusinessId = Int32.Parse(_configuration["RegisterEntityData:BusinessId"]!),
                CategoryId = Int32.Parse(_configuration["RegisterEntityData:CategoryId"]!),
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone
            };

            var result = await _mekashronApiService.RegisterNewCustomer(blank);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Error?.Message, key = result.Error?.Key });
            }

            return Ok(result.Value);
        }
    }
}
