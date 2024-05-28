namespace DataInCloud.Model.Car
{
    public interface ICarRepository
    {
        Task<List<Car>> GetAllAsync();

        Task<Car> CreateAsync(Car entityToCreate);

        Task<Car> GetCarAsync(int id);
        Task<Car> DeleteAsync(int id);

        Task<Car> EditAsync(Car inputModel);
    }
}
