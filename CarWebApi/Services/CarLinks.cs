using Entities.DataTransferObjects;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Services.Contracts;

namespace Services
{
    public class CarLinks : ICarLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<CarDto> _dataShaper;

        public CarLinks(LinkGenerator linkGenerator, IDataShaper<CarDto> shaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = shaper;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<CarDto> carsDto, 
                                            string fields, 
                                            HttpContext httpContext)
        {
            var shapedCars = ShapeData(carsDto, fields);
            if (ShouldGenerateLink(httpContext))
                return ReturnLinkedCars(carsDto, fields, httpContext, shapedCars);
            return ReturnShapedCars(shapedCars);
        }

        private LinkResponse ReturnLinkedCars(IEnumerable<CarDto> carsDto, string fields, HttpContext httpContext, List<Entity> shapedCars)
        {
            var carDtoList = carsDto.ToList();

            for (int index = 0; index < carDtoList.Count(); index++)
            {
                var carLinks = CreateForCar(httpContext, carDtoList[index], fields);
                shapedCars[index].Add("Links", carLinks);
            }

            var carCollection = new LinkCollectionWrapper<Entity>(shapedCars);
            CreateForCars(httpContext, carCollection);
            return new LinkResponse { HasLinks = true, LinkedEntities = carCollection };
        }

        private LinkCollectionWrapper<Entity> CreateForCars(HttpContext httpContext, LinkCollectionWrapper<Entity> carCollectionWrapper)
        {
            carCollectionWrapper.Links.Add(new Link() 
            {
                Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}",
                Rel = "self",
                Method = "GET",
            });
            return carCollectionWrapper;
        }

        private List<Link> CreateForCar(HttpContext httpContext, CarDto carDto, string fields)
        {
            var links = new List<Link>()
            {
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}" +
                           $"/{carDto.id}",
                    Rel = "self",
                    Method = "GET"
                },
                new Link()
                {
                    Href = $"/api/{httpContext.GetRouteData().Values["controller"].ToString().ToLower()}",
                    Rel = "create",
                    Method = "POST"
                },
            };
            return links;
        }

        private LinkResponse ReturnShapedCars(List<Entity> shapedCars)
        {
            return new LinkResponse() { ShapedEntities = shapedCars };
        }

        private bool ShouldGenerateLink(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType
                .SubTypeWithoutSuffix
                .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private List<Entity> ShapeData(IEnumerable<CarDto> carsDto, string fields)
        {
            return _dataShaper.ShapeData(carsDto, fields).Select(c => c.Entity).ToList();
        }
    }
}
