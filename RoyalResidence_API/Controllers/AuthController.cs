using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalResidence_API.Controllers.Data;
using RoyalResidence_API.Models.DTO;

namespace RoyalResidence_API.Controllers
{
    [Route("api/[auth]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {

        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResidenceDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse<UserDTO>>> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            // Auth service
            var response = ApiResponse<UserDTO>.Ok(null, "User created successfully");
            return Ok(response);
        }

    }
}
