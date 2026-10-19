using Microsoft.AspNetCore.Mvc;
using RoyalResidence_API.Controllers.Data;
using RoyalResidence_API.Models;
using Microsoft.EntityFrameworkCore;
using RoyalResidence.DTO;
using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace RoyalResidence_API.Controllers
{
    [Route("api/residenceamenities")]
    [ApiController]
    // [Authorize(Roles = "Customer, Admin")]
    public class ResidenceAmenitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ResidenceAmenitiesController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        // [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ResidenceAmenitiesDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse<IEnumerable<ResidenceAmenitiesDTO>>>> GetResidencesAmenities()
        {
            var residenceAmenities = await _db.ResidenceAmenities.ToListAsync();
            var dtoResponseResidenceAmenities = _mapper.Map<List<ResidenceAmenitiesDTO>>(residenceAmenities);
            var response = ApiResponse<IEnumerable<ResidenceAmenitiesDTO>>.Ok(dtoResponseResidenceAmenities, "Residence Amenities retrieved successfully");
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        // [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<ResidenceAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ResidenceAmenitiesDTO>>> GetResidencesAmenitiesById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("ResidenceAmenities ID must be greater than 0"));
                }

                var residenceAmenities = await _db.ResidenceAmenities.FirstOrDefaultAsync(u => u.Id == id);
                if (residenceAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"ResidenceAmenities with ID {id} was not found"));
                }
                return Ok(ApiResponse<ResidenceAmenitiesDTO>.Ok(_mapper.Map<ResidenceAmenitiesDTO>(residenceAmenities), "Records retrieved successfully"));
            }

            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while retrieving residence", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ResidenceAmenitiesDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ResidenceAmenitiesDTO>>> CreateResidenceAmenities(ResidenceAmenitiesCreateDTO residenceAmenitiesDTO)
        {
            try
            {
                if (residenceAmenitiesDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Residence Amenities data is required"));
                }

                var residenceExists = await _db.Residences.FirstOrDefaultAsync(u => u.Id == residenceAmenitiesDTO.ResidenceId);

                if (residenceExists == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A residence with the ID '{residenceAmenitiesDTO.ResidenceId}' does not exists"));
                }

                ResidenceAmenities residenceAmenities = _mapper.Map<ResidenceAmenities>(residenceAmenitiesDTO);
                residenceAmenities.CreatedDate = DateTime.Now;
                await _db.ResidenceAmenities.AddAsync(residenceAmenities);
                await _db.SaveChangesAsync();

                var response = ApiResponse<ResidenceAmenitiesDTO>.CreatedAt(_mapper.Map<ResidenceAmenitiesDTO>(residenceAmenities), "Residence Amenities created successfully");
                return CreatedAtAction(nameof(CreateResidenceAmenities), new { id = residenceAmenities.Id }, response);
            }

            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while creating residence amenities", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<ResidenceAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<ResidenceAmenitiesDTO>>> UpdateResidenceAmenities(int id, ResidenceAmenitiesUpdateDTO residenceAmenitiesDTO)
        {
            try
            {
                if (residenceAmenitiesDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Residence Amenities data is required"));
                }

                if (id != residenceAmenitiesDTO.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Residence Amenities ID in URL does not match ResidenceAmenities ID in request body"));
                }

                var residenceExists = await _db.Residences.FirstOrDefaultAsync(u => u.Id == residenceAmenitiesDTO.ResidenceId);

                if (residenceExists == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A residence with the ID '{residenceAmenitiesDTO.ResidenceId}' does not exists"));
                }

                var existingResidenceAmenities = await _db.ResidenceAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if(existingResidenceAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Residence Amenities with ID {id} was not found"));
                }


                _mapper.Map(residenceAmenitiesDTO, existingResidenceAmenities);
                existingResidenceAmenities.UpdatedDate = DateTime.Now;

                await _db.SaveChangesAsync();
                var response = ApiResponse<ResidenceAmenitiesDTO>.Ok(_mapper.Map<ResidenceAmenitiesDTO>(existingResidenceAmenities), "Residence Amenities updated successfully");
                return Ok(response);
            }

            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while updating residence amenities", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteResidenceAmenities(int id)
        {
            try
            {
                var existingResidenceAmenities = await _db.ResidenceAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if (existingResidenceAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Residence Amenities with ID {id} was not found"));
                }

                _db.ResidenceAmenities.Remove(existingResidenceAmenities);
                await _db.SaveChangesAsync();

                var response = ApiResponse<object>.NoContent("Residence Amenities deleted successfully");
                return Ok(response);
            }

            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, "An error occurred while deleting residence amenities", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}
