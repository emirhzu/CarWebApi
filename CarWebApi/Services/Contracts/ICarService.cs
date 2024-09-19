using Entities.DataTransferObjects;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using System.Dynamic;

namespace Services.Contracts
{
    public interface ICarService
    {
        Task<(LinkResponse linkResponse, MetaData metaData)> GetCarsAsync(LinkParameters linkParameters, bool trackChanges);
        Task<CarDto> GetACarByIdAsync(int id, bool trackChanges);
        Task<CarDto> AddACarAsync(CarDtoForInsertion car);
        Task UpdateACarAsync(int id, CarDtoForUpdate carDto, bool trackChanges);
        Task DeleteACarAsync(int id, bool trackChanges);
        Task<(CarDtoForUpdate cardtoforupdate, Car car)> GetOneCarForPatchAsync(int id, bool trackChanges);
        Task SaveChangesForPatchAsync(CarDtoForUpdate cardtoforupdate, Car car);
        Task<List<Car>> GetCarsAsync(bool trackChanges);
        Task<IEnumerable<Car>> GetAllACarsWithDetailsAsync(bool trackChanges);
    }
}
