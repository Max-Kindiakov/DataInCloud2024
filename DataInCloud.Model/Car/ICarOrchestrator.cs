namespace DataInCloud.Model.Car
{
    public interface ICarOrchestrator
    {
        Task<List<Car>> GetAllAsync();

        Task<Car> CreateAsync(Car entityToCreate);

        Task<Car> GetCarAsync(int id);

        Task<Model.Car.Car> DeleteAsync (int id);

        Task<Car> EditAsync(Car inputModel);
    }
}
