using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla.DTO;

namespace RoyalVilla_API.Controllers.v1
{
    
    //[ApiExplorerSettings(GroupName = "v1")]
    [Route("api/villa-amenitie")]
    [ApiController]
    //[Authorize(Roles = "Customer,Admin")]
    public class VillaAmenitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaAmenitiesController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmenitiesDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaAmenitiesDTO>>>> GetVillaAmenities()
        {
            var villaAmenities = await _db.VillaAmenities.ToListAsync();
            var dtoResponseVillaAmenities = _mapper.Map<List<VillaAmenitiesDTO>>(villaAmenities);
            var response = ApiResponse<List<VillaAmenitiesDTO>>.Ok(dtoResponseVillaAmenities, "villa amenities retrieved successfully");

            return Ok(response);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> GetVillaAmenitiesById(int id)
        {
            try
            {

                if (id <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("villa amenities id must be greater then 0"));
                }

                var villaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if (villaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"villa amenities with id {id} was not found"));
                }

                return Ok(ApiResponse<VillaAmenitiesDTO>.Ok(_mapper.Map<VillaAmenitiesDTO>(villaAmenities), "records retrieved successfully"));
            }
            catch(Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"an error occured when creating the villa {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> CreateVillaAmenities(VillaAmenitiesCreateDTO villaAmenitiesCreateDTO)
        {
            try
            {
                if (villaAmenitiesCreateDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("villa is required"));
                }

                var villaExist = await _db.Vilas.FirstOrDefaultAsync(u => u.Id == villaAmenitiesCreateDTO.VillaId);

                if (villaExist == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"a villa amenities with the ID {villaAmenitiesCreateDTO.VillaId} does not exist"));
                }

                VillaAmenities villaAmenities = _mapper.Map<VillaAmenities>(villaAmenitiesCreateDTO);
                villaAmenities.CreateDate = DateTime.Now;
                await _db.AddAsync(villaAmenities);
                await _db.SaveChangesAsync();

                var response = ApiResponse<VillaAmenitiesDTO>.CreatedAt(_mapper.Map<VillaAmenitiesDTO>(villaAmenities), "villa created successfully");

                return CreatedAtAction(nameof(CreateVillaAmenities), new { id = villaAmenities.Id }, response);

            }
            catch(Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"an error occured when creating the villa {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> UpdateVillaAmenities(int id, VillaAmenitiesUpdateDTO villaAmenitiesUpdateDTO)
        {
            try
            {
                if (villaAmenitiesUpdateDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("villa amenities is required"));
                }
                if (villaAmenitiesUpdateDTO.Id != id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("villa amenities ID in url does not match villa amenities ID in request body"));
                }

                var villaExist = await _db.Vilas.FirstOrDefaultAsync(u => u.Id == villaAmenitiesUpdateDTO.VillaId);

                if (villaExist == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"a villa amenities with the ID {villaAmenitiesUpdateDTO.VillaId} does not exist"));
                }

                var existingVillaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVillaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"villa amenities with id {id} was not found"));
                }

                
                _mapper.Map(villaAmenitiesUpdateDTO, existingVillaAmenities);
                existingVillaAmenities.UpdateDate = DateTime.Now;
                
                await _db.SaveChangesAsync();

                var response = ApiResponse<VillaAmenitiesDTO>.Ok(_mapper.Map<VillaAmenitiesDTO>(existingVillaAmenities), "villa amenities updated successfully");
                
                return Ok(response);

            }
            catch (Exception ex) 
            {
                var errorResponse = ApiResponse<object>.Error(500, $"an error ocured when creating the villa {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVillaAmenities(int id)
        {
            try
            {
                var existingVillaAmenities = await _db.VillaAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVillaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"villa amenities with id {id} was not found"));
                }

                _db.VillaAmenities.Remove(existingVillaAmenities);
                await _db.SaveChangesAsync();

                var response = ApiResponse<object>.NoContent("villa amenities deleted successfully");

                return Ok(response);
            }
            catch(Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"an error ocured when creating the villa {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }

        }


    }
}
