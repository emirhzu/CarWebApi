using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly ICarService _carService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly ICategoryService _categoryService;

        public ServiceManager(IRepositoryManager repositoryManager,
                              ILoggerService logger,
                              IMapper mapper,
                              IConfiguration configuration,
                              UserManager<User> userManager,
                              ICarLinks carLinks)
        {
            _categoryService = new CategoryManager(repositoryManager);
            _carService = new CarManager(repositoryManager, logger, mapper, carLinks, _categoryService);
       
            _authenticationService = new Lazy<IAuthenticationService>(() =>
            new AuthenticateManager(logger, mapper, userManager, configuration));
        }

        public ICarService CarService => _carService;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;

        public ICategoryService CategoryService => _categoryService;
    }
}
