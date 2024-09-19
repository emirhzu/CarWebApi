using Entities.Models;
using Entities.RequestFeatures;

namespace Repositories.Contracts
{
    public interface ICarRepository : IRepositoryBase<Car>
    {
        Task<PagedList<Car>> GetCarsAsync(CarParameters carParameters, bool trackChanges);
        Task<List<Car>> GetCarsAsync(bool trackChanges);
        Task<Car> GetACarByIdAsync(int id, bool trackChanges);
        void AddACar(Car car);
        void UpdateACar(Car car);
        void DeleteACar(Car car);
        Task<IEnumerable<Car>> GetAllACarsWithDetailsAsync(bool trackChanges);
    }
}
