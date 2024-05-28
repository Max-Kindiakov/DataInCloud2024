using DataInCloud.Model.Car;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataInCloud.Orchestrators.Car
{
    public class CarOrchestrator : ICarOrchestrator
    {
        private readonly ICarRepository _carRepository;

        public CarOrchestrator(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<Model.Car.Car> CreateAsync(Model.Car.Car entityToCreate)
        {
            return await _carRepository.CreateAsync(entityToCreate);
        }

        public async Task<List<Model.Car.Car>> GetAllAsync()
        {
            return await _carRepository.GetAllAsync();
        }

        public async Task<Model.Car.Car> DeleteAsync(int id)
        {
            return await (_carRepository.DeleteAsync(id));
        }

        public async Task<Model.Car.Car> GetCarAsync(int id)
        {
            var existingCar = await _carRepository.GetCarAsync(id);

            return existingCar;
        }

        public async Task<Model.Car.Car?> EditAsync(Model.Car.Car car)
        {
            var existingCar = await GetCarAsync(car.Id);
            if (existingCar == null)
            {
                return null;
            }

            existingCar.Name = car.Name;

            var modifiedCar = await _carRepository.EditAsync(existingCar);
            return modifiedCar;

        }
    }
}
