using Entities.DataTransferObjects;
using Entities.RequestFeatures;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Text.Json;

namespace Presentation.Controllers
{
    //[ApiVersion("1.0")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    [ApiController]
    [Route("api/Cars")]
    //[ResponseCache(CacheProfileName = "5mins")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CarsController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public CarsController(IServiceManager manager)
        {
            _manager = manager;
        }

        [Authorize(Roles = "User, Editor, Admin")]
        [HttpHead]
        [HttpGet(Name = "GetCarsAsync")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        //[ResponseCache(Duration = 60)] // Cache
        //[HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)] // Cache
        public async Task<IActionResult> GetCarsAsync([FromQuery] CarParameters carParameters)
        {
            var linkParameters = new LinkParameters()
            {
                CarParameters = carParameters,
                HttpContext = HttpContext
            };
            var result = await _manager.CarService.GetCarsAsync(linkParameters, false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

            return result.linkResponse.HasLinks ?
                Ok(result.linkResponse.LinkedEntities) :
                Ok(result.linkResponse.ShapedEntities);
        }

        [Authorize]
        [HttpGet("details")]
        public async Task<IActionResult> GetAllACarsWithDetailsAsync()
        {
            return Ok(await _manager.CarService.GetAllACarsWithDetailsAsync(false));
        }

        [Authorize(Roles = "User, Editor, Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetACarByIdAsync([FromRoute(Name = "id")] int id)
        {
            var car = await _manager.CarService.GetACarByIdAsync(id, false);
            return Ok(car);
        }

        [Authorize(Roles = "Editor, Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost(Name = "AddACarAsync")]
        public async Task<IActionResult> AddACarAsync([FromBody] CarDtoForInsertion carDto)
        {
            var car = await _manager.CarService.AddACarAsync(carDto);
            return StatusCode(201, car);
        }

        [Authorize(Roles = "Editor, Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdetaCarAsync([FromRoute(Name = "id")] int id, [FromBody] CarDtoForUpdate carDto)
        {
            await _manager.CarService.UpdateACarAsync(id, carDto, true);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteACarAsync([FromRoute(Name = "id")] int id)
        {
            await _manager.CarService.DeleteACarAsync(id, false);
            return NoContent();
        }

        [Authorize(Roles = "Editor, Admin")]
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateOneThingOnCarAsync([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<CarDtoForUpdate> carPatch)
        {
            if (carPatch is null)
                return BadRequest("Patch document is null.");

            var result = await _manager.CarService.GetOneCarForPatchAsync(id, false);

            carPatch.ApplyTo(result.cardtoforupdate, ModelState);

            TryValidateModel(result.cardtoforupdate);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _manager.CarService.SaveChangesForPatchAsync(result.cardtoforupdate, result.car);
            return NoContent();
        }

        [Authorize(Roles = "Editor, Admin")]
        [HttpOptions]
        public IActionResult GetCarOptions()
        {
            Response.Headers.Add("Allow", "GET, PUT, POST, PATCH, DELETE, HEAD, OPTIONS");
            return Ok();
        }
    }
}
