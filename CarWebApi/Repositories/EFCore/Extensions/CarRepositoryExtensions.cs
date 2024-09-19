using Entities.Models;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace Repositories.EFCore.Extensions
{
    public static class CarRepositoryExtensions
    {
        public static IQueryable<Car> FilterCars(this IQueryable<Car> cars, uint minYears, uint maxYears) =>
            cars.Where(car => car.Years >= minYears && car.Years <= maxYears);

        public static IQueryable<Car> FilterByPrice(this IQueryable<Car> cars, uint minPrice, uint maxPrice) =>
            cars.Where(car => car.Price >= minPrice && car.Price <= maxPrice);

        public static IQueryable<Car> FilterByBrand(this IQueryable<Car> cars, string brand) =>
            string.IsNullOrEmpty(brand) ? cars : cars.Where(car => car.Brand.ToLower() == brand.ToLower());

        public static IQueryable<Car> FilterByModel(this IQueryable<Car> cars, string model) =>
            string.IsNullOrEmpty(model) ? cars : cars.Where(car => car.Model.ToLower() == model.ToLower());

        public static IQueryable<Car> Sort(this IQueryable<Car> cars, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return cars.OrderBy(c => c.Id);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Car>(orderByQueryString);

            if (orderQuery is null)
                return cars.OrderBy(c => c.Id);

            return cars.OrderBy(orderQuery);
        }
    }
}
