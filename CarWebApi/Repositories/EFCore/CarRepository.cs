using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EfCore;
using Repositories.EFCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Repositories.EFCore
{
    public sealed class CarRepository : RepositoryBase<Car>, ICarRepository
    {
        public CarRepository(RepositoryContext context) : base(context)
        {
        }

        public void AddACar(Car car) => Create(car);

        public void DeleteACar(Car car) => Delete(car);

        public async Task<Car> GetACarByIdAsync(int id, bool trackChanges) => await FindByCondition(c => c.Id.Equals(id), trackChanges).FirstOrDefaultAsync();

        public async Task<IEnumerable<Car>> GetAllACarsWithDetailsAsync(bool trackChanges)
        {
            return await _context.Car.Include(c => c.Category).OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<PagedList<Car>> GetCarsAsync(CarParameters carParameters, bool trackChanges)
        {
            // Validate carParameters before querying the database
            if (!carParameters.ValidYearsRange)
            {
                throw new ArgumentException("Invalid years range specified.");
            }

            if (!carParameters.ValidPriceRange)
            {
                throw new ArgumentException("Invalid price range specified.");
            }

            var carsQuery = FindAll(trackChanges)
                .FilterCars(carParameters.MinYears, carParameters.MaxYears)
                .FilterByPrice(carParameters.MinPrice, carParameters.MaxPrice)
                .FilterByBrand(carParameters.Brand)
                .FilterByModel(carParameters.Model)
                .Sort(carParameters.OrderBy);

            var totalCount = await carsQuery.CountAsync();

            var cars = await carsQuery
            .Skip((carParameters.PageNumber - 1) * carParameters.PageSize)
            .Take(carParameters.PageSize)
            .ToListAsync();

            return new PagedList<Car>(cars, totalCount, carParameters.PageNumber, carParameters.PageSize);
        }

        public async Task<List<Car>> GetCarsAsync(bool trackChanges)
        {
           return await FindAll(trackChanges).OrderBy(c => c.Id).ToListAsync();
        }

        public void UpdateACar(Car car) => Update(car);
    }
}
