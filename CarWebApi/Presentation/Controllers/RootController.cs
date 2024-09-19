using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class RootController : ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;
        public RootController(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        [HttpGet(Name = "GetRoot")]
        public async Task<IActionResult> GetRoot([FromHeader(Name = "Accept")]string mediatype)
        {
            if (mediatype.Contains("application/vnd.emir.apiroot"))
            {
                var list = new List<Link>()
                {
                    new Link ()
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new{}),
                        Rel = "_self",
                        Method = "GET"
                    },
                    new Link ()
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(CarsController.GetCarsAsync), new{}),
                        Rel = "cars",
                        Method = "GET"
                    },
                    new Link ()
                    {
                        Href = _linkGenerator.GetUriByName(HttpContext, nameof(CarsController.AddACarAsync), new{}),
                        Rel = "cars",
                        Method = "POST"
                    },
                };

                return Ok(list);
            }
            return NoContent();
        }
    }
}
