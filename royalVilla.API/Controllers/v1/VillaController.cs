using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla.DTO;

namespace RoyalVilla_API.Controllers.v1
{
    [Route("api/villa")]
    ////[ApiExplorerSettings(GroupName = "v1")]
    //[ApiVersion("1.0")]
    [ApiController]

    [Authorize(Roles = "Customer,Admin")]
    public class VillaController: ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }



        [HttpGet]
        /*[Authorize(Roles = "Admin")] */ // only admin can have access 
        // for having the response in scalar we wrote ProducesResponseType
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaDTO>>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaDTO>>>> GetVillas() // when we put it in ok function its type is no longer IEnumerable it will be ActionResult
        {
            var villas = await _db.Vilas.ToListAsync();
            var dtoResponseVilla = _mapper.Map<List<VillaDTO>>(villas);
            var response = ApiResponse<IEnumerable<VillaDTO>>.Ok(dtoResponseVilla, "villas retrieved successfully");

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]  // anyone can access to this method
        [ProducesResponseType(typeof(ApiResponse<VillaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> GetVillaByID(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("villa id must be greater then 0"));
                    
                }

                var villa = await _db.Vilas.FirstOrDefaultAsync(u => u.Id == id);

                if (villa == null) 
                {
                    return NotFound(ApiResponse<object>.NotFound($"villa with id {id} was not found"));
                }

                return Ok(ApiResponse<VillaDTO>.Ok(_mapper.Map<VillaDTO>(villa), "records retrieved successfully"));
                
            }
            catch (Exception ex) 
            {
                var errorResponse = ApiResponse<object>.Error(500, $"an error occured when creating the villa {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
               
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<VillaDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<VillaCreateDTO>>> CreateVilla(VillaCreateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("villa is required"));
                }

                var duplicateVilla = await _db.Vilas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower());

                if (duplicateVilla != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"a villa with the name {villaDTO.Name} already exist")); // instead of BadRequest we used this method returns 409 status code
                }

                Villa villa = _mapper.Map<Villa>(villaDTO); // we have to mention source and the destination

                await _db.AddAsync(villa);
                await _db.SaveChangesAsync();

                var response = ApiResponse<VillaDTO>.CreatedAt(_mapper.Map<VillaDTO>(villa), "villa created successfully");
                return CreatedAtAction(nameof(GetVillaByID), new { id = villa.Id }, response); // instead of using ok method we used this (return where created) returns 201 created
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"an error ocured when creating the villa {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }


        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<VillaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> UpdateVilla(int id,VillaUpdateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("villa is required"));
                }

                if (id != villaDTO.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("villa ID in url does not match villa ID in request body"));
                }

                var existingVilla = await _db.Vilas.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVilla == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"villa with id {id} was not found"));
                }

                var duplicateVilla = await _db.Vilas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower() && u.Id != id);

                if (duplicateVilla != null)
                {
                    return Conflict(ApiResponse<object>.Conflict( $"a villa with the name {villaDTO.Name} already exist")); // instead of BadRequest we used this method returns 409 status code
                }

                _mapper.Map(villaDTO, existingVilla); // we have to mention source and the destination
                existingVilla.UpdateData = DateTime.Now;
              
               await _db.SaveChangesAsync();

                var response = ApiResponse<VillaDTO>.Ok(_mapper.Map<VillaDTO>(villaDTO), "villa updated successfully");
                return Ok(villaDTO);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"an error ocured when creating the villa {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVilla(int id)
        {
            try
            {
               
                var existingVilla = await _db.Vilas.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVilla == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"villa with id {id} was not found"));
                }

               _db.Vilas.Remove(existingVilla);

                await _db.SaveChangesAsync();

                var response = ApiResponse<object>.NoContent("villa deleted successfully");
                return Ok(response); 

                // we pass this when we know the api is not required to pass any content
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"an error ocured when creating the villa {ex.Message}", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }


    }
}
