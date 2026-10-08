using Microsoft.AspNetCore.Mvc;
using RoyalResidence_API.Controllers.Data;
using RoyalResidence_API.Models;
using Microsoft.EntityFrameworkCore;
using RoyalResidence_API.Models.DTO;
using System.Data;
using AutoMapper;

namespace RoyalResidence_API.Controllers
{
    [Route("api/residence")]
    [ApiController]
    public class ResidenceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ResidenceController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResidenceDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse<IEnumerable<ResidenceDTO>>>> GetResidences()
        {
            var residences = await _db.Residences.ToListAsync();
            var dtoResponseResidence = _mapper.Map<List<ResidenceDTO>>(residences);
            var response = ApiResponse<IEnumerable<ResidenceDTO>>.Ok(dtoResponseResidence, "Residences retrieved successfully");
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<ResidenceDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ResidenceDTO>>> GetResidencesById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("Residence ID must be greater than 0"));
                }

                var residence = await _db.Residences.FirstOrDefaultAsync(u => u.Id == id);
                if (residence == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Residence with ID {id} was not found"));
                }
                return Ok(ApiResponse<ResidenceDTO>.Ok(_mapper.Map<ResidenceDTO>(residence), "Records retrieved successfully"));
            }

            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while retrieving residence", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ResidenceDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ResidenceCreateDTO>>> CreateResidence(ResidenceCreateDTO residenceDTO)
        {
            try
            {
                if (residenceDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Residence data is required"));
                }

                var duplicateResidence = await _db.Residences.FirstOrDefaultAsync(u => u.Name.ToLower() == residenceDTO.Name.ToLower());

                if (duplicateResidence != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A residence with the name '{residenceDTO.Name}' already exists"));
                }

                Residence residence = _mapper.Map<Residence>(residenceDTO);

                await _db.Residences.AddAsync(residence);
                await _db.SaveChangesAsync();

                var response = ApiResponse<ResidenceDTO>.CreatedAt(_mapper.Map<ResidenceDTO>(residence), "Residence created successfully");
                return CreatedAtAction(nameof(GetResidencesById), new { id = residence.Id }, response);
            }

            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while creating residence", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<ResidenceDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ResidenceDTO>>> UpdateResidence(int id, ResidenceUpdateDTO residenceDTO)
        {
            try
            {
                if (residenceDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Residence data is required"));
                }

                if (id != residenceDTO.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Residence ID in URL does not match Residence ID in request body"));
                }

                var existingResidence = await _db.Residences.FirstOrDefaultAsync(u => u.Id == id);

                if(existingResidence == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Residence with ID {id} was not found"));
                }

                var duplicateResidence = await _db.Residences.FirstOrDefaultAsync(u => u.Name.ToLower() == residenceDTO.Name.ToLower()
                && u.Id != id);

                if (duplicateResidence != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A residence with the name '{residenceDTO.Name}' already exists"));
                }

                _mapper.Map(residenceDTO, existingResidence);
                existingResidence.UpdatedDate = DateTime.Now;

                await _db.SaveChangesAsync();
                var response = ApiResponse<ResidenceDTO>.Ok(_mapper.Map<ResidenceDTO>(residenceDTO), "Residence updated successfully");
                return Ok(response);
            }

            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while updating residence", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteResidence(int id)
        {
            try
            {
                var existingResidence = await _db.Residences.FirstOrDefaultAsync(u => u.Id == id);

                if (existingResidence == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Residence with ID {id} was not found"));
                }

                _db.Residences.Remove(existingResidence);
                await _db.SaveChangesAsync();

                var response = ApiResponse<object>.NoContent("Residence deleted successfully");
                return Ok(response);
            }

            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while deleting residence", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}
