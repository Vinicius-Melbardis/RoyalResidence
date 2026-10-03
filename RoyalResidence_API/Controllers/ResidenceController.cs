using Microsoft.AspNetCore.Mvc;

namespace RoyalResidence_API.Controllers
{
    [Route("api/residence")]
    [ApiController]
    public class ResidenceController : ControllerBase
    {
        [HttpGet]
        public string GetResidences()
        {
            return "Get all residences";
        }

        [HttpGet("{id:int}")]
        public string GetResidencesById(int id)
        {
            return "Get residence by ID: "+ id;
        }
    }
}
