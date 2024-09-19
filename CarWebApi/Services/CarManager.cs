using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CarManager : ICarService
    {
        private readonly ICategoryService _categoryService;
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly ICarLinks _carLinks;

        public CarManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper, ICarLinks carLinks, ICategoryService categoryService)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
            _carLinks = carLinks;
            _categoryService = categoryService;
        }

        public async Task<CarDto> AddACarAsync(CarDtoForInsertion carDto)
        {
            var category = await _categoryService.GetOneCategoryByIdAsync(carDto.CategoryId,false);
            var entity = _mapper.Map<Car>(carDto);
            entity.CategoryId = carDto.CategoryId;
            _manager.Car.AddACar(entity);
            await _manager.SaveAsync();
            return _mapper.Map<CarDto>(entity);
        }

        public async Task DeleteACarAsync(int id, bool trackChanges)
        {
            var entity = await GetOneCarByIdAndCheckExits(id, trackChanges);
            _manager.Car.DeleteACar(entity);
            await _manager.SaveAsync();
        }

        public async Task<CarDto> GetACarByIdAsync(int id, bool trackChanges)
        {
            var entity = await GetOneCarByIdAndCheckExits(id, trackChanges);
            return _mapper.Map<CarDto>(entity);
        }

        public async Task<IEnumerable<Car>> GetAllACarsWithDetailsAsync(bool trackChanges)
        {
            return await _manager.Car.GetAllACarsWithDetailsAsync(trackChanges);
        }

        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetCarsAsync(LinkParameters linkParameters, bool trackChanges)
        {
            if (!linkParameters.CarParameters.ValidYearsRange)
                throw new YearsOutofRangeBadRequestException();
            if (!linkParameters.CarParameters.ValidPriceRange)
                throw new PriceOutofRangeBadRequestException();

            var carsWithMetaData = await _manager.Car.GetCarsAsync(linkParameters.CarParameters, trackChanges);
            var carsDto = _mapper.Map<IEnumerable<CarDto>>(carsWithMetaData);
            var links = _carLinks.TryGenerateLinks(carsDto, 
                linkParameters.CarParameters.Fields, 
                linkParameters.HttpContext);

            return (linkResponse: links,metaData: carsWithMetaData.MetaData);
        }

        public async Task<List<Car>> GetCarsAsync(bool trackChanges)
        {
            var cars = await _manager.Car.GetCarsAsync(trackChanges);
            return cars;
        }

        public async Task<(CarDtoForUpdate cardtoforupdate, Car car)> GetOneCarForPatchAsync(int id, bool trackChanges)
        {
            var entity = await GetOneCarByIdAndCheckExits(id, trackChanges);

            var carDtoForUpdate = _mapper.Map<CarDtoForUpdate>(entity);
            return (carDtoForUpdate, entity);
        }

        public async Task SaveChangesForPatchAsync(CarDtoForUpdate cardtoforupdate, Car car)
        {
            _mapper.Map(cardtoforupdate, car);
            _manager.Car.Update(car);
            await _manager.SaveAsync();
        }

        public async Task UpdateACarAsync(int id, CarDtoForUpdate carDto, bool trackChanges)
        {
            var entity = await GetOneCarByIdAndCheckExits(id, trackChanges);

            //Mapping
            //entity.Brand = carDto.Brand;
            //entity.Model = carDto.Model;
            //entity.Years = carDto.Years;
            //entity.Price = carDto.Price;
            _mapper.Map(carDto, entity);

            _manager.Car.Update(entity);
            await _manager.SaveAsync();
        }

        private async Task<Car> GetOneCarByIdAndCheckExits(int id, bool trackChanges)
        {
            var entity = await _manager.Car.GetACarByIdAsync(id, trackChanges);
            if (entity is null)
                throw new CarNotFoundException(id);
            return entity;
        }
    }
}
