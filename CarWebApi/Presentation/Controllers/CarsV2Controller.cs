using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers
{
    //[ApiVersion("2.0", Deprecated = true)]
    [ApiController]
    [Route("api/cars")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class CarsV2Controller : ControllerBase
    {
        private readonly IServiceManager _manager;

        public CarsV2Controller(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarsAsync()
        {
            var cars = await _manager.CarService.GetCarsAsync(false);
            var carsv2 = cars.Select(c => new
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Years = c.Years,
            });
            return Ok(carsv2);
        }
    }
}
