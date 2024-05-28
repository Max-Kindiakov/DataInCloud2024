using AutoMapper;
using DataInCloud.Model.Car;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataInCloud.Dal.Car
{
    public class CarRepository : ICarRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CarRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<Model.Car.Car>> GetAllAsync()
        {
            var entities = _context.Cars.ToList();

            var cars = _mapper.Map<List<Model.Car.Car>>(entities);

            return cars;
        }

        public async Task<Model.Car.Car> CreateAsync(Model.Car.Car entityToCreate)
        {
            var entity = _mapper.Map<CarDao>(entityToCreate);

            var createdEntity = await _context.Cars.AddAsync(entity);

            await _context.SaveChangesAsync();

            return _mapper.Map<Model.Car.Car>(createdEntity.Entity);
        }

        public async Task<Model.Car.Car> GetCarAsync(int id)
        {
            var entity = await _context.Cars.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<Model.Car.Car>(entity);
        }

        public async Task<Model.Car.Car> DeleteAsync(int id)
        {
            var carToDelete = _context.Cars.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (carToDelete != null)
            {
                _context.Cars.Remove(carToDelete);
                await _context.SaveChangesAsync();
                return _mapper.Map<Model.Car.Car>(carToDelete);
            }
            return null;
        }


        public async Task<Model.Car.Car> EditAsync(Model.Car.Car inputModel)
        {
            var carEntity = await _context.Cars.FirstAsync(c => c.Id == inputModel.Id);

            carEntity.Name = inputModel.Name;

            await _context.SaveChangesAsync();

            return _mapper.Map<Model.Car.Car>(carEntity);
        }

    }
}
