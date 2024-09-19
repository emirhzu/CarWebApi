using Repositories.Contracts;
using Repositories.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly ICarRepository _carRepository;
        private readonly ICategoryRepository _categoryRepository;

        public RepositoryManager(RepositoryContext context, ICarRepository carRepository, ICategoryRepository categoryRepository)
        {
            _context = context;
            _carRepository = carRepository;
            _categoryRepository = categoryRepository;
        }

        public ICarRepository Car => _carRepository;

        public ICategoryRepository Category => _categoryRepository;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
