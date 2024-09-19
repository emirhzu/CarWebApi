using Entities.DataTransferObjects;
using Entities.LinkModels;
using Microsoft.AspNetCore.Http;

namespace Services.Contracts
{
    public interface ICarLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<CarDto> carsDto, string fields, HttpContext httpContext);
    }
}
