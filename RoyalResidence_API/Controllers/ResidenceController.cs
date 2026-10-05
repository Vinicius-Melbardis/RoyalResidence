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
        public async Task<ActionResult<IEnumerable<Residence>>> GetResidences()
        {
            return Ok(await _db.Residences.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Residence>> GetResidencesById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Residence ID must be greater than 0");
                }

                var residence = await _db.Residences.FirstOrDefaultAsync(u => u.Id == id);
                if (residence == null)
                {
                    return NotFound($"Residence with ID {id} was not found");
                }
                return Ok(residence);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while retrieving residence with ID {id}: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Residence>> CreateResidence(ResidenceCreateDTO residenceDTO)
        {
            try
            {
                if (residenceDTO == null)
                {
                    return BadRequest("Residence data is required");
                }

                Residence residence = _mapper.Map<Residence>(residenceDTO);

                await _db.Residences.AddAsync(residence);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetResidencesById), new { id = residence.Id }, residence);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while creating residence: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Residence>> UpdateResidence(int id, ResidenceUpdateDTO residenceDTO)
        {
            try
            {
                if (residenceDTO == null)
                {
                    return BadRequest("Residence data is required");
                }

                if (id != residenceDTO.Id)
                {
                    return BadRequest("Residence ID in URL does not match Residence ID in request body");
                }

                var existingResidence = await _db.Residences.FirstOrDefaultAsync(u => u.Id == id);

                if(existingResidence == null)
                {
                    return NotFound($"Residence with ID {id} was not found");
                }

                _mapper.Map(residenceDTO, existingResidence);
                existingResidence.UpdatedDate = DateTime.Now;

                await _db.SaveChangesAsync();

                return Ok(residenceDTO);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while updating residence: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Residence>> DeleteResidence(int id)
        {
            try
            {
                var existingResidence = await _db.Residences.FirstOrDefaultAsync(u => u.Id == id);

                if (existingResidence == null)
                {
                    return NotFound($"Residence with ID {id} was not found");
                }

                _db.Residences.Remove(existingResidence);
                await _db.SaveChangesAsync();

                return NoContent();
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while updating residence: {ex.Message}");
            }
        }
    }
}
